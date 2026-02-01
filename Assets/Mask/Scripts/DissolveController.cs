using AmazingAssets.AdvancedDissolve;
using System;
using System.Collections;
using UnityEngine;

namespace XingXing.GlobalGameJam.Y2026
{
    public class DissolveController : MonoBehaviour
    {
        public Action onDissolveChanged = delegate { };

        [SerializeField] private Transform m_DissolveObject;

        [Header("[ Floor ]")]
        [SerializeField] private Transform m_ParentFloor1;
        [SerializeField] private Transform m_ParentFloor2;
        [SerializeField] private AudioSource m_Source;
        private Collider[] _colliderFloor1, _colliderFloor2;

        [Header("[ Dissolve Parameters ]")]
        [SerializeField, Min(0.001f)] private float m_Duration = 1f;
        [SerializeField] private AnimationCurve m_DissoveCurve = AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 m_TargetScale;
        [SerializeField] private AdvancedDissolveGeometricCutoutController m_DissolveGeometricCutout1;
        [SerializeField] private AdvancedDissolveGeometricCutoutController m_DissolveGeometricCutout2;

        private Coroutine _dissolveCoroutine;
        private bool _dissolving,_floor1Active;
        private void Awake()
        {
            _colliderFloor1 = m_ParentFloor1.GetComponentsInChildren<Collider>(true);
            _colliderFloor2 = m_ParentFloor2.GetComponentsInChildren<Collider>(true);
        }
        private void Start()
        {
            if (!m_DissolveGeometricCutout1.invert)
            {
                foreach (var c in _colliderFloor1) c.enabled = false;
                foreach (var c in _colliderFloor2) c.enabled = true;
            }
            else
            {
                foreach (var c in _colliderFloor1) c.enabled = true;
                foreach (var c in _colliderFloor2) c.enabled = false;
            }
        }

        public void Dissolve(Action onEnded = null)
        {
            if(_dissolving) return;
            if (_dissolveCoroutine != null) StopCoroutine(_dissolveCoroutine);
            _dissolveCoroutine = StartCoroutine(DissolveRoutine(onEnded));
        }

        private IEnumerator DissolveRoutine(Action onEnded = null)
        {
            if (m_DissolveGeometricCutout1.invert)
            {
                foreach (var c in _colliderFloor1) c.enabled = false;
                foreach (var c in _colliderFloor2) c.enabled = true;
            }
            else
            {
                foreach (var c in _colliderFloor1) c.enabled = true;
                foreach (var c in _colliderFloor2) c.enabled = false;
            }
            m_Source.Play();
            m_ParentFloor1.gameObject.SetActive(true);
            m_ParentFloor2.gameObject.SetActive(true);

            onDissolveChanged?.Invoke();

            _dissolving = true;
            float t = 0;
            Vector3 current = Vector3.zero;
            Vector3 target = m_TargetScale;

            while (t < 1)
            {
                t += Time.deltaTime / Mathf.Max(0.0001f, m_Duration);
                m_DissolveObject.localScale = Vector3.Lerp(current,target,m_DissoveCurve.Evaluate(t));
                yield return null;
            }

            m_DissolveGeometricCutout1.invert = !m_DissolveGeometricCutout1.invert;
            m_DissolveGeometricCutout2.invert = !m_DissolveGeometricCutout2.invert;
            m_DissolveObject.localScale = Vector3.zero;

            m_ParentFloor1.gameObject.SetActive(m_DissolveGeometricCutout1.invert);
            m_ParentFloor2.gameObject.SetActive(m_DissolveGeometricCutout2.invert);

            onEnded?.Invoke();

            _dissolving = false;
        }
    }
}
