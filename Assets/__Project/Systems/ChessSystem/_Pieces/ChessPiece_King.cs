using System.Collections.Generic;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_King : ChessPieceBase
    {
        private Vector3Int[] directions = new Vector3Int[] { new Vector3Int(1, 1, 0), new Vector3Int(1, -1, 0), new Vector3Int(-1, 1, 0), new Vector3Int(-1, -1, 0), Vector3Int.left, Vector3Int.up, Vector3Int.right, Vector3Int.down };
        public override List<Vector3Int> FindAvailableTiles()
        {
            AvailableMoveList.Clear();
            foreach (var direction in directions)
            {
                var nextPos = OccupiedTilePosition + direction;
                if (GridLayer.IsPositionValid(nextPos))
                {
                    if (GridLayer.GetNode(nextPos).GetIsWalkable())
                    {
                        if (GridLayer.GetNode(nextPos).NTileBase)
                        {
                            AvailableMoveList.Add(nextPos);
                        }
                    }
                }
            }

            return AvailableMoveList;
        }
    }
}