using UnityEngine;

namespace Kk
{
    public class TimeForTwerkingLerp : MonoBehaviour
    {
        public enum TwerkingAnimation
        {
            InSine, InQuad, InCubic, InQuart, InQuint, InExpo, InCirc, InBack, InElastic,
            OutSine, OutQuad, OutCubic, OutQuart, OutQuint, OutExpo, OutCirc, OutBack, OutElastic
        }

        public static float TimeTwerkingLerp(TwerkingAnimation twerking, float t)
        {
            var time = 0f;
            switch (twerking)
            {
                case TwerkingAnimation.InSine: time = InSine(t); break;
                case TwerkingAnimation.InQuad: time = InQuad(t); break;
                case TwerkingAnimation.InCubic: time = InCubic(t); break;
                case TwerkingAnimation.InQuart: time = InQuart(t); break;
                case TwerkingAnimation.InQuint: time = InQuint(t); break;
                case TwerkingAnimation.InExpo: time = InExpo(t); break;
                case TwerkingAnimation.InCirc: time = InCirc(t); break;
                case TwerkingAnimation.InBack: time = InBack(t); break;
                case TwerkingAnimation.OutSine: time = OutSine(t); break;
                case TwerkingAnimation.OutQuad: time = OutQuad(t); break;
                case TwerkingAnimation.OutCubic: time = OutCubic(t); break;
                case TwerkingAnimation.OutQuart: time = OutQuart(t); break;
                case TwerkingAnimation.OutExpo: time = OutExpo(t); break;
                case TwerkingAnimation.OutCirc: time = OutCirc(t); break;
                case TwerkingAnimation.OutBack: time = OutBack(t); break;
                case TwerkingAnimation.OutElastic: time = OutElastic(t); break;
            }
            return time;
        }
        public static float Flip(float t) => 1 - t;
        #region Ease in
        public static float InSine(float t) => 1 - Mathf.Cos((Mathf.PI / 2) * t);
        public static float InQuad(float t) => Mathf.Pow(t, 2);
        public static float InCubic(float t) => Mathf.Pow(t, 3);
        public static float InQuart(float t) => Mathf.Pow(t, 4);
        public static float InQuint(float t) => Mathf.Pow(t, 5);
        public static float InExpo(float t) => Mathf.Pow(2, 10 * (t - 1));
        public static float InCirc(float t) => 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
        public static float InBack(float t) => Mathf.Pow(t, 2) * ((2.70158f * t) - 1.70158f);
        public static float InElastic(float t) => Mathf.Pow(2, 10 * (t - 1) * Mathf.Sin(10 * Mathf.PI * t));
        #endregion

        #region Ease Out
        public static float OutSine(float t) => Mathf.Sin((Mathf.PI / 2) * t);
        public static float OutQuad(float t) => Flip(InQuad(Flip(t)));
        public static float OutCubic(float t) => Flip(InCubic(Flip(t)));
        public static float OutQuart(float t) => Flip(InQuart(Flip(t)));
        public static float OutQuint(float t) => Flip(InQuint(Flip(t)));
        public static float OutExpo(float t) => Flip(InExpo(Flip(t)));
        public static float OutCirc(float t) => Flip(InCirc(Flip(t)));
        public static float OutBack(float t) => Flip(InBack(Flip(t)));
        public static float OutElastic(float t) => Flip(InElastic(Flip(t)));
        #endregion
    }
}
