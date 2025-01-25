using System.Collections.Generic;

namespace _NueCore.NStatSystem
{
    public interface INStatHolder
    {
        public Dictionary<string, NStatSave> GetNStatDict();
        public void IncreaseNStat(string key, float value, NStatType increaseType = NStatType.Flat)
        {
            if (GetNStatDict().TryGetValue(key, out var stat))
            {
                if (increaseType == NStatType.Flat)
                    stat.flatValue += value;
                else
                    stat.percentValue += value;
            }
            else
            {
                var nStat = new NStatSave { key = key};
                if (increaseType == NStatType.Flat)
                    nStat.flatValue += value;
                else
                    nStat.percentValue += value;
                GetNStatDict().Add(key, nStat);
            }
        }
        public void DecreaseNStat(string key, float value, NStatType increaseType = NStatType.Flat)
        {
            if (GetNStatDict().TryGetValue(key, out var stat))
            {
                if (increaseType == NStatType.Flat)
                    stat.flatValue += value;
                else
                    stat.percentValue += value;
            }
            else
            {
                var nStat = new NStatSave { key = key};
                if (increaseType == NStatType.Flat)
                    nStat.flatValue += value;
                else
                    nStat.percentValue += value;
                GetNStatDict().Add(key, nStat);
            }
        }
        
    }
}