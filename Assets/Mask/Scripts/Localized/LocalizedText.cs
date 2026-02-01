using TMPro;
using UnityEngine;

namespace XingXing.GlobalGameJam.Y2026
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField, TextArea(2, 10)] private string m_TextTH;
        [SerializeField,TextArea(2,10)] private string m_TextEN;
        private TextMeshProUGUI _text;
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        private void Start()
        {
            _text.text = LocalizeManager.language switch
            {
                Language.EN => m_TextEN,
                Language.TH => m_TextTH,
                _ => m_TextEN,
            };
        }
        private void OnEnable()
        {
            LocalizeManager.onLanguageChanged += OnLanguageChanged;
        }
        private void OnDisable()
        {
            LocalizeManager.onLanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(Language l)
        {
            if(!_text) _text = GetComponent<TextMeshProUGUI>();

            _text.text = LocalizeManager.language switch
            {
                Language.EN => m_TextEN,
                Language.TH => m_TextTH,
                _ => m_TextEN,
            };
        }
    }
}
