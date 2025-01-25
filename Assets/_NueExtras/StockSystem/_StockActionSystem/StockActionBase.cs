using UnityEngine;

namespace _NueExtras.StockSystem._StockActionSystem
{
    public abstract class StockActionBase : ScriptableObject
    {
        public abstract void TriggerAction();
    }
}