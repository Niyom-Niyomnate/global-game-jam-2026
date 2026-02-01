using UnityEngine;

namespace XingXing.GlobalGameJam.Y2026
{
    [CreateAssetMenu(fileName = "ScriptableObjects",menuName = "ScriptableObjects/Tutarial")]
    public class TutorialScriptableObject : ScriptableObject
    {
        public Sprite sprite;
        [TextArea(3, 10)] public string tutorialTH;
        [TextArea(3, 10)] public string tutorialEN;
    }
}
