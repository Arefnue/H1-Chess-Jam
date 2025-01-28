using System;
using System.Collections.Generic;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.GridSystem;
using DG.Tweening;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public abstract class ChessPieceBase : MonoBehaviour
    {
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }
        public GridLayer_Chess GridLayer { get; private set; }

        [ShowInInspector,ReadOnly]public List<Vector3Int> AvailableMoveList { get; private set; } = new List<Vector3Int>();

        [Button]
        public abstract List<Vector3Int> FindAvailableTiles();

        public void Build(GridLayer_Chess gridLayer)
        {
            GridLayer = gridLayer;
            PlaceOnTile(GridLayer.GetGridLocalPosition(transform.position));
            AvailableMoveList = new List<Vector3Int>();
        }

        public List<Vector3Int> GetAvailableMoves()
        {
            return AvailableMoveList;
        }

        public void UpdatePiece()
        {
            FindAvailableTiles();
        }
        public void PlaceOnTile(Vector3Int pos)
        {
            OccupiedTilePosition = pos;
        }
        
        public bool CanMoveTo(Vector3Int pos)
        {
            return AvailableMoveList.Contains(pos);
        }
        
        protected void TryToAddMove(Vector3Int pos)
        {
            AvailableMoveList.Add(pos);
        }
        
        public virtual void Move(Vector3Int targetPos)
        {
            var targetPosition = GridLayer.GetNode(targetPos).GetNodePosition();
            OccupiedTilePosition = targetPos;
            var finalDest = GridLayer.Grid.GetCellCenterLocal(targetPosition);
            finalDest.y = 0;
            transform.DOJump(finalDest,2,1, 0.5f).OnComplete(() =>
            {
                PlaceOnTile(targetPos);
                UpdatePiece();
            });
        }

        private void OnDrawGizmosSelected()
        {
            if (GridLayer == null)
            {
                return;
            }
            foreach (var vector3Int in AvailableMoveList)
            {
                var worldPos = GridLayer.Grid.GetCellCenterLocal(vector3Int);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(worldPos, 0.5f);
            }
        }
    }
}