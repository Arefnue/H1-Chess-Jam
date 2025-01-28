using System.Collections.Generic;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_Pawn : ChessPieceBase
    {
        private Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1,- 1),
        };
        public override List<Vector3Int> FindAvailableTiles()
        {
            AvailableMoveList.Clear();
            foreach (var direction in directions)
            {
                var nextPos = OccupiedTilePosition + new Vector3Int(direction.x, direction.y, 0);
                while (GridLayer.IsPositionOnGrid(nextPos))
                {
                    var nextNode = GridLayer.GetNode(nextPos);
                    if (nextNode.GetIsWalkable())
                    {
                        if (nextNode.NTileBase)
                        {
                            AvailableMoveList.Add(nextPos);
                        }
                        break;
                    }
                    AvailableMoveList.Add(nextPos);
                    nextPos += new Vector3Int(direction.x, direction.y, 0);
                }
            }

            return AvailableMoveList;
        }
    }
}