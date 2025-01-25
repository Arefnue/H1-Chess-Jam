using System;
using UnityEngine;

namespace _NueCore.Common.KeyValueDict
{
    [Serializable]
    public class KeyValue<K, V>
    {
        [SerializeField] private K key;
        [SerializeField] private V value;

        public K Key
        {
            get { return key; }
            set { key = value; }
        }

        public V Value
        {
            get => value;
            set => this.value = value;
        }

        public KeyValue(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }
}