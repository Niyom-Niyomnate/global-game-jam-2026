using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace XingXing.GlobalGameJam.Y2026
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        public static Action<string> onSceneLoaded = delegate { };

        [SerializeField] private Animator m_Animation;
        [SerializeField] private string m_AnimationNameFadeIn;
        [SerializeField] private string m_AnimationNameFadeOut;
        [Space(15f)]
        [SerializeField,Min(1f)] private float m_Duration;
        [SerializeField] private AnimationCurve m_AnimationCurve;

        public static bool loading { get; private set; }
        private void Awake()
        {
            if(Instance != null) Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public void LoadScene(string name)
        {
            LoadScene(name, null);
        }
        public static void LoadScene(string name, Action onDone = null)
        {
            if(loading)return;
            Instance.StartCoroutine(Instance.Loading(name,onDone));
        }
        private IEnumerator Loading(string name,Action onDone = null)
        {
            loading = true;
            onSceneLoaded?.Invoke(name);
            Time.timeScale = 0f;
            float t = 0;
            float n = 0;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / m_Duration;
                n = Mathf.Lerp(0, 1, t);
                m_Animation.Play(m_AnimationNameFadeOut, 0, m_AnimationCurve.Evaluate(n));
                yield return null;
            }

            m_Animation.Play(m_AnimationNameFadeOut, 0, 1);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
            asyncLoad.allowSceneActivation = false;
            yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
            yield return new WaitForSecondsRealtime(1f);
            asyncLoad.allowSceneActivation = true;
            onDone?.Invoke();
            yield return new WaitUntil(() => asyncLoad.isDone);
            Time.timeScale = 1;
            t = 0;
            n = 0;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime / m_Duration;
                n = Mathf.Lerp(0, 1, t);
                m_Animation.Play(m_AnimationNameFadeIn, 0, m_AnimationCurve.Evaluate(n));
                yield return null;
            }
            m_Animation.Play(m_AnimationNameFadeIn,0,1);
            Time.timeScale = 1f;
            loading = false;
        }
    }
}
