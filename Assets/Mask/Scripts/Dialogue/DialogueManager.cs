using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class DialogueManager : MonoBehaviour
    {
        [Serializable] public struct Dialogue
        {
            [TextArea(3, 10)] public string sentenceTH;
            [TextArea(3, 10)] public string sentenceEN;
        }

        public static bool _endDailogue;

        [SerializeField] private SlideAnimation m_SlideAnimation;
        [SerializeField] private TextMeshProUGUI m_TextDialogue;
        [SerializeField] private Button m_ButtonNext;
        [SerializeField] private float m_SpeedLetter = 0.025f;
        [Space(12f)]
        [SerializeField] private Dialogue[] m_Dialogues;
        [Space(12f)]
        [SerializeField] private UnityEvent m_OnDialogued;

        private int _index;
        private Coroutine _dialogueCoroutine;
        private bool _started;
        private void OnEnable()
        {
            LocalizeManager.onLanguageChanged += OnLanguageChange;
        }
        private void OnDisable()
        {
            LocalizeManager.onLanguageChanged -= OnLanguageChange;
        }
        private IEnumerator Start()
        {
            m_TextDialogue.text = string.Empty;
            m_ButtonNext.onClick.AddListener(() => StartDialogue());
            yield return new WaitForSeconds(1f);

            _started = true;

            if (!_endDailogue)
            {
                m_SlideAnimation.SlideIn();
                _dialogueCoroutine = StartCoroutine(DialogueRoutine(GetSentence(_index)));
            }
            else
            {
                m_OnDialogued?.Invoke();
            }
        }
        private void StartDialogue()
        {
            if (m_TextDialogue.text != GetSentence(_index))
            {
                if (_dialogueCoroutine != null) StopCoroutine(_dialogueCoroutine);
                m_TextDialogue.text = GetSentence(_index);
            }
            else if(m_TextDialogue.text == GetSentence(_index))
            {
                ++_index;
                if (_index >= m_Dialogues.Length)
                {
                    _endDailogue = true;
                    m_SlideAnimation.SlideOut(() => m_OnDialogued?.Invoke());
                    return;
                }

                if (_dialogueCoroutine != null) StopCoroutine(_dialogueCoroutine);
                _dialogueCoroutine = StartCoroutine(DialogueRoutine(GetSentence(_index)));
            }
        }
        private string GetSentence(int index)
        {
            if(_index < 0 || _index >= m_Dialogues.Length) return "####";

            return LocalizeManager.language switch
            {
                Language.TH => m_Dialogues[_index].sentenceTH,
                Language.EN => m_Dialogues[_index].sentenceEN,
                _ => m_Dialogues[_index].sentenceEN
            };
        }
        private void OnLanguageChange(Language l)
        {
            if (_started && !_endDailogue)
            {
                if (_dialogueCoroutine != null) StopCoroutine(_dialogueCoroutine);
                _dialogueCoroutine = StartCoroutine(DialogueRoutine(GetSentence(_index)));
            }
        }

        private IEnumerator DialogueRoutine(string dialogue)
        {
            m_TextDialogue.text = string.Empty;
            foreach (var c in dialogue)
            {
                m_TextDialogue.text += c;
                yield return new WaitForSeconds(m_SpeedLetter);
            }

            m_TextDialogue.text = dialogue;
        }
    }
}
