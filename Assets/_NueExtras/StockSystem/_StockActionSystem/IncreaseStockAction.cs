using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueExtras.StockSystem._StockActionSystem
{
    [CreateAssetMenu(fileName = "StockAction_Increase_X",menuName = "StockSystem/StockActions/IncreaseStockAction")]
    public class IncreaseStockAction : StockActionBase
    {
        [SerializeField] private StockTypes stockType;
        [SerializeField] private int increaseAmount;
        [SerializeField] private bool useSpawner;
        [SerializeField,ShowIf(nameof(useSpawner))] private int spawnCount = 10;

        public override void TriggerAction()
        {
            if (increaseAmount<0)
            {
                StockStatic.DecreaseStock(stockType,Mathf.Abs(increaseAmount));
            }
            else
            {
                if (useSpawner)
                {
                    var perAmount = (float) increaseAmount / spawnCount;
                    StockStatic.SpawnStock(stockType,spawnCount,perAmount);
                }
                else
                {
                    StockStatic.IncreaseStock(stockType,increaseAmount);
                }
            }
            
        }
    }
}