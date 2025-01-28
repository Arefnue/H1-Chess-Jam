using System;
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
                return false;
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