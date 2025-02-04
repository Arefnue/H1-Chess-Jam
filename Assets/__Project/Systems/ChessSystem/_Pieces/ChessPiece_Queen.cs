using System.Collections.Generic;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_Queen : ChessPieceBase
    {
        private Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            new Vector3Int(1, 1),
            new Vector3Int(1, -1),
            new Vector3Int(-1, 1),
            new Vector3Int(-1,- 1),
        };

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
                    if (GridLayer.IsPositionOccupied(nextPos))
                        break;
                    AvailableMoveList.Add(nextPos);
                    nextPos += direction;
                }
            }

            return AvailableMoveList;
            
            
        }
    }
}