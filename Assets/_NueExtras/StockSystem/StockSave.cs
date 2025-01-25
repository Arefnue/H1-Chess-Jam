using System;
using System.Collections.Generic;
using _NueCore.SaveSystem;

namespace _NueExtras.StockSystem
{
    public class StockSave : NBaseSave
    {
        #region Setup
        private const string SavePath = "StockSave";
        protected override string GetSavePath()
        {
            return SavePath;
        }
        public override void Save()
        {
            NSaver.SaveData<StockSave>();
        }

        public override void Load()
        {
            NSaver.GetSaveData<StockSave>();
        }

        public override void ResetSave()
        {
            NSaver.ResetSave<StockSave>();
        }

        public override SaveTypes GetSaveType()
        {
           return SaveTypes.Global;
        }
        #endregion

        public bool IsFirstTimeStocksGiven;
        public List<StockSaveData> StocksSaveList = new List<StockSaveData>();

        public void SetStockValue(StockTypes targetStock, float value)
        {
            var stockId = (int) targetStock;
            var stockSaveData = StocksSaveList.Find(x => x.stockId == stockId);
            if (stockSaveData == null)
            {
                stockSaveData = new StockSaveData(stockId, value);
                StocksSaveList.Add(stockSaveData);
            }
            else
            {
                stockSaveData.stockValue = value;
            }
        }
        
        public float GetStockValue(StockTypes targetStock)
        {
            var stockId = (int) targetStock;
            var stockSaveData = StocksSaveList.Find(x => x.stockId == stockId);
            return stockSaveData?.stockValue ?? 0;
        }
        
        [Serializable]
        public class StockSaveData
        {
            public StockSaveData(int stockId, float stockValue)
            {
                this.stockId = stockId;
                this.stockValue = stockValue;
            }
            public int stockId;
            public float stockValue;
        }
    }
}