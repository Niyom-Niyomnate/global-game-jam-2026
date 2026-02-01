using UnityEngine;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class BackToMenuButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;

        private void Start()
        {
            m_Button.onClick.AddListener(() =>
            {
                SceneLoader.LoadScene("Menu");
            });
        }
        private void OnEnable()
        {
            SceneLoader.onSceneLoaded += SetActiveButton;
        }
        private void OnDisable()
        {
            SceneLoader.onSceneLoaded -= SetActiveButton;
        }

        private void SetActiveButton(string scene)
        {
            if(scene != "Menu")
            {
                m_Button.gameObject.SetActive(true);
            }
            else
            {
                m_Button.gameObject.SetActive(false);
            }
        }
    }
}
