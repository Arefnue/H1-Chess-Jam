using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_Knight : ChessPieceBase
    {
        private Vector3Int[] directions = new Vector3Int[] { new Vector3Int(1, 2, 0), new Vector3Int(2, 1, 0), new Vector3Int(2, -1, 0), new Vector3Int(1, -2, 0), new Vector3Int(-1, -2, 0), new Vector3Int(-2, -1, 0), new Vector3Int(-2, 1, 0), new Vector3Int(-1, 2, 0) };


        public override Tween MoveTween(Vector3Int targetPos)
        {
	        var finalDest = GridLayer.Grid.GetCellCenterLocal(targetPos);
	        finalDest.y = 0;
	        return transform.DOJump(finalDest,2,1, 0.5f);
        }
        public override List<Vector3Int> FindAvailableTiles()
        {
            AvailableMoveList.Clear();
			foreach (var direction in directions)
			{
				var nextPos = OccupiedTilePosition + direction;
				if (GridLayer.IsPositionOnGrid(nextPos))
				{
					if (!GridLayer.GetNode(nextPos).GetIsWalkable())
					{
						continue;
					}
					if (GridLayer.IsPositionOccupied(nextPos))
						continue;
					if (GridLayer.GetNode(nextPos).NTileBase)
					{
						AvailableMoveList.Add(nextPos);
					}
				}
			}

			return AvailableMoveList;
        }
    }
}