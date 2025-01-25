using System.Text;
using UnityEngine;

namespace _NueCore.Common.Extensions
{
    public static class ColorExtension
    {
        public static string Colorize(this string text, Color color)
        {
            var str = new StringBuilder();
            str.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(color)).Append(">").Append(text)
                .Append("</color>");
            return str.ToString();
        }
        
        public static string Colorize(this string text, string color)
        {
            var str = new StringBuilder();
            str.Append("<color=").Append(color).Append(">").Append(text)
                .Append("</color>");
            return str.ToString();
        }
       
    }
}