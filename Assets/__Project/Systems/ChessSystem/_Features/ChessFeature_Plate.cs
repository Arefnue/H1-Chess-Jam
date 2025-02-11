
using System.Linq;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.ChessSystem._Pieces;
using _NueCore.Common.ReactiveUtils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Features
{
    public class ChessFeature_Plate : ChessFeatureBase
    {
        [SerializeField] private ChessColorEnum targetColor;
        [SerializeField] private Renderer rend;
        [SerializeField] private Animator animator;
        
        #region Cache
        public ChessColorEnum TargetColor => targetColor;
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }
        public ChessFeature_Door TargetDoor { get; private set; }
        public bool IsDoorOpened { get; private set; }
        #endregion
       
        public override void Build(GridLayer_Chess gridLayerChess)
        {
            base.Build(gridLayerChess);
            PlaceOnTile(GridLayerChess.GetWorldPos(transform.position));
            
            TargetDoor = GridLayerChess.FeatureList
                .OfType<ChessFeature_Door>()
                .FirstOrDefault(door => door.TargetColor == this.TargetColor);

            if (TargetDoor)
            {
                TargetDoor.SetDoor(this);
            }
            RBuss.OnEvent<ChessREvents.PieceMoveFinishedREvent>().Subscribe(ev =>
            {
              
                // if (GridLayerChess.IsPositionsMatched(ev.Piece.OccupiedTilePosition,OccupiedTilePosition))
                // {
                //     OpenDoor(ev.Piece);
                // }
                UpdateFeature();
            }).AddTo(gameObject);
        }

        public override void UpdateFeature()
        {
            base.UpdateFeature();
            if (GridLayerChess.IsPositionOccupied(TargetDoor.OccupiedTilePosition))
            {
                if (IsDoorOpened) return;
                TargetDoor.Open();
                IsDoorOpened = true;
                animator.SetBool("Status",IsDoorOpened);
            }
            else
            {
                if (!IsDoorOpened) return;
                IsDoorOpened = false;
                TargetDoor.Close();
                animator.SetBool("Status",IsDoorOpened);
            }
            
        }
        
        public void PlaceOnTile(Vector3Int pos)
        {
            OccupiedTilePosition = pos;
        }

        public override void RemoveFeature()
        {
            base.RemoveFeature();
            transform.DOScale(Vector3.zero,0.5f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}