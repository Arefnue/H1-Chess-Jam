using System.Text;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public static class TimeHelper
    {
        private static StringBuilder Str { get; set; } = new StringBuilder();
        public static int GetRemainingDuration(int targetDuration,int startTime)
        {
            var passedTime = (int)Time.time - startTime;
            var remaining = targetDuration - passedTime > 0 ? targetDuration - passedTime : 0;
            return remaining;
        }

        public static string ConvertToTimer(int targetSeconds)
        {
            Str.Clear();
            var minute = targetSeconds / 60;
            var seconds = targetSeconds % 60;
            var hour = minute / 60;

            if (hour>=2)
            {
                if (hour<10)
                    Str.Append(0);
                
                Str.Append(hour).Append(":");
                minute -= hour * 60;
            }
            
            if (minute<10)
                Str.Append(0);
            
            Str.Append(minute).Append(":");
            
            if (seconds<10)
                Str.Append(0);
            
            Str.Append(seconds);
            
            return Str.ToString();
        }
    }
}