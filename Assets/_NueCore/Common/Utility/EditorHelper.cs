using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    public static class EditorHelper
    {
#if UNITY_EDITOR
        public static void FindAll<T>(string path,List<T> targetList) where T: Object
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] {path});
            targetList.Clear();
            foreach (var asset in assets)
            {
                var targetPath = AssetDatabase.GUIDToAssetPath(asset);
                var loadedAsset = (T)AssetDatabase.LoadMainAssetAtPath(targetPath);
                targetList.Add(loadedAsset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void FindAll<T>(List<string> pathList,List<T> targetList) where T: Object
        {
            targetList.Clear();
            foreach (var path in pathList)
            {
                var assets = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] {path});
                foreach (var asset in assets)
                {
                    var targetPath = AssetDatabase.GUIDToAssetPath(asset);
                    var loadedAsset = (T)AssetDatabase.LoadMainAssetAtPath(targetPath);
                    if (targetList.Contains(loadedAsset))
                        continue;
                    targetList.Add(loadedAsset);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
      
    }
}