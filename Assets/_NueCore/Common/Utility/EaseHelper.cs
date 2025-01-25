using System;
using System.Collections.Generic;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public static class EaseHelper
    {
      private const float _PiOver2 = 1.5707964f;
      private const float _TwoPi = 6.2831855f;
      public static float defaultEaseOvershootOrAmplitude = 1.70158f;
      public static float defaultEasePeriod = 0.0f;

        private static Dictionary<NEaseTypes, Func<float, float, float>> EaseTypeDict =
            new Dictionary<NEaseTypes, Func<float, float, float>>()
            {
                {NEaseTypes.Linear, EaseLiner},
                {NEaseTypes.InSine, EaseInSine},
                {NEaseTypes.OutSine, EaseOutSine},
                {NEaseTypes.InOutSine, EaseInOutSine},
                {NEaseTypes.InQuad, EaseInQuad},
                {NEaseTypes.OutQuad, EaseOutQuad},
                {NEaseTypes.InOutQuad, EaseInOutQuad},
                {NEaseTypes.InCubic, EaseInCubic},
                {NEaseTypes.OutCubic, EaseOutCubic},
                {NEaseTypes.InOutCubic, EaseInOutCubic},
                {NEaseTypes.InQuart, EaseInQuart},
                {NEaseTypes.OutQuart, EaseOutQuart},
                {NEaseTypes.InOutQuart, EaseInOutQuart},
                {NEaseTypes.InQuint, EaseInQuint},
                {NEaseTypes.OutQuint, EaseOutQuint},
                {NEaseTypes.InOutQuint, EaseInOutQuint},
                {NEaseTypes.InExpo, EaseInExpo},
                {NEaseTypes.OutExpo, EaseOutExpo},
                {NEaseTypes.InOutExpo, EaseInOutExpo},
                {NEaseTypes.InCirc, EaseInCirc},
                {NEaseTypes.OutCirc, EaseOutCirc},
                {NEaseTypes.InOutCirc, EaseInOutCirc},
                {NEaseTypes.InElastic, EaseInElastic},
                {NEaseTypes.OutElastic, EaseOutElastic},
                {NEaseTypes.InOutElastic, EaseInOutElastic},
                {NEaseTypes.InBack, EaseInBack},
                {NEaseTypes.OutBack, EaseOutBack},
                {NEaseTypes.InOutBack, EaseInOutBack},
                {NEaseTypes.InBounce, EaseInBounce},
                {NEaseTypes.OutBounce, EaseOutBounce},
                {NEaseTypes.InOutBounce, EaseInOutBounce},
                {NEaseTypes.Flash, EaseFlash},
                {NEaseTypes.InFlash, EaseInFlash},
                {NEaseTypes.OutFlash, EaseOutFlash},
                {NEaseTypes.InOutFlash, EaseInOutFlash},
                {NEaseTypes.Default, DefaultEase}
            };

        private const double Val1 = 1.5707963705062866;
        private const double Pi = 3.1415927410125732;
        private const double DoublePi = 6.2831854820251465;
        private const double Zero = 0.0;
        private const double Half = 0.5;
        private const double One = 1.0;
        private const double Two = 2.0;
        private const double Ten = 10.0;
        private const float ZeroF = 0.0f;
        private const float HalfF = 0.5f;
        private const float OneF = 1.0f;
        private const float TwoF = 2.0f;
        public static float Evaluate(float time, float duration, NEaseTypes easeType)
        {
            return EaseTypeDict[easeType](time, duration);
        }
        
        public static float EaseLiner(float time, float duration)
        {
          return time / duration;
        }

        public static float EaseInSine(float time, float duration)
        {
          return (float) (-Math.Cos((double) time / (double) duration * Val1) + One);
        }

        public static float EaseOutSine(float time, float duration)
        {
          return (float) Math.Sin((double) time / (double) duration * Val1);
        }

        public static float EaseInOutSine(float time, float duration)
        {
          return (float) (-Half * (Math.Cos(Pi * (double) time / (double) duration) - One));
        }
        
        public static float EaseInQuad(float time, float duration)
        {
          return (time /= duration) * time;
        }

        public static float EaseOutQuad(float time, float duration)
        {
          return (float) (-(double) (time /= duration) * ((double) time - Two));
        }

        public static float EaseInOutQuad(float time, float duration)
        {
          return (double) (time /= duration * HalfF) < One ? HalfF * time * time : (float) (-Half * ((double) --time * ((double) time - Two) - One));
        }

        public static float EaseInCubic(float time, float duration)
        {
          return (time /= duration) * time * time;
        }
        public static float EaseOutCubic(float time, float duration)
        {
          return (float) ((double) (time = (float) ((double) time / (double) duration - One)) * (double) time * (double) time + One);
        }

        public static float EaseInOutCubic(float time, float duration)
        {
          return (double) (time /= duration * HalfF) < One ? HalfF * time * time * time : (float) (Half * ((double) (time -= TwoF) * (double) time * (double) time + Two));
        }

        public static float EaseInQuart(float time, float duration)
        {
          return (time /= duration) * time * time * time;
        }

        public static float EaseOutQuart(float time, float duration)
        {
          return (float) -((double) (time = (float) ((double) time / (double) duration - One)) * (double) time * (double) time * (double) time - One);
        }

        public static float EaseInOutQuart(float time, float duration)
        {
          return (double) (time /= duration * HalfF) < One ? HalfF * time * time * time * time : (float) (-Half * ((double) (time -= TwoF) * (double) time * (double) time * (double) time - Two));
        }

        public static float EaseInQuint(float time, float duration)
        {
          return (time /= duration) * time * time * time * time;
        }

        public static float EaseOutQuint(float time, float duration)
        {
          return (float) ((double) (time = (float) ((double) time / (double) duration - One)) * (double) time * (double) time * (double) time * (double) time + One);
        }

        public static float EaseInOutQuint(float time, float duration)
        {
          return (double) (time /= duration * HalfF) < One ? HalfF * time * time * time * time * time : (float) (Half * ((double) (time -= TwoF) * (double) time * (double) time * (double) time * (double) time + Two));
        }

        public static float EaseInExpo(float time, float duration)
        {
          return (double) time != Zero ? (float) Math.Pow(Two, Ten * ((double) time / (double) duration - One)) : ZeroF;
        }

        public static float EaseOutExpo(float time, float duration)
        {
          return (double) time == (double) duration ? OneF : (float) (-Math.Pow(Two, -Ten * (double) time / (double) duration) + One);
        }

        public static float EaseInOutExpo(float time, float duration)
        {
          if ((double) time == Zero)
            return ZeroF;
          if ((double) time == (double) duration)
            return OneF;
          return (double) (time /= duration * HalfF) < One ? HalfF * (float) Math.Pow(Two, Ten * ((double) time - One)) : (float) (Half * (-Math.Pow(Two, -Ten * (double) --time) + Two));
        }

        public static float EaseInCirc(float time, float duration)
        {
          return (float) -(Math.Sqrt(One - (double) (time /= duration) * (double) time) - One);
        }

        public static float EaseOutCirc(float time, float duration)
        {
          return (float) Math.Sqrt(One - (double) (time = (float) ((double) time / (double) duration - One)) * (double) time);
        }

        public static float EaseInOutCirc(float time, float duration)
        {
          return (double) (time /= duration * HalfF) < One ? (float) (-Half * (Math.Sqrt(One - (double) time * (double) time) - One)) : (float) (Half * (Math.Sqrt(One - (double) (time -= TwoF) * (double) time) + One));
        }

        public static float EaseInElastic(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          if ((double) time == Zero)
            return ZeroF;
          if ((double) (time /= duration) == One)
            return OneF;
          if ((double) period == Zero)
            period = duration * 0.3f;
          float num1;
          if ((double) overshootOrAmplitude < One)
          {
            overshootOrAmplitude = OneF;
            num1 = period / 4f;
          }
          else
            num1 = period / 6.2831855f * (float) Math.Asin(1.0 / (double) overshootOrAmplitude);
          return (float) -((double) overshootOrAmplitude * Math.Pow(Two, Ten * (double) --time) * Math.Sin(((double) time * (double) duration - (double) num1) * DoublePi / (double) period));
        }

        public static float EaseOutElastic(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          if ((double) time == Zero)
            return ZeroF;
          if ((double) (time /= duration) == One)
            return OneF;
          if ((double) period == Zero)
            period = duration * 0.3f;
          float num2;
          if ((double) overshootOrAmplitude < One)
          {
            overshootOrAmplitude = OneF;
            num2 = period / 4f;
          }
          else
            num2 = period / 6.2831855f * (float) Math.Asin(One / (double) overshootOrAmplitude);
          return (float) ((double) overshootOrAmplitude * Math.Pow(Two, -Ten * (double) time) * Math.Sin(((double) time * (double) duration - (double) num2) * DoublePi / (double) period) + One);

        }

        public static float EaseInOutElastic(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          if ((double) time == Zero)
            return ZeroF;
          if ((double) (time /= duration * HalfF) == Two)
            return OneF;
          if ((double) period == Zero)
            period = duration * 0.45000002f;
          float num3;
          if ((double) overshootOrAmplitude < One)
          {
            overshootOrAmplitude = OneF;
            num3 = period / 4f;
          }
          else
            num3 = period / 6.2831855f * (float) Math.Asin(One / (double) overshootOrAmplitude);
          return (double) time < One ? (float) (-Half * ((double) overshootOrAmplitude * Math.Pow(Two, Ten * (double) --time) * Math.Sin(((double) time * (double) duration - (double) num3) * DoublePi / (double) period))) : (float) ((double) overshootOrAmplitude * Math.Pow(Two, -Ten * (double) --time) * Math.Sin(((double) time * (double) duration - (double) num3) * DoublePi / (double) period) * Half + One);
        }

        public static float EaseInBack(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          return (float) ((double) (time /= duration) * (double) time * (((double) overshootOrAmplitude + One) * (double) time - (double) overshootOrAmplitude));
        }

        public static float EaseOutBack(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          return (float) ((double) (time = (float) ((double) time / (double) duration - One)) * (double) time * (((double) overshootOrAmplitude + One) * (double) time + (double) overshootOrAmplitude) + One);
        }

        public static float EaseInOutBack(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          return (double) (time /= duration * HalfF) < One ? (float) (Half * ((double) time * (double) time * (((double) (overshootOrAmplitude *= 1.525f) + One) * (double) time - (double) overshootOrAmplitude))) : (float) (Half * ((double) (time -= TwoF) * (double) time * (((double) (overshootOrAmplitude *= 1.525f) + One) * (double) time + (double) overshootOrAmplitude) + Two));
        }

        public static float EaseInBounce(float time, float duration)
        {
          return OneF - EaseOutBounce(time, duration);
        }

        public static float EaseOutBounce(float time, float duration)
        {
          if ((double) (time /= duration) < 0.3636363744735718)
            return 121f / 16f * time * time;
          if ((double) time < 0.7272727489471436)
            return (float) (121.0 / 16.0 * (double) (time -= 0.54545456f) * (double) time + 0.75);
          return (double) time < 0.9090909361839294 ? (float) (121.0 / 16.0 * (double) (time -= 0.8181818f) * (double) time + 15.0 / 16.0) : (float) (121.0 / 16.0 * (double) (time -= 0.95454544f) * (double) time + 63.0 / 64.0);
        }

        public static float EaseInOutBounce(float time, float duration)
        {
          return (double) time < (double) duration * Half ? EaseInBounce(time * TwoF, duration) * HalfF : (float) ((double) EaseOutBounce(time * TwoF - duration, duration) * Half + Half);
        }

        public static float EaseFlash(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
          float stepDuration = duration / overshootOrAmplitude;
          time -= stepDuration * (float) (stepIndex - 1);
          float dir = stepIndex % 2 != 0 ? 1f : -1f;
          if ((double) dir < 0.0)
            time -= stepDuration;
          float res = time * dir / stepDuration;
          return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseInFlash(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
          float stepDuration = duration / overshootOrAmplitude;
          time -= stepDuration * (float) (stepIndex - 1);
          float dir = stepIndex % 2 != 0 ? 1f : -1f;
          if ((double) dir < 0.0)
            time -= stepDuration;
          time *= dir;
          float res = (time /= stepDuration) * time;
          return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseOutFlash(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
          float stepDuration = duration / overshootOrAmplitude;
          time -= stepDuration * (float) (stepIndex - 1);
          float dir = stepIndex % 2 != 0 ? 1f : -1f;
          if ((double) dir < 0.0)
            time -= stepDuration;
          time *= dir;
          float res = (float) (-(double) (time /= stepDuration) * ((double) time - 2.0));
          return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseInOutFlash(float time, float duration)
        {
          var overshootOrAmplitude = defaultEaseOvershootOrAmplitude;
          var period = defaultEasePeriod;
          int stepIndex = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
          float stepDuration = duration / overshootOrAmplitude;
          time -= stepDuration * (float) (stepIndex - 1);
          float dir = stepIndex % 2 != 0 ? 1f : -1f;
          if ((double) dir < 0.0)
            time -= stepDuration;
          time *= dir;
          float res = (double) (time /= stepDuration * 0.5f) < 1.0 ? 0.5f * time * time : (float) (-0.5 * ((double) --time * ((double) time - 2.0) - 1.0));
          return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float DefaultEase(float time, float duration)
        {
          return (float) (-(double) (time /= duration) * ((double) time - 2.0));
        }
        
        
        
        
        
        
        
        public static float WeightedEase(
          float overshootOrAmplitude,
          float period,
          int stepIndex,
          float stepDuration,
          float dir,
          float res)
        {
          float num1 = 0.0f;
          float num2 = 0.0f;
          if ((double) dir > 0.0 && (int) overshootOrAmplitude % 2 == 0)
            ++stepIndex;
          else if ((double) dir < 0.0 && (int) overshootOrAmplitude % 2 != 0)
            ++stepIndex;
          if ((double) period > 0.0)
          {
            float num3 = (float) Math.Truncate((double) overshootOrAmplitude);
            float num4 = overshootOrAmplitude - num3;
            if ((double) num3 % 2.0 > 0.0)
              num4 = 1f - num4;
            num2 = num4 * (float) stepIndex / overshootOrAmplitude;
            num1 = res * (overshootOrAmplitude - (float) stepIndex) / overshootOrAmplitude;
          }
          else if ((double) period < 0.0)
          {
            period = -period;
            num1 = res * (float) stepIndex / overshootOrAmplitude;
          }
          float num5 = num1 - res;
          res += num5 * period + num2;
          if ((double) res > 1.0)
            res = 1f;
          return res;
        }


    }
}