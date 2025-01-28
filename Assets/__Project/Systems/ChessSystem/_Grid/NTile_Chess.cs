using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.GridSystem;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class NTile_Chess : NTileBase
    {
        [SerializeField] private bool isWall;

        public override bool IsWalkable()
        {
            if (isWall)
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