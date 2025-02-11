using System;
using System.Collections.Generic;
using System.Linq;
using _NueCore.Common.ReactiveUtils;
using _NueCore.SaveSystem;
using _NueExtras.StockSystem._StockSpawnerSubSystem;
using UnityEngine;

namespace _NueExtras.StockSystem
{
    public static class StockStatic
    {
        #region Cache
        private static StockREvents.StockValueChangedREvent _valueChangedREvent;
        private static StockREvents.StockOutREvent _stockOutREvent;
        private static Dictionary<StockTypes, float> StockValuesDict { get; set; } =
            new Dictionary<StockTypes, float>();
        
     
        #endregion
        
        #region Setup

        public static void InitStocks()
        {
            var types = Enum.GetNames(typeof(StockTypes));
            var saveData = NSaver.GetSaveData<StockSave>();
            var canSave = false;
            for (var i = 0; i < types.Length; i++)
            {
                var save = saveData.StocksSaveList.FirstOrDefault(x => x.stockId == i);
                if (save == null)
                {
                    var newStockSaveData = new StockSave.StockSaveData(i,0);
                    saveData.StocksSaveList.Add(newStockSaveData);
                    canSave = true;
                }
            }

            if (canSave)
                saveData.Save();

            LoadStocks();
        }

        private static void LoadStocks()
        {
            var saveData = NSaver.GetSaveData<StockSave>();
            foreach (var stockSaveData in saveData.StocksSaveList)
            {
                var convertedType = (StockTypes)stockSaveData.stockId;
                if (StockValuesDict.ContainsKey(convertedType))
                {
                    StockValuesDict[convertedType] = stockSaveData.stockValue;
                    continue;
                }

                StockValuesDict.Add(convertedType, stockSaveData.stockValue);
                RBuss.Publish(new StockREvents.StockInitREvent(convertedType, stockSaveData.stockValue));
                PublishValueChangedREvent(convertedType, 0, stockSaveData.stockValue);
            }
        }

        public static void SaveStocks()
        {
            var saveData = NSaver.GetSaveData<StockSave>();

            foreach (var kvp in StockValuesDict)
            {
                var key = (int)kvp.Key;
                var value = kvp.Value;
                var item = saveData.StocksSaveList.FirstOrDefault(x => x.stockId == key);
                if (item != null)
                    item.stockValue = value;
                else
                    saveData.StocksSaveList.Add(new StockSave.StockSaveData(key,value));
            }
            saveData.Save();
        }

        #endregion

        #region Custom Collect
        public static void CollectCustomStock(ICustomStock customStock,StockTypes targetType,Action onCollectedAction)
        {
            if (StockSpawnerController.StockMoveTargetDict.TryGetValue(targetType,out var targetRect))
            {
                StockSpawnerController.Move3DToUIWithPhysicGlobal(customStock, targetRect.transform,
                    () =>
                    {
                        onCollectedAction?.Invoke();
                        customStock.Dispose();
                    },1);
            }
           
        }
        public static void CollectCustomStock(ICustomStock customStock,Transform target,Action onCollectedAction)
        {
            StockSpawnerController.Move3DToUIWithPhysicGlobal(customStock, target.transform,
                () =>
                {
                    onCollectedAction?.Invoke();
                    customStock.Dispose();
                },0.5f);
        }
        #endregion
        
        #region Spawn Methods
      
        public static void SpawnStock(StockTypes stockType, int count, float increasePerAmount = 1, Action onFinished = null)
        {
           
            for (int i = 0; i < count; i++)
            {
                StockSpawnerStatic.SpawnStock(stockType, o =>
                {
                    IncreaseStock(stockType, increasePerAmount);
                    onFinished?.Invoke();
                });
            }
        }

        public static void SpawnStock(StockTypes stockType, Vector3 startPos, int count, float increasePerAmount = 1, Action onFinished = null)
        {
            for (int i = 0; i < count; i++)
            {
                StockSpawnerStatic.SpawnStock(stockType, startPos, o =>
                {
                    IncreaseStock(stockType, increasePerAmount);
                    onFinished?.Invoke();
                });
            }
        }

        public static void SpawnStock3D(StockTypes stockType, int count, float increasePerAmount = 1, Action onFinished = null)
        {
            for (int i = 0; i < count; i++)
            {
                StockSpawnerStatic.SpawnStock3D(stockType, o =>
                {
                    IncreaseStock(stockType, increasePerAmount);
                    onFinished?.Invoke();
                });
            }
        }

        public static void SpawnStock3D(StockTypes stockType, Vector3 startPos, int count, float increasePerAmount = 1, Action onFinished = null)
        {
           
            for (int i = 0; i < count; i++)
            {
                StockSpawnerStatic.SpawnStock3D(stockType, startPos, o =>
                {
                    IncreaseStock(stockType, increasePerAmount);
                    onFinished?.Invoke();
                });
            }
        }

        #endregion

        #region Despawn

        public static void DeSpawnStock(StockTypes stockType, Transform target, int count, float decreasePerAmount = 1, Action perFinishedAction = null, Action lastFinishedAction = null)
        {
            for (int i = 0; i < count; i++)
            {
                var index = i;
                var isLast = index >= count - 1;
                StockSpawnerStatic.DeSpawnStock(stockType, target, o =>
                {
                    DecreaseStock(stockType, decreasePerAmount);
                    perFinishedAction?.Invoke();
                    if (index >= count - 1)
                    {
                        lastFinishedAction?.Invoke();
                    }
                }, isLast);
            }
        }

        #endregion

        #region Logic

        public static bool CanPurchase(StockTypes stockType, float amount)
        {
            var stock = GetStockInternal(stockType);
            return stock >= amount;
        }

        public static bool CanPurchase(StockTypes stockType, int amount)
        {
            var stock = GetStockInternal(stockType);
            return stock >= amount;
        }

        #endregion
        
        #region Temp
        
        private static Dictionary<StockTypes, float> TempStockValuesDict { get; set; } =
            new Dictionary<StockTypes, float>();
        public static float GetTempStock(StockTypes targetType)
        {
            if (TempStockValuesDict.TryGetValue(targetType, out var stock))
                return stock;

            TempStockValuesDict.Add(targetType, 0);
            return TempStockValuesDict[targetType];
        }
        
        public static int GetTempStockRounded(StockTypes targetType)
        {
            return Mathf.RoundToInt(GetTempStock(targetType));
        }

        public static void ResetTempStock()
        {
            TempStockValuesDict.Clear();
        }
        public static void SetTempStock(StockTypes targetType, float value)
        {
            if (!TempStockValuesDict.ContainsKey(targetType))
            {
                TempStockValuesDict.Add(targetType, value);
            }
            else
            {
                TempStockValuesDict[targetType] = value;
            }
        }
        
        public static void IncreaseTempStock(StockTypes targetType, float amount)
        {
            if (!TempStockValuesDict.TryAdd(targetType, amount))
            {
                TempStockValuesDict[targetType] += amount;
            }
        }
        
        public static void DecreaseTempStock(StockTypes targetType, float amount)
        {
            if (!TempStockValuesDict.TryAdd(targetType, 0))
            {
                TempStockValuesDict[targetType] -= amount;
                if (TempStockValuesDict[targetType] < 0)
                {
                    TempStockValuesDict[targetType] = 0;
                }
            }
        }

        #endregion
        
        #region Operations

        public static float GetStock(StockTypes targetType)
        {
            if (StockValuesDict.TryGetValue(targetType, out var stock))
                return stock;

            StockValuesDict.Add(targetType, 0);
            return StockValuesDict[targetType];
        }

        public static int GetStockRounded(StockTypes targetType)
        {
            if (StockValuesDict.TryGetValue(targetType, out var stock))
                return Mathf.RoundToInt(stock);

            StockValuesDict.Add(targetType, 0);
            return Mathf.RoundToInt(StockValuesDict[targetType]);
        }

        public static void SetStock(StockTypes targetType, float value)
        {
            var unchangedValue = GetStockInternal(targetType);
            if (!StockValuesDict.ContainsKey(targetType))
            {
                StockValuesDict.Add(targetType, value);
            }
            else
            {
                StockValuesDict[targetType] = value;
            }

            var changedValue = GetStockInternal(targetType);
            PublishValueChangedREvent(targetType, unchangedValue, changedValue);
        }

        public static void IncreaseStock(StockTypes targetType, float amount)
        {
            var unchangedValue = GetStockInternal(targetType);
            if (!StockValuesDict.ContainsKey(targetType))
            {
                StockValuesDict.Add(targetType, amount);
            }
            else
            {
                StockValuesDict[targetType] += amount;
            }
            IncreaseTempStock(targetType,amount);
            var changedValue = GetStockInternal(targetType);
            PublishValueChangedREvent(targetType, unchangedValue, changedValue); 
            SaveStocks();
        }

        public static void DecreaseStock(StockTypes targetType, float amount)
        {
            var unchangedValue = GetStockInternal(targetType);
            if (!StockValuesDict.ContainsKey(targetType))
            {
                StockValuesDict.Add(targetType, 0);
            }
            else
            {
                StockValuesDict[targetType] -= amount;

                if (StockValuesDict[targetType] < 0)
                {
                    var remainingAmount = Mathf.Abs(StockValuesDict[targetType]);
                    StockValuesDict[targetType] = 0;

                    if (_stockOutREvent == null)
                        _stockOutREvent = new StockREvents.StockOutREvent(targetType, remainingAmount);
                    else
                        _stockOutREvent.SetValues(targetType, remainingAmount);

                    RBuss.Publish(_stockOutREvent);
                }
            }
            DecreaseTempStock(targetType,amount);
            var changedValue = GetStockInternal(targetType);

            PublishValueChangedREvent(targetType, unchangedValue, changedValue);
            SaveStocks();
        }
        #endregion
        
        #region Local Methods
        private static float GetStockInternal(StockTypes targetType)
        {
            return GetStock(targetType);
        }
        private static void PublishValueChangedREvent(StockTypes targetType, float unchangedValue, float changedValue)
        {
            if (_valueChangedREvent == null)
                _valueChangedREvent =
                    new StockREvents.StockValueChangedREvent(targetType, unchangedValue, changedValue);
            else
                _valueChangedREvent.SetValues(targetType, unchangedValue, changedValue);

            RBuss.Publish(_valueChangedREvent);
        }

        #endregion
    }
}