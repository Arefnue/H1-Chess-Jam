using System.Collections.Generic;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_Rook : ChessPieceBase
    {
        private Vector3Int[] directions = new Vector3Int[] { Vector3Int.left, Vector3Int.up, Vector3Int.right, Vector3Int.down };

        public override List<Vector3Int> FindAvailableTiles()
        {
            AvailableMoveList.Clear();
            foreach (var direction in directions)
            {
                var nextPos = OccupiedTilePosition + direction;
                while (GridLayer.IsPositionOnGrid(nextPos))
                {
                    if (!GridLayer.GetNode(nextPos).GetIsWalkable())
                    {
                        break;
                    }
                    AvailableMoveList.Add(nextPos);
                    nextPos += direction;
                    if (GridLayer.IsPositionOccupied(nextPos))
                        break;

                }
            }

            return AvailableMoveList;
        }
    }
}