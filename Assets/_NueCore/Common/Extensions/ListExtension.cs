using System;
using System.Collections.Generic;
using _NueCore.Common.NueLogger;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _NueCore.Common.Extensions
{
    public static class ListExtension
    {
        public static T RandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException("List is Empty");

            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }
        
        public static T RandomItemExclude<T>(this List<T> list, Func<T,bool> condition)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException("List is Empty");
            
            var randomIndex = Random.Range(0, list.Count);
            var item =list[randomIndex];
            if (condition == null)
            {
                return item;
            }
            var breakCheck = list.Count+10;
            while (condition.Invoke(item))
            {
                randomIndex += 1;
                randomIndex %= list.Count;
                item = list[randomIndex];
                breakCheck -= 1;
                if (breakCheck<=0)
                {
                    break;
                }
            }
            return item;
        }
        
        public static T RandomItemRemove<T>(this List<T> list)
        {
            var item = list.RandomItem();
            list.Remove(item);
            return item;
        }
        
        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            for (var i = 0; i <= n - 2; i++)
            {
                //random index
                var rdn = Random.Range(0, n - i);

                //swap positions
                (list[i], list[i + rdn]) = (list[i + rdn], list[i]);
            }
        }
        
        public static void AddToFront<T>(this List<T> list, T item) => list.Insert(0, item);
        
        public static void AddBeforeOf<T>(this List<T> list, T item, T newItem)
        {
            var targetPosition = list.IndexOf(item);
            list.Insert(targetPosition, newItem);
        }
        
        public static void AddAfterOf<T>(this List<T> list, T item, T newItem)
        {
            var targetPosition = list.IndexOf(item) + 1;
            list.Insert(targetPosition, newItem);
        }
        
        public static void Print<T>(this List<T> list, string log = "")
        {
            log += "[";
            for (var i = 0; i < list.Count; i++)
            {
                log += list[i].ToString();
                log += i != list.Count - 1 ? ", " : "]";
            }

            Debug.Log(log);
        }
    }
}