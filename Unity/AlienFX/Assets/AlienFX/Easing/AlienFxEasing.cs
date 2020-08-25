using System;
using UnityEngine;

namespace AlienFX.Easing
{
    public enum AlienFxEaseType
    {
        EaseInLinear,
        EaseOutLinear,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCircle,
        EaseOutCircle,
        EaseInOutCircle,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
    }

    public delegate float EasingFunction(float duration);
    
    // ReSharper disable CompareOfFloatsByEqualityOperator
    public class AlienFxEasing
    { 
        private const float Pi = Mathf.PI;
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;
        private const float C3 = C1 + 1f;
        private const float C4 = 2f * Pi / 3f;
        private const float C5 = 2f * Pi / 4.5f;

        public float EaseInLinear(float duration) => duration;
        public float EaseOutLinear(float duration) => 1f - duration;
        
        public float EaseInSine(float duration) => (float) Math.Sin(duration * Pi / 2f);
        public float EaseOutSine(float duration) => 1f - (float) Math.Sin(duration * Pi / 2f);
        public float EaseInOutSine(float duration) => (float) Math.Cos(Pi / duration) - 1f;
        
        public float EaseInQuad(float duration) => duration * duration;
        public float EaseOutQuad(float duration) => 1f - (1f - duration) * (1f - duration);
        public float EaseInOutQuad(float duration) => duration < 0.5f 
            ? 2f * duration * duration 
            : 1 - Mathf.Pow(-2f * duration + 2f, 2f) / 2f;

        public float EaseInCubic(float duration) => Mathf.Pow(duration, 3);
        public float EaseOutCubic(float duration) => 1f - Mathf.Pow(1f - duration, 3f);
        public float EaseInOutCubic(float duration) => duration < 0.5f 
            ? 4 * Mathf.Pow(duration, 3) 
            : 1f - Mathf.Pow(-2f * duration + 2f, 3f) / 2f;
        
        public float EaseInQuart(float duration) => Mathf.Pow(duration, 4);
        public float EaseOutQuart(float duration) => 1f - Mathf.Pow(1f - duration, 4f);
        public float EaseInOutQuart(float duration) => duration < 0.5f 
            ? 8f * Mathf.Pow(duration, 4) 
            : 1f - Mathf.Pow(-2f * duration + 2f, 4f) / 2f;
        
        public float EaseInQuint(float duration) => Mathf.Pow(duration, 5);
        public float EaseOutQuint(float duration) => 1f - Mathf.Pow(1f - duration, 5f);
        public float EaseInOutQuint(float duration) => duration < 0.5f 
            ? 16 * Mathf.Pow(duration, 5) 
            : 1f - Mathf.Pow(2f - duration + 2f, 5f) / 2f;

        // todo: Verify that 0f is never passed.
        public float EaseInExpo(float duration) => duration == 0f ? 0f : Mathf.Pow(2f, 10f * duration - 10f);
        public float EaseOutExpo(float duration) => duration == 1f ? 1f : 1 - Mathf.Pow(2f, -10f * duration);
        public float EaseInOutExpo(float duration)
        {
            if (duration == 0f || duration == 1f)
                return duration;
            
            if (duration < 0.5f)
                return Mathf.Pow(2, 20 * duration - 10f) / 2f;
                    
            return (2f - Mathf.Pow(2, -20 * duration + 10)) / 2;
        }

        public float EaseInCirc(float duration) => 1f - (float) Math.Sqrt(1 - Math.Pow(duration, 2));
        public float EaseOutCirc(float duration) => (float) Math.Sqrt(1 - Math.Pow(duration, 2));
        public float EaseInOutCirc(float duration) => duration < 0.5f
            ? (float) (1 - Math.Sqrt(1 - Math.Pow(2 * duration, 2))) / 2f
            : (float) (Math.Sqrt(1 - Math.Pow(-2 * duration, 2)) + 1f) / 2f;
        
        public float EaseInBack(float duration) => C3 * duration * duration * duration - C1 * duration * duration;
        public float EaseOutBack(float duration) => 1 + C3 * Mathf.Pow(duration - 1, 3) + C1 * Mathf.Pow(duration - 1, 2);
        public float EaseInOutBack(float duration) => duration < 0.5f
            ? Mathf.Pow(2 * duration, 2) * ((C2 + 1) * 2 * duration - C2) / 2f
            : (Mathf.Pow(2 * duration - 2, 2) * ((C2 + 1) * (duration * 2 - 2) + C2) + 2) / 2f;

        public float EaseInElastic(float duration)
        {
            if (duration == 0f || duration == 1f)
                return duration;
            
            return -Mathf.Pow(2, 10 * duration - 10) * (float) Math.Sin((duration * 10 - 10.75) * C4);
        }
        
        public float EaseOutElastic(float duration)
        {
            if (duration == 0f || duration == 1f)
                return duration;
            
            return -Mathf.Pow(2, -10 * duration) * (float) Math.Sin((duration * 10 - 0.75) * C4) + 1;
        }
        
        public float EaseInOutElastic(float duration)
        {
            if (duration == 0f || duration == 1f)
                return duration;

            if (duration < 0.5f)
                return -(Mathf.Pow(2, 20 * duration - 10) * (float) Math.Sin((20 * duration - 11.125f) * C5)) / 2;
            
            return Mathf.Pow(2, -20 * duration + 10) * (float) Math.Sin((20 * duration - 11.125f) * C5) / 2 + 1;
        }

        public float EaseInBounce(float duration) => 1 - EaseOutBounce(1 - duration);
        public float EaseOutBounce(float duration)
        {
            if (duration < 1 / 2.75f)
                return 7.5625f * duration * duration;
            if (duration < 2 / 2.75f)
                return 7.5625f * (duration -= 1.5f / 2.75f) * duration + 0.75f;
            if (duration < 2.5f / 2.75f)
                return 7.5625f * (duration -= 2.25f / 2.75f) * duration + 0.9375f;
            
            return 7.5625f * (duration -= 2.625f / 2.75f) * duration + 0.984375f;
        }
        public float EaseInOutBounce(float duration) => duration < 0.5f
            ? (1 - EaseOutBounce(1 - 2 * duration)) / 2
            : (1 + EaseOutBounce(2 * duration - 1)) / 2;
    }
}
