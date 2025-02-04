using System;
using System.Collections.Generic;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.GridSystem;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using DG.Tweening;
using HighlightPlus;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public abstract class ChessPieceBase : MonoBehaviour
    {
        [SerializeField] private ChessColorEnum colorEnum;
        [SerializeField] public HighlightEffect highlightEffect;
        
        #region Cache
        public GridLayer_Chess GridLayer { get; private set; }
        public ChessColorEnum ColorEnum => colorEnum;
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }
        [ShowInInspector,ReadOnly]public List<Vector3Int> AvailableMoveList { get; private set; } = new List<Vector3Int>();

        public bool IgnoreInteraction { get; set; }
        #endregion

        #region Setup
        public void Build(GridLayer_Chess gridLayer)
        {
            GridLayer = gridLayer;
            var targetPosition = GridLayer.GetNode(transform.position).GetNodePosition();
            PlaceOnTile(targetPosition);
            AvailableMoveList = new List<Vector3Int>();
        }
        #endregion

        #region Methods

        public void OnSelected()
        {
            SetHighlight(true);
        }
        public void OnDeselected()
        {
            SetHighlight(false);
        }
        public void SetHighlight(bool status)
        {
            if (!highlightEffect)
            {
                return;
            }

            highlightEffect.iconFX = status;
            highlightEffect.innerGlow = status ? 0.25f : 0f;
        }
        public void SetColor(ChessColorEnum color)
        {
            colorEnum = color;
        }
        
        [Button]
        public abstract List<Vector3Int> FindAvailableTiles();
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
            OccupiedTilePosition = targetPos;
         
            RBuss.Publish(new ChessREvents.PieceMoveStartedREvent(this));
         
            MoveTween(targetPos).OnComplete(() =>
            {
                PlaceOnTile(targetPos);
                UpdatePiece();
                RBuss.Publish(new ChessREvents.PieceMoveFinishedREvent(this));
            });
        }

        public virtual Tween MoveTween(Vector3Int targetPos)
        {
            var finalDest = GridLayer.Grid.GetCellCenterLocal(targetPos);
            finalDest.y = 0;
            return transform.DOMove(finalDest,0.5f);
        }
        
        public virtual void Teleport(Vector3Int targetPos)
        {
            var targetPosition = GridLayer.GetNode(targetPos).GetNodePosition();
            OccupiedTilePosition = targetPos;
            var finalDest = GridLayer.Grid.GetCellCenterLocal(targetPosition);
            finalDest.y = 0;
            RBuss.Publish(new ChessREvents.PieceMoveStartedREvent(this));
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                transform.position = finalDest;
            }));
            seq.Append(transform.DOScale(Vector3.one, 0.25f));
            seq.OnComplete(() =>
            {
                PlaceOnTile(targetPos);
                UpdatePiece();
                RBuss.Publish(new ChessREvents.PieceMoveFinishedREvent(this));
            });
        }

        
        #endregion
        
        #region Editor
#if UNITY_EDITOR
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
#endif
        #endregion
    }
}