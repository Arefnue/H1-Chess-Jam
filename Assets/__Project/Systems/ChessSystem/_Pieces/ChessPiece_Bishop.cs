using System.Collections.Generic;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_Bishop : ChessPieceBase
    {
        private Vector3Int[] directions = new Vector3Int[] { new Vector3Int(1, 1, 0), new Vector3Int(1, -1, 0), new Vector3Int(-1, 1, 0), new Vector3Int(-1, -1, 0) };
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