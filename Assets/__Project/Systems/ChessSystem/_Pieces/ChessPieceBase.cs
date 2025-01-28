using System.Collections.Generic;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.GridSystem;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public abstract class ChessPieceBase : MonoBehaviour
    {
        public Vector3Int OccupiedTilePosition { get; private set; }
        public GridLayer_Chess GridLayer { get; private set; }

        public List<Vector3Int> AvailableMoveList { get; private set; } = new List<Vector3Int>();

        public abstract List<Vector3Int> FindAvailableTiles();

        public void Build()
        {
            AvailableMoveList = new List<Vector3Int>();
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
            transform.DOMove(targetPosition, 0.5f);
        }
    }
}