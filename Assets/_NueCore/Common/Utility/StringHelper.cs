using System;
using System.Text;
using System.Text.RegularExpressions;

namespace _NueCore.Common.Utility
{
    public static class StringHelper
    {
        private static readonly Regex CamelCaseRegex = new Regex("([A-Za-z0-9]|\\G(?!^))([A-Z])");
        private static readonly Regex SnakeCaseRegex = new Regex("(.*?)_([a-zA-Z0-9])");
        private static readonly Regex WhitespaceRegex = new Regex("\\s+");
        private static readonly Regex SpecialCharRegex = new Regex("[^A-Z_]");
        
        private static StringBuilder TempStr { get; set; } = new StringBuilder();
        
      
        public static bool IsNull(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            return false;
        }
        public static string ReplaceTextSprite(char targetStr,string identifier = "!")
        {
            TempStr.Clear();
            TempStr.Append("<sprite name=").Append('"').Append(identifier).Append(targetStr).Append(identifier).Append('"').Append(">");
            return TempStr.ToString();
        }
        
        public static string ReplaceTextSprite(string targetStr,string identifier = "!")
        {
            TempStr.Clear();
            TempStr.Append("<sprite name=").Append('"').Append(identifier).Append(targetStr).Append(identifier).Append('"').Append(">");
            return TempStr.ToString();
        }

        public static int ExtractNumber(string txt)
        {
            if (string.IsNullOrEmpty(txt) || string.IsNullOrWhiteSpace(txt))
            {
                return -2;
            }
            string[] digits= Regex.Split(txt, @"\D+");
            for (int i = 0; i < digits.Length; i++)
            {
                var item = digits[i];
                if (int.TryParse(item, out var value))
                {
                    return value;
                }
            }

            return -1;
        }
        
        public static string Slugify(string txt)
        {
            string str = StringHelper.CamelCaseRegex.Replace(txt.Trim(), "$1_$2");
            string input = StringHelper.WhitespaceRegex.Replace(str.ToUpper(), "_");
            return StringHelper.SpecialCharRegex.Replace(input, "");
        }

        public static string UnSlugify(string txt)
        {
            string str1 = StringHelper.SnakeCaseRegex.Replace(txt.Trim().ToLower(), (MatchEvaluator) (match => match.Groups[1].ToString() + match.Groups[2].ToString().ToUpper()));
            string str2 = char.ToUpper(str1[0]).ToString();
            string str3 = str1;
            string str4 = str3.Substring(1, str3.Length - 1);
            return str2 + str4;
        }

        public static string CompactText(string text) => text.Trim();

        public static int GetDeterministicHashCode(string str)
        {
            int num1 = 352654597;
            int num2 = num1;
            for (int index = 0; index < str.Length; index += 2)
            {
                num1 = (num1 << 5) + num1 ^ (int) str[index];
                if (index != str.Length - 1)
                    num2 = (num2 << 5) + num2 ^ (int) str[index + 1];
                else
                    break;
            }
            return num1 + num2 * 1566083941;
        }
        
        public static T ExtractEnum<T>(string con) where T : Enum
        {
            var enumType = typeof(T);
            var enumValues = Enum.GetValues(enumType);
            foreach (var enumValue in enumValues)
            {
                if (con.Contains(enumValue.ToString()))
                {
                    return (T) enumValue;
                }
            }
            return default;
        }
        
    }
}