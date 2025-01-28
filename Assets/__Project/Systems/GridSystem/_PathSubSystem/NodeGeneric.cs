using System;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.GridSystem._PathSubSystem
{
    [Serializable]
    public class NodeGeneric<TTile> : BaseNode where TTile : NTileBase
    {
        public NodeGeneric(Vector3Int currentNodeNodePosition) : base(currentNodeNodePosition)
        {
            
        }

        [SerializeField] private TTile nTileBase;
        public TTile NTileBase => nTileBase;

        public override bool GetIsWalkable()
        {
            if (NTileBase)
            {
                if (!NTileBase.IsWalkable())
                    return false;
            }
            
            return base.GetIsWalkable();
        }

        public void SetGridTile(TTile tile)
        {
            this.nTileBase = tile;
        }
        
        public virtual string GetNodeName()
        {
            return nTileBase ? nTileBase.name : "Null";
        }
    }
}