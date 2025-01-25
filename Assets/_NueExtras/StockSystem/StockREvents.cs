using _NueCore.Common.ReactiveUtils;
using UnityEngine;

namespace _NueExtras.StockSystem
{
    public abstract class StockREvents : MonoBehaviour
    {
        public class StockValueChangedREvent : REvent
        {
            public StockTypes StockType { get; private set; }
            public float Value { get; private set; }
            public float UnchangedValue { get; private set; }
            public float Delta { get; private set; }
            public int RoundedValue { get; private set; }
            public int RoundedUnchangedValue { get; private set; }
            public int RoundedDelta { get; private set; }

            public StockValueChangedREvent(StockTypes stockType, float unchangedValue, float changedValue)
            {
                SetValues(stockType,unchangedValue,changedValue);
            }

            public void SetValues(StockTypes stockType, float unchangedValue, float changedValue)
            {
                StockType = stockType;
                
                UnchangedValue = unchangedValue;
                if (UnchangedValue<0)
                    UnchangedValue = 0;
                Value = changedValue;
                if (Value<0)
                    Value = 0;
                Delta = changedValue - unchangedValue;

                RoundedValue = Mathf.RoundToInt(Value);
                RoundedUnchangedValue = Mathf.RoundToInt(UnchangedValue);
                RoundedDelta = Mathf.RoundToInt(Delta);

            }
        }
        
        public class StockOutREvent : REvent
        {
            public StockTypes StockType { get; private set; }
            public float RemainingAmount { get; private set; }
            public StockOutREvent(StockTypes stockType, float remainingAmount)
            {
               SetValues(stockType,remainingAmount);
            }

            public void SetValues(StockTypes stockType, float remainingAmount)
            {
                StockType = stockType;
                RemainingAmount = remainingAmount;
            }
        }
        
        public class StockInitREvent : REvent
        {
            public StockTypes StockType { get; private set; }
            public float Value { get; private set; }
            public float RoundedValue { get; private set; }
            public StockInitREvent(StockTypes stockType, float value)
            {
                SetValues(stockType,value);
            }

            public void SetValues(StockTypes stockType, float value)
            {
                StockType = stockType;
                Value = value;
                RoundedValue = Mathf.RoundToInt(value);
            }
        }
        
       
    }
}