using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.GridSystem;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class NTile_Chess : NTileBase
    {
        [SerializeField] private bool isWall;

        
        public bool BlockWalk { get; set; }
        
        public override bool IsWalkable()
        {
            if (isWall)
            {
                return false;
            }

            if (BlockWalk)
            {
                return false;
            }

            var bc = base.IsWalkable();
            if (!bc)
            {
                return false;
            }
            return true;
        }
    }
}