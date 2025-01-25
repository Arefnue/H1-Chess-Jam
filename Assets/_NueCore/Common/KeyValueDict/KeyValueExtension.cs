using System;
using System.Collections.Generic;

namespace _NueCore.Common.KeyValueDict
{
    public static class KeyValueExtension
    {
         public static Tuple<int, T> GetMin<T>(this KeyValueDict<int, T> dict)
        {
            int key = int.MaxValue;
            T value = default;

            foreach (var kvp in dict)
            {
                if (key <= kvp.Key)
                    continue;

                key = kvp.Key;
                value = kvp.Value;
            }
            return new Tuple<int, T>(key, value);;
        }
        
        public static Tuple<int, T> GetMax<T>(this KeyValueDict<int, T> dict)
        {
            int key = int.MinValue;
            T value = default;

            foreach (var kvp in dict)
            {
                if (key > kvp.Key)
                    continue;

                key = kvp.Key;
                value = kvp.Value;
            }
            return new Tuple<int, T>(key, value);;
        }
        
        public static Tuple<float, T> GetMin<T>(this KeyValueDict<float, T> dict)
        {
            float key = float.MaxValue;
            T value = default;

            foreach (var kvp in dict)
            {
                if (key <= kvp.Key)
                    continue;

                key = kvp.Key;
                value = kvp.Value;
            }
            return new Tuple<float, T>(key, value);;
        }
        
        public static Tuple<float, T> GetMax<T>(this KeyValueDict<float, T> dict)
        {
            float key = float.MinValue;
            T value = default;

            foreach (var kvp in dict)
            {
                if (key > kvp.Key)
                    continue;

                key = kvp.Key;
                value = kvp.Value;
            }
            return new Tuple<float, T>(key, value);;
        }
        
        public static void Order<TKey, TValue>(this KeyValueDict<TKey, TValue> dict)
        {
            SortedDictionary<TKey, TValue> sortedDictionary = new SortedDictionary<TKey, TValue>();
            foreach (var kvp in dict)
            {
                sortedDictionary.Add(kvp.Key, kvp.Value);
            }
            dict.Clear();
            foreach (var kvp in sortedDictionary)
            {
                dict.Add(kvp.Key, kvp.Value);
            }
        }
    }
}