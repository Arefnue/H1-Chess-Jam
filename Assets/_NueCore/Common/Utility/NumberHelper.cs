using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace _NueCore.Common.Utility
{
    public static class NumberHelper
    {
        #region Cache
        private static StringBuilder Str { get; }= new StringBuilder();

        private static readonly SortedDictionary<int, (decimal, string)> ConvertTargetDict =
            new SortedDictionary<int, (decimal, string)>
            {
                {0, (1000, "K")},
                {1, (1000000, "M")},
                {2, (1000000000, "B")},
                {3, (1000000000000, "T")},
                {4, (1000000000000000, "Qa")},
                {5, (1000000000000000000, "Qu")}
            };

        #endregion
        
        public static (decimal, string) GetConvertValue(decimal number)
        {
            for (var i = ConvertTargetDict.Count - 1; i >= 0; i--)
            {
                var tuple = ConvertTargetDict[i];
                if (number >= tuple.Item1)
                {
                    var roundedNumber = number / tuple.Item1;
                    return (roundedNumber, tuple.Item2);
                }
            }

            return (number, "");
        }
        
        public static string ToAbbreviate(decimal number, bool singleDigit = false)
        {
            Str.Clear();
            var numberTuple = GetConvertValue(number);

            var digitCount = number<10000 ? 2 : 1;
            if (singleDigit)
                digitCount = 1;
            
            var targetNumber = numberTuple.Item1;
            var flooredValue = Math.Floor(targetNumber);
            var decimalPoint = targetNumber - flooredValue;

            var decimalStr = "";
            if (decimalPoint > 0)
            {
                decimalStr = decimalPoint.ToString(CultureInfo.InvariantCulture);
                decimalStr = decimalStr.TrimStart('0');
                if (decimalStr.Length >= 3)
                {
                    decimalStr = decimalStr.Substring(0, 3);

                    if (digitCount == 1)
                    {
                        decimalStr = decimalStr.TrimEnd('0');
                        if (decimalStr.Length == 3)
                        {
                            decimalStr = decimalStr.Remove(decimalStr.Length - 1, 1);
                            
                            if (decimalStr[^1] == '0')
                                decimalStr = decimalStr.TrimEnd('0');
                        }
                    }
                    else
                    {
                        if (decimalStr.Length < digitCount + 1)
                        {
                            var length = decimalStr.Length;
                            for (int i = length; i < digitCount + 1; i++)
                                decimalStr += '0';
                        }
                    }
                    
                    if (decimalStr.Length == 1)
                        decimalStr = "";
                }
            }


            var lastText = flooredValue.ToString(CultureInfo.InvariantCulture);
            
            Str.Append(lastText);
            Str.Append(decimalStr);
            if (decimalStr.Length == 2 && digitCount == 2)
                Str.Append('0');
            Str.Append(numberTuple.Item2);
            return Str.ToString();
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) return string.Empty;
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return string.Empty;
        }
    }
}