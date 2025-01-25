using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace _NueCore.SaveSystem
{
    public class NDiscWriter
    {
        public const string Identifier = ".nue";
        private string _uniquePath;
        private static string _cachedPath;
        
        public NDiscWriter(string uniquePath)
        {
            _uniquePath = uniquePath;
        } 
        
        public void WriteDisc<T>(T data)
        {
            var path = GetPath();
            var saveData = JsonUtility.ToJson(data);
            
            File.WriteAllText(path, saveData, Encoding.UTF8);
        }
        
        public void DeleteDisc()
        {
            var path = GetPath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        
        public T ReadDisc<T>()
        {
            var path = GetPath();
            if (!File.Exists(path)) return (T) default;

            var content = File.ReadAllText(path,Encoding.UTF8);
            var obj = JsonUtility.FromJson<T>(content);
            return obj;
        }
        
        
        private string GetPath()
        {
            if (string.IsNullOrEmpty(_cachedPath) || string.IsNullOrWhiteSpace(_cachedPath))
            {
                _cachedPath = Application.persistentDataPath + "/";
            }
            return _cachedPath + _uniquePath + Identifier;
        }
    }
}