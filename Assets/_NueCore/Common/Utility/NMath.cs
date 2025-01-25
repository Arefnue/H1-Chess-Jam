using System;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public class NMath
    {
        private const float CurveMult = 4f;

        public static float OutSine(float baseValue, float currentLevel)
        {
            return baseValue + Mathf.Sin((currentLevel * Mathf.PI) / 2f);
        }
        
        public static float SquareRoot(float baseValue, float currentLevel)
        {
            return baseValue + currentLevel + Mathf.Sqrt(Mathf.Pow(currentLevel, 5));
        }

        public static double Map(double x, double inMin, double inMax, double outMin, double outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
        
        public static double ClampedMap(double x, double inMin, double inMax, double outMin, double outMax)
        {
            if (inMin < inMax)
            {
                if (x < inMin)
                    return outMin;
                if (x > inMax)
                    return outMax;
            }
            else
            {
                if (x > inMin)
                    return outMin;
                if (x < inMax)
                    return outMax;
            }
          
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        public static float Step(float x, float edge)
        {
            return x >= edge ? 1 : 0;
        }

        public static float SmoothStep(float x, float highEdge, float lowEdge)
        {
            var diff = highEdge - lowEdge;
            return x >= lowEdge ? Mathf.Lerp(0f, diff, Mathf.Abs(x - lowEdge) /  diff) / diff : 0;
        }

        public static bool InSquare(float x, float minX, float minY, float width, float height)
        {
            var horizontalValue = Step(x, minX) - Step(x, minX + width);
            var verticalValue = Step(x, minY) - Step(x, minY + height);
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return horizontalValue * verticalValue == 1;
        }

        public static float SmoothCurve(float start, float end, float height, float t, float multiplier = 4)
        {
            float Func(float x) => multiplier * (-height * x * x + height * x);

            return Func(t) + Mathf.Lerp(start, end, t);
        }

        public static float SmoothCurveEase(float start, float end, float height, float t,
            NEaseTypes firstEase = NEaseTypes.InOutSine,  NEaseTypes secondEase = NEaseTypes.InOutSine)
        {
            float Func(float x) => x  < .5f ? EaseHelper.Evaluate(x * 2, 1f, firstEase) * height : 
                height - EaseHelper.Evaluate(((x - .5f) * 2f), 1f, secondEase) * height ;

            return Func(t) + Mathf.Lerp(start, end, t);
        }


        public static Vector2 SmoothCurveVector2(Vector2 start, Vector2 end, float height, float t)
        {
            float Func(float x) => CurveMult * (-height * x * x + height * x);

            var mid = Vector2.Lerp(start, end, t);

            return new Vector2(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t));
        }
        
        public static Vector3 SmoothCurveX(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => CurveMult * (-height * x * x + height * x);

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(Func(t) + Mathf.Lerp(start.x, end.x, t), mid.y, mid.z);
        }
        
        public static Vector3 SmoothCurveY(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => CurveMult * (-height * x * x + height * x);

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        public static Vector3 SmoothCurveZ(Vector3 start, Vector3 end, float height, float t)
        {
            float Func(float x) => CurveMult * (-height * x * x + height * x);

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, mid.y, Func(t) + Mathf.Lerp(start.z, end.z, t));
        }
        
        public static Vector3 SmoothCurveEaseX(Vector3 start, Vector3 end, float height, float t, 
            NEaseTypes firstEase = NEaseTypes.InOutSine,  NEaseTypes secondEase = NEaseTypes.InOutSine)
        {
            float Func(float x) => x  < .5f ? EaseHelper.Evaluate(x * 2, 1f, firstEase) * height : 
                height - EaseHelper.Evaluate(((x - .5f) * 2f), 1f, secondEase) * height ;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(Func(t) + Mathf.Lerp(start.x, end.x, t), mid.y, mid.z);
        }
        
        public static Vector3 SmoothCurveEaseY(Vector3 start, Vector3 end, float height, float t, 
            NEaseTypes firstEase = NEaseTypes.InOutSine,  NEaseTypes secondEase = NEaseTypes.InOutSine)
        {
            float Func(float x) => x  < .5f ? EaseHelper.Evaluate(x * 2, 1f, firstEase) * height : 
                height - EaseHelper.Evaluate(((x - .5f) * 2f), 1f, secondEase) * height ;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }
        
        public static Vector3 SmoothCurveEaseZ(Vector3 start, Vector3 end, float height, float t, 
            NEaseTypes firstEase = NEaseTypes.InOutSine,  NEaseTypes secondEase = NEaseTypes.InOutSine)
        {
            float Func(float x) => x  < .5f ? EaseHelper.Evaluate(x * 2, 1f, firstEase) * height : 
                height - EaseHelper.Evaluate(((x - .5f) * 2f), 1f, secondEase) * height ;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, mid.y, Func(t) + Mathf.Lerp(start.z, end.z, t));
        }
        
     
        
        public static float CartesianFormula(float x, float divisionFactor, float multiplierFactor)
        {
            var square = x * x;
            var y = square / divisionFactor;
            y = Mathf.Tan(y);
            y = y * multiplierFactor;
            return y;
        }
        
        public static float Truncate(float value, int digits)
        {
            if (digits<=0)
            {
                digits = 1;
            }
            value /= 10*digits;
            value *= 10*digits;
            return (float)value;
        }
    }
}