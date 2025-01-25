using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.NStatSystem
{
    [CreateAssetMenu(fileName = "NStatContainer", menuName = "NueCore/NStatSystem/NStatContainer", order = 0)]
    public class NStatContainer : ScriptableObject
    {
        [SerializeField] private string key;
        [SerializeField] private string title;
        
        public string Title => title;

        public string Key => key;


        [Button]
        private void SetKey(bool apply)
        {
            if (apply)
            {
                key =name.Replace("NStat_", "");
                title = Key;
            }
           
        }
        // public float GetValue()
        // {
        //     return ShopStatic.Temp.GetTotalNStatValue(key);
        // }
        //
        // public float GetPercent()
        // {
        //     return ShopStatic.Temp.GetTotalNStatPercent(key);
        // }
        //
        //
        // public float GetValueReversed()
        // {
        //     var value = ShopStatic.Temp.GetTotalNStatValue(key);
        //     if (value==0)
        //         return 0.0001f;
        //
        //     return 1 / value;
        // }
        //
        // public int GetValueRounded()
        // {
        //     return ShopStatic.Temp.GetTotalNStatValueRounded(key);
        // }
    }
}