using _NueCore.Common.Utility;
using _NueExtras.StockSystem;
using TMPro;
using UnityEngine;

namespace __Project.Systems.LevelSystem._LevelEndSubSystem
{
    public class LevelEnd_Win : LevelEndPopup
    {
        [SerializeField] private TMP_Text coinRewardText;
        [SerializeField] private TMP_Text colorRewardText;
        public override void Build(LevelEndInfo levelEndInfo)
        {
            base.Build(levelEndInfo);
            var reward = levelEndInfo.CoinReward;
            var count = reward;
            var perValue = 1;

            if (count>10)
            {
                perValue = count / 10;
                count = 10;
            }
            
            StockStatic.SpawnStock(StockTypes.Coin, count,perValue);
            var text = reward + SpriteHelper.GetStockSpriteText(StockTypes.Coin);
            coinRewardText.SetText(coinRewardText.text.Replace("#reward", text));
            var colorText = levelEndInfo.ColorReward + SpriteHelper.GetStockSpriteText(StockTypes.Gem);
            colorRewardText.SetText(colorRewardText.text.Replace("#reward", colorText));
            StockStatic.IncreaseStock(StockTypes.Gem,levelEndInfo.ColorReward);
            
        }
    }
}