using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private Button m_ButtonPrevious;
        [SerializeField] private Button m_ButtonNext;
        [SerializeField] private Image m_ImageTutorial;
        [SerializeField] private TextMeshProUGUI m_TextTutorial;
        [SerializeField] private TextMeshProUGUI m_TextCount;
        [Space(12f)]
        [SerializeField] private TutorialScriptableObject[] m_Tutorials;

        private int _index;

        private void OnEnable()
        {
            LocalizeManager.onLanguageChanged += OnLanguageChanged;
        }
        private void OnDisable()
        {
            LocalizeManager.onLanguageChanged -= OnLanguageChanged;
        }
        private void Start()
        {
            m_ButtonNext.onClick.AddListener(Next);
            m_ButtonPrevious.onClick.AddListener(Previous);
        }

        public void SetUITutarial(int index)
        {
            _index = index;
            SetUITutorial(m_Tutorials[_index]);
        }

        private void Next()
        {
            ++_index;
            _index %= m_Tutorials.Length;
            SetUITutorial(m_Tutorials[_index]);
        }
        private void Previous()
        {
            --_index;
            _index = _index < 0 ? m_Tutorials.Length - 1 : _index;
            SetUITutorial(m_Tutorials[_index]);
        }
        private void SetUITutorial(TutorialScriptableObject tutorial)
        {
            m_TextCount.text = (_index + 1) + " / " + m_Tutorials.Length;
            m_ImageTutorial.sprite = tutorial.sprite;
            m_TextTutorial.text = LocalizeManager.language switch
            {
                Language.EN => tutorial.tutorialEN,
                Language.TH => tutorial.tutorialTH,
                _ => tutorial.tutorialEN
            };
        }
        private void OnLanguageChanged(Language l)
        {
            m_TextTutorial.text = LocalizeManager.language switch
            {
                Language.EN => m_Tutorials[_index].tutorialEN,
                Language.TH => m_Tutorials[_index].tutorialTH,
                _ => m_Tutorials[_index].tutorialEN
            };
        }
    }
}
