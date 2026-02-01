using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class CutsceneManager : MonoBehaviour
    {
        [Serializable] public struct Cutscene
        {
            public string id;
            public string url;
        }

        public static CutsceneManager Instance;
        private Action onEnded = delegate { };

        [Header("Parameters")]
        [SerializeField] private Button m_ButtonSkip;
        [SerializeField] private SlideAnimation m_slideAnimation;
        [SerializeField] private VideoPlayer m_VideoPlayer;
        [SerializeField] private List<Cutscene> _cutscenes = new();

        public static bool playing { get; private set; }
        public static readonly Dictionary<string, string> cutscenesDic = new();
        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;

                _cutscenes.ForEach(c => cutscenesDic.Add(c.id, c.url));
                DontDestroyOnLoad(gameObject);
            }
        }
        private void OnEnable()
        {
            SceneLoader.onSceneLoaded += Skip;
        }
        private void OnDisable()
        {
            SceneLoader.onSceneLoaded -= Skip;
        }
        private void Start()
        {
            m_VideoPlayer.playOnAwake = false;
            m_VideoPlayer.isLooping = false;
            m_VideoPlayer.source = VideoSource.Url;
            m_VideoPlayer.loopPointReached += OnEnded;

            m_ButtonSkip.interactable = false;
            m_ButtonSkip.onClick.AddListener(Skip);
            m_slideAnimation.gameObject.SetActive(false);
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.vKey.wasPressedThisFrame)
            {
                Playcutscene("First");
            }
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                Skip();
            }
#endif
        }

        public void Skip()
        {
            if(!playing) return;

            m_ButtonSkip.interactable = false;
            m_slideAnimation.SlideOut(() =>
            {
                playing = false;
                Time.timeScale = 1;
                onEnded?.Invoke();
            });
        }
        public void Skip(string scene)
        {
            if (playing && scene == "Menu")
            {
                m_ButtonSkip.interactable = false;
                m_slideAnimation.SlideOut(() =>
                {
                    playing = false;
                    Time.timeScale = 1;
                    onEnded?.Invoke();
                });
            }

        }
        private void OnEnded(VideoPlayer video)
        {
            m_slideAnimation.SlideOut();
            playing = false;
            Time.timeScale = 1;
            onEnded?.Invoke();
        }

        public void Playcutscene(string id,Action onEnded = null)
        {
            try
            {
                if (cutscenesDic.ContainsKey(id))
                {
                    this.onEnded = onEnded;
                    playing = true;
                    Time.timeScale = 0;
                    m_ButtonSkip.interactable = true;
                    m_VideoPlayer.url = cutscenesDic[id];
                    m_VideoPlayer.targetTexture.Release();
                    m_slideAnimation.SlideIn(() => m_VideoPlayer.Play());
                }
            }
            catch
            {
                SceneLoader.LoadScene("Menu");
            }
        }
    }
}
