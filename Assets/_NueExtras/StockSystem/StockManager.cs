using _NueCore.ManagerSystem.Core;
using _NueCore.SaveSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueExtras.StockSystem
{
    public class StockManager : NManagerBase
    {
        #region Setup
        public override void NStart()
        {
            base.NStart();
            StockStatic.InitStocks();
            var save = NSaver.GetSaveData<StockSave>();
            if (!save.IsFirstTimeStocksGiven)
            {
                save.IsFirstTimeStocksGiven = true;
                StockStatic.IncreaseStock(StockTypes.Coin,200);
            }
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            StockStatic.SaveStocks();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                StockStatic.SaveStocks();
        }
        #endregion
        
        #region Editor
#if UNITY_EDITOR
        [Button,BoxGroup("Editor",CenterLabel = true,ShowLabel = true)]
        private void ChangeStock(StockTypes stockType, float amount)
        {
            if (!Application.isPlaying) return;
            StockStatic.IncreaseStock(stockType,amount);
        }
#endif
        #endregion
    }
}
