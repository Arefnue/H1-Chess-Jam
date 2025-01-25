using System;
using UnityEngine;

namespace __Project.Systems.GridSystem._PathSubSystem
{
    [Serializable]
    public class BaseNodeTile : BaseNode
    {
        public BaseNodeTile(Vector3Int currentNodeNodePosition) : base(currentNodeNodePosition)
        {
            
        }

        [SerializeField] private GridTileBase gridTile;
        
      
        public GridTileBase GridTile => gridTile;

        public override bool GetIsWalkable()
        {
            if (GridTile)
                return false;
            return base.GetIsWalkable();
        }

        public void SetGridTile(GridTileBase gridTileBase)
        {
            gridTile = gridTileBase;
        }
        
        public virtual string GetNodeName()
        {
            return gridTile ? gridTile.name : "Null";
        }
    }
}