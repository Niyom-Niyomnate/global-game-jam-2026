using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace XingXing.GlobalGameJam.Y2026
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer m_AudioMixer;
        [SerializeField] private Slider m_Slider;

        private void Start()
        {
            SetVolume(1f);
            m_Slider.onValueChanged.AddListener(value =>
            {
                SetVolume(value);
            });
        }
        public void SetVolume(float value)
        {
            float v = Mathf.Clamp(value, 0.0001f, 1f);
            m_AudioMixer.SetFloat("MasterSound", Mathf.Log10(v) * 20);
        }
    }
}
