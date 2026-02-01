using UnityEngine;

namespace XingXing.GlobalGameJam.Y2026
{
    public class TargetFrameRate : MonoBehaviour
    {
        public enum FarmeRate : int
        {
            FPS_30 = 30,
            FPS_60 = 60,
        }
        [SerializeField] private FarmeRate m_FrameRate;
        private void Awake()
        {
            Application.targetFrameRate = (int)m_FrameRate;
        }
    }
}
