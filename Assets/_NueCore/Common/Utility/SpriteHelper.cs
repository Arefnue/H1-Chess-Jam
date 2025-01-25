using System;
using System.Text;
using _NueExtras.StockSystem;
using UnityEditor;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public static class SpriteHelper
    {
        private static readonly StringBuilder Str = new StringBuilder();

        public static Sprite GetRandomSprite()
        {
#if UNITY_EDITOR
            var assets =AssetDatabase.FindAssets("t:sprite");
            var randomIndex = UnityEngine.Random.Range(0, assets.Length);
            var assetPath = AssetDatabase.GUIDToAssetPath(assets[randomIndex]);
            return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
#endif
            return null;
        }
        public static string GetSpriteText(string itemName)
        {
            Str.Clear();
            Str.Append("<sprite name=");
            Str.Append('"');
            Str.Append(itemName);
            Str.Append('"');
            Str.Append('>');
            return Str.ToString();
        }

        public static string GetStockSpriteName(StockTypes stockType)
        {
            var itemName = stockType switch
            {
                StockTypes.Coin => "coin",
                StockTypes.Gem => "gem",
                StockTypes.Emerald => "emerald",
                _ => throw new ArgumentOutOfRangeException(nameof(stockType), stockType, null)
            };
        
            return itemName;
        }
        
        public static string GetStockSpriteText(StockTypes stockType)
        {
            return GetSpriteText(GetStockSpriteName(stockType));
        }
    }
}