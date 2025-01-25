using System;

namespace _NueCore.NStatSystem
{
    [Serializable]
    public class NStat
    {
        public string key;
        public float value;
        public int statType;
        public NStatType GetStatType()
        {
            return (NStatType)statType;
        }
        public static NStat operator +(NStat a, NStat b)
        {
            if (a.key != b.key)
            {
                throw new InvalidOperationException("Cannot add NStats with different keys");
            }

            return new NStat
            {
                key = a.key,
                value = a.value + b.value,
                statType = a.statType
            };
        }
    }
}