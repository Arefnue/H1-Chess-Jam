using System.Text;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _NueExtras.StockSystem
{
    public class StockText : MonoBehaviour
    {
        [SerializeField] private StockTypes targetStockType;
        [SerializeField] private TMP_Text stockTextField;
        [SerializeField] private string prefix;
        [SerializeField] private string suffix;
        [SerializeField] private bool getRounded = true;
        [SerializeField,HideIf(nameof(getRounded))] private string floatFormat = "0.00";
        
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        public void UpdateText()
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(prefix);
            _stringBuilder.Append(getRounded ? StockStatic.GetStockRounded(targetStockType) : StockStatic.GetStock(targetStockType).ToString(floatFormat));
            _stringBuilder.Append(suffix);
            stockTextField.SetText(_stringBuilder.ToString());
        }
        
        
    }
}