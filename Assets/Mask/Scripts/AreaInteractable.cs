using UnityEngine;
using UnityEngine.Events;

namespace XingXing.GlobalGameJam.Y2026
{
    [RequireComponent(typeof(BoxCollider))]
    public class AreaInteractable : MonoBehaviour
    {
        [SerializeField] private string m_Tag = "Player";
        [SerializeField] private UnityEvent m_Events;

        private bool _triggered;
        private BoxCollider _collider;
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }
        private void Start()
        {
            _collider.isTrigger = true;
            _collider.size = new Vector3(0.25f,1,0.25f);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(_triggered || string.IsNullOrEmpty(m_Tag)) return;

            if (other.CompareTag(m_Tag))
            {
                m_Events?.Invoke();
                _triggered = true;
            }
        }
    }
}
