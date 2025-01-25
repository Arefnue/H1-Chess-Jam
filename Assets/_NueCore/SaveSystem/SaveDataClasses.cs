using System;

namespace _NueCore.SaveSystem
{
    [Serializable]
    public class StringToFloat
    {
        public string key;
        public float value;
    }
    
    [Serializable]
    public class StringToInt
    {
        public string key;
        public int value;
        
        public StringToInt(string key, int value)
        {
            this.key = key;
            this.value = value;
        }
    }
    
    [Serializable]
    public class StringToString
    {
        public string key;
        public string value;
    }
    
    [Serializable]
    public class IntToString
    {
        public int key;
        public string value;
    }
    
    [Serializable]
    public class IntToFloat
    {
        public int Key;
        public float Value;

        public IntToFloat(int key, float value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}