using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace XingXing.GlobalGameJam.Y2026
{
    public class Setting : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private RectTransform m_Rect;
        [SerializeField] private float m_Duration = 1f;
        [SerializeField] private AnimationCurve m_Curve;
        [Space(15f)]
        [SerializeField] private float m_XPosTarget;

        private Coroutine _settingCoroutine;
        private Vector2 _originalPos,_targetPos;
        private bool _isOn, _procressing;
        private void Start()
        {
            _originalPos = m_Rect.anchoredPosition;
            _targetPos = new Vector2(m_XPosTarget, m_Rect.anchoredPosition.y);

            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (_procressing) return;
            _isOn = !_isOn;
            if (_settingCoroutine != null) StopCoroutine(_settingCoroutine);
            _settingCoroutine = StartCoroutine(SwitchRoutine(_isOn));
        }

        private IEnumerator SwitchRoutine(bool isOn)
        {
            _procressing = true;
            float t = 0;
            Vector2 current = m_Rect.anchoredPosition;
            Vector2 target = isOn ? _targetPos : _originalPos;

            while (t < 1)
            {
                t += Time.deltaTime / Mathf.Max(0.0001f, m_Duration);
                m_Rect.anchoredPosition = Vector2.Lerp(current, target, m_Curve.Evaluate(t));
                yield return null;
            }

            m_Rect.anchoredPosition = target;
            _procressing = false;
        }
    }
}
