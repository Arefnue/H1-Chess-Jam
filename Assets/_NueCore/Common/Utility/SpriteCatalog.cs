using System.Collections.Generic;
using System.Linq;
using _NueCore.Common.NueLogger;
using _NueCore.Common.Sandbox;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _NueCore.Common.Utility
{
    [CreateAssetMenu(fileName = "Sprite Catalog", menuName = "NueData/Utils/SpriteCatalog", order = 0)]
    public class SpriteCatalog : NScriptableObject<SpriteCatalog>
    {
        [SerializeField] private List<Sprite> spriteList;
        [SerializeField,FolderPath] private string path;

        public List<Sprite> SpriteList => spriteList;

        public virtual Sprite GetSprite(string id)
        {
            var targetSprite =spriteList.FirstOrDefault(x => x.name == id);

            if (targetSprite == null)
            {
                $"There is no {id} sprite!!!!".NLog(Color.red);
            }
            return targetSprite;
        }

#if UNITY_EDITOR
        [Button]
        public void FindAllEditor()
        {
            var assets = AssetDatabase.FindAssets($"t:{nameof(Sprite)}", new[] {path});
            SpriteList.Clear();
            foreach (var asset in assets)
            {
                var targetPath = AssetDatabase.GUIDToAssetPath(asset);
                var loadedAsset = (Sprite)AssetDatabase.LoadAssetAtPath(targetPath,typeof(Sprite));
               
                SpriteList.Add(loadedAsset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
        
    }
}