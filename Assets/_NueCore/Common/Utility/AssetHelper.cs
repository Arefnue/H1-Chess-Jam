using System;
using System.Collections.Generic;
using System.Text;
using _NueCore.Common.NueLogger;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _NueCore.Common.Utility
{
    public static class AssetHelper
    {
        
#if UNITY_EDITOR
        private static readonly StringBuilder Str = new StringBuilder();


        public static T GetScriptableObjectAtPath<T>(string path) where T: ScriptableObject
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        
        public static string GetObjectPath(Object obj)
        {
            return AssetDatabase.GetAssetPath(obj);
        }

        public static T CreateScriptableAsset<T>(string path,Action<T> buildAction = null) where T : ScriptableObject
        {
            T cloneData = ScriptableObject.CreateInstance<T>();
            buildAction?.Invoke(cloneData);
            AssetDatabase.CreateAsset(cloneData, path);
            return GetScriptableObjectAtPath<T>(path);
        }

        public static T CloneScriptableObject<T>(T refObject,string path) where T : ScriptableObject
        {
            T cloneData = ScriptableObject.CreateInstance<T>();
            EditorUtility.CopySerialized(refObject, cloneData);
            AssetDatabase.CreateAsset(cloneData, path);
            return GetScriptableObjectAtPath<T>(path);
        }
        
        public static void RenameAsset(Object targetAsset, string newName)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(targetAsset), newName);
        }
        
        public static void RenameAsset(string assetPath, string newName)
        {
            AssetDatabase.RenameAsset(assetPath, newName);
        }

        public static List<T> FindAllAssets<T>(string path,List<T> targetList) where T: Object
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(T)}", new[] {path});
            targetList.Clear();
            foreach (var asset in assets)
            {
                var targetPath = AssetDatabase.GUIDToAssetPath(asset);
                var loadedAsset = (T)AssetDatabase.LoadMainAssetAtPath(targetPath);
                targetList.Add(loadedAsset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return targetList;
        }
        
        public static List<Object> FindAllAssetsAsObject<T>(string path) where T: Object
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(T)}", new[] {path});
            var tList = new List<Object>();
            foreach (var asset in assets)
            {
                var targetPath = AssetDatabase.GUIDToAssetPath(asset);
                var loadedAsset = (T)AssetDatabase.LoadMainAssetAtPath(targetPath);
                tList.Add(loadedAsset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return tList;
        }
        
        public static string GetPathFormat(string targetPath,string fileName ,string prefix = "", string suffix = "", string assetIdentifier = ".asset")
        {
            Str.Clear();
            Str.Append(targetPath).Append("/");
            Str.Append(prefix);
            Str.Append(fileName);
            Str.Append(suffix);
            Str.Append(assetIdentifier);
            return Str.ToString();
        }
#endif
    }
}