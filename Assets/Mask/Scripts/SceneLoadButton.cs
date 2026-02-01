using UnityEngine;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private string m_Scene;

        private void Start()
        {
            m_Button.onClick.AddListener(() => SceneLoader.LoadScene(m_Scene));
        }
    }
}
