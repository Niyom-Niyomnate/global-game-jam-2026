using HorrorGameJam;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace XingXing.GlobalGameJam.Y2026
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string m_SceneNext;

        [SerializeField] private bool[] m_Missions;

        [SerializeField] private UnityEvent m_OnCompleted;
        [SerializeField] private UnityEvent m_OnFailed;
        private bool _failed,_completed;

        public void SetBoolIndex(int index)
        {
            if(index < 0 || index >= m_Missions.Length) return;

            m_Missions[index] = true;
        }
        public void MissionComplete()
        {
            if(_completed) return;
            foreach (var item in m_Missions) if (!item) return;

            SceneLoader.LoadScene(m_SceneNext);
            m_OnCompleted?.Invoke();
            _completed = true;
        }
        public void MissionFail()
        {
            if(_failed) return;

            Scene scene = SceneManager.GetActiveScene();
            SceneLoader.LoadScene(scene.name);
            m_OnFailed?.Invoke();
            _failed = true;
        }
    }
}
