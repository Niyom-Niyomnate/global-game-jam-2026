using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XingXing.GlobalGameJam.Y2026
{
    public enum Language { EN,TH }
    public class LocalizeManager : MonoBehaviour
    {
        public static LocalizeManager Instance;

        public static Action<Language> onLanguageChanged = delegate { };
        [field : SerializeField] public static Language language { get; set; }

        private SwitchToggle _switchToggle;
        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;

                _switchToggle = GetComponent<SwitchToggle>();

                DontDestroyOnLoad(gameObject);
            }
        }
        private void OnEnable()
        {
            if(_switchToggle)
            _switchToggle.onValueChanged += OnChangeLanguage;
        }
        private void OnDisable()
        {
            if(_switchToggle)
            _switchToggle.onValueChanged -= OnChangeLanguage;
        }
        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame) OnChangeLanguage(Language.EN);
            if (Keyboard.current.tKey.wasPressedThisFrame) OnChangeLanguage(Language.TH);
        }

        private void OnChangeLanguage(bool onChange)
        {
            if (!onChange) OnChangeLanguage(Language.TH);
            else OnChangeLanguage(Language.EN);
        }
        public static void OnChangeLanguage(Language _language)
        {
            language = _language;

            onLanguageChanged?.Invoke(language);
        }
    }
}
