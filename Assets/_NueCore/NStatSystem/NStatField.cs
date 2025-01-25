using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.NStatSystem
{
    [Serializable]
    public class NStatField
    {
        [SerializeField,HideIf("@container != null || key.Length>0")] private NStatEnum statEnum;
        [SerializeField,HideIf("@container != null || statEnum != NStatEnum.None")] private string key = "";
        [SerializeField,HideIf("@key.Length>0 || statEnum != NStatEnum.None")] private NStatContainer container;
        [SerializeField] private float value;
        [SerializeField] private NStatType statType;
        
        public virtual string Key
        {
            get
            {
                if (statEnum != NStatEnum.None)
                {
                    return statEnum.GetStatName();
                }
                return container ? container.Key : key;
            }
        }

        public void ApplyToLocal(Dictionary<string,float> dict)
        {
            if (!dict.TryAdd(Key, value))
            {
                dict[Key] += value;
            }
        }

        public float GetValue()
        {
            return value;
        }

        public NStatType GetStatType()
        {
            return statType;
        }
    }
}