using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

namespace XingXing.GlobalGameJam.Y2026
{
    public class PlayerController : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool IsMobile();
#endif
        public bool isGround => _characterController.isGrounded;

        [Header("Parameters")]
        [SerializeField] private bool m_InitCanMove;
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_SprintSpeed;
        [SerializeField] private float m_RotateSmooth;
        [SerializeField] private float m_Garvity;

        [Header("Animations")]
        [SerializeField] private Animator m_Animator;

        [Header("Controller")]
        [SerializeField] private InputAction m_MoveInput;
        [SerializeField] private InputAction m_SprintInput;
        [SerializeField] private GameObject m_Joystick;
        [SerializeField] private GameObject m_HindKeyboard;

        private Vector2 _moveDirection;
        private Vector3 _velocity;
        private bool _isSprint,_block,_moving;
        private CharacterController _characterController;
        private DissolveController _dissoveController;
        private GameManager _gameManager;
        private void Awake()
        {
            _gameManager = FindAnyObjectByType<GameManager>(FindObjectsInactive.Include);
            _dissoveController = FindAnyObjectByType<DissolveController>(FindObjectsInactive.Include);
            _characterController = GetComponent<CharacterController>();
        }
        private void OnEnable()
        {
            m_MoveInput.performed += OnMove;
            m_MoveInput.canceled += OnRest;
            m_MoveInput.Enable();

            m_SprintInput.performed += IsSprint;
            m_SprintInput.canceled += IsWalk;
            m_SprintInput.Enable();

            _dissoveController.onDissolveChanged += OnDissoveChanged;
        }
        private void OnDisable()
        {
            m_MoveInput.performed -= OnMove;
            m_MoveInput.canceled -= OnRest;
            m_MoveInput.Disable();

            m_SprintInput.performed -= IsSprint;
            m_SprintInput.canceled -= IsWalk;
            m_SprintInput.Disable();

            _dissoveController.onDissolveChanged -= OnDissoveChanged;
        }
        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (IsMobile())
            m_Joystick.SetActive(true);
        else
            m_Joystick.SetActive(false);
#else
            if (Application.isMobilePlatform)
                m_Joystick.SetActive(true);
#endif

            m_HindKeyboard.SetActive(!m_Joystick.activeSelf);
            _block = !m_InitCanMove;
        }
        private void Update()
        {
            if(!_block)
            Move();
        }
        private void LateUpdate()
        {
            if (m_Animator)
            {
                m_Animator.SetBool("fall",!isGround);
                m_Animator.SetBool("walk", _moving && _characterController.isGrounded);
            }

            if(transform.localPosition.y < -10)
            {
                _gameManager.MissionFail();
            }
        }

        public void CanMove(bool can) => _block = !can;

        private void OnDissoveChanged()
        {
            _characterController.Move(Vector3.forward * 0.0001f);
        }
        private void Move()
        {
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y -= m_Garvity * Time.deltaTime;
            Vector3 dir = new Vector3(_moveDirection.x, 0, _moveDirection.y);

            if (dir.sqrMagnitude > 0.001f)
            {
                var rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    rot,
                    m_RotateSmooth * Time.deltaTime
                );
                _moving = true;
            }
            else
            {
                _moving = false;
            }

            Vector3 move =
                dir * (_isSprint ? m_SprintSpeed : m_MoveSpeed) +
                _velocity;

            _characterController.Move(move * Time.deltaTime);
        }

        #region Controller
        private void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
        private void OnRest(InputAction.CallbackContext context)
        {
            _moveDirection = Vector2.zero;
        }
        private void IsSprint(InputAction.CallbackContext context)
        {
            _isSprint = true;
        }
        private void IsWalk(InputAction.CallbackContext context)
        {
            _isSprint = false;
        }
        #endregion
    }

}
