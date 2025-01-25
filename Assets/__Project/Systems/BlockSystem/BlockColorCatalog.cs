using System;
using System.Collections.Generic;
using System.Linq;
using _NueCore.Common.KeyValueDict;
using _NueCore.Common.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    [CreateAssetMenu(fileName = "Block_Color_Catalog", menuName = "BlockSystem/BlockColorCatalog", order = 0)]
    public class BlockColorCatalog : ScriptableObject
    {
        [SerializeField] private KeyValueDict<BlockColorEnum,ColorInfo> blockColorDict = new KeyValueDict<BlockColorEnum, ColorInfo>();
        [SerializeField,FolderPath] private string prefabPath;
        [SerializeField,FolderPath] private string materialPath;
        
        
        [Serializable]
        public class ColorInfo
        {
            public BlockColor BlockPrefab;
        }
        public BlockColor GetBlockColorPrefab(BlockColorEnum colorEnum)
        {
            return blockColorDict[colorEnum].BlockPrefab;
        }

    }
}