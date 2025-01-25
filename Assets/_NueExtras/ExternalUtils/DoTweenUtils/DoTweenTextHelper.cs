using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _NueExtras.ExternalUtils.DoTweenUtils
{
    public static class DoTweenTextHelper
    {
        private static StringBuilder _str = new StringBuilder();
        public static Sequence CountText(this Sequence seq,TMP_Text textField, int currentValue, int targetValue,string prefix = "",string suffix = "%",float duration = 0.5f, bool respondNegative = false,Ease ease = Ease.InQuad)
        {
            seq.Append(DOVirtual.Float(currentValue, targetValue, duration, value =>
            {
                _str.Clear();
                _str.Append(prefix);
                _str.Append(Mathf.RoundToInt(value));
                _str.Append(suffix);
                textField.SetText(_str.ToString());
            }).SetEase(ease));
            
            var oldColor = Color.white;
            textField.color = respondNegative ? Color.red : Color.green;
            
            seq.Join(textField.DOColor(oldColor, duration).SetEase(Ease.InBack));
            
            return seq;
        }
    }
}
