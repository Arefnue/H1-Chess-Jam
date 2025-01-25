using _NueCore.Common.Utility;
using UnityEngine;

namespace _NueExtras.StockSystem
{
    [CreateAssetMenu(fileName = "StockAction_Increase_X",menuName = "StockSystem/SpriteCatalog")]
    public class StockSpriteCatalog : SpriteCatalog
    {
        public Sprite GetStockSprite(StockTypes stockType)
        {
            var stockID = $"Stock_{stockType}";
            return GetSprite(stockID);
        }
    }
}