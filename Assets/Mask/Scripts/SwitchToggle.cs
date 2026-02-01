using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class SwitchToggle : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private RectTransform m_Rect;
        [SerializeField] private float m_Duration = 1f;
        [SerializeField] private AnimationCurve m_Curve;
        [Space(15f)]
        [SerializeField] private CanvasGroup canvasGroupIsOn;
        [SerializeField] private CanvasGroup canvasGroupIsOff;
        [Space(15f)]
        [SerializeField] private UnityEvent m_On;
        [SerializeField] private UnityEvent m_Off;

        public Action<bool> onValueChanged = delegate { };

        private Coroutine _switchCoroutine;
        private Vector2 _left, _right;
        private bool _isOn,_procressing;
        private void Start()
        {
            _left = new Vector2(Mathf.Abs(m_Rect.anchoredPosition.x), m_Rect.anchoredPosition.y);
            _right = new Vector2(-Mathf.Abs(m_Rect.anchoredPosition.x), m_Rect.anchoredPosition.y);
            _isOn = false;
            m_Button.onClick.AddListener(OnClick);
            OnClick();
        }

        private void OnClick()
        {
            if(_procressing) return;
            _isOn = !_isOn;
            if (_switchCoroutine != null) StopCoroutine(_switchCoroutine);
            _switchCoroutine = StartCoroutine(SwitchRoutine(_isOn));
        }
        private IEnumerator SwitchRoutine(bool isOn)
        {
            _procressing = true;
            float t = 0;
            Vector2 current = m_Rect.anchoredPosition;
            Vector2 target = isOn ? _right : _left;

            canvasGroupIsOn.alpha = isOn ? 1 : 0;
            canvasGroupIsOff.alpha = isOn ? 0 : 1;

            while (t < 1)
            {
                t += Time.deltaTime / Mathf.Max(0.0001f, m_Duration);
                m_Rect.anchoredPosition = Vector2.Lerp(current,target,m_Curve.Evaluate(t));
                yield return null;
            }

            m_Rect.anchoredPosition = target;
            onValueChanged?.Invoke(isOn);
            if (isOn) m_On?.Invoke();
            else m_Off?.Invoke();
            _procressing = false;
        }
    }
}
