using System.Collections;
using UnityEngine;

namespace XingXing.GlobalGameJam.Y2026
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private string m_ID;
        [SerializeField] private string m_Scene;
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            CutsceneManager.Instance.Playcutscene(m_ID,() => SceneLoader.LoadScene(m_Scene));
        }
    }
}
