using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XingXing.GlobalGameJam.Y2026
{
    public class MaskController : MonoBehaviour
    {
        [SerializeField] private InputAction m_Controller;
        [SerializeField] private GameObject m_Mask1;
        [SerializeField] private GameObject m_Mask2;
        private DissolveController _dissolveController;
        private bool _mask;

        private void Awake()
        {
            _dissolveController = FindAnyObjectByType<DissolveController>(FindObjectsInactive.Include);
        }
        private void OnEnable()
        {
            m_Controller.performed += OnMaskChanged;
            m_Controller.Enable();
        }
        private void OnDisable()
        {
            m_Controller.performed -= OnMaskChanged;
            m_Controller.Disable();
        }
        private void Start()
        {
            m_Mask1.SetActive(_mask);
            m_Mask2.SetActive(!_mask);
        }

        private void OnMaskChanged(InputAction.CallbackContext context)
        {
            _dissolveController.Dissolve(() =>
            {
                _mask = !_mask;
                m_Mask1.SetActive(_mask);
                m_Mask2.SetActive(!_mask);
            });

        }
    }
}
