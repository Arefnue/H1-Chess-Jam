
using System.Linq;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.ChessSystem._Pieces;
using _NueCore.Common.NueLogger;
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
                
                UpdateFeature();
            }).AddTo(gameObject);
        }

        public override void UpdateFeature()
        {
            base.UpdateFeature();
            var piece =GridLayerChess.GetPiece(OccupiedTilePosition);
            if (piece)
            {
                if (IsDoorOpened) return;
                "Opened".NLog();
                TargetDoor.Open();
                IsDoorOpened = true;
                animator.SetTrigger("Press");
            }
            else
            {
                if (!IsDoorOpened) return;
                "Closed".NLog();
                IsDoorOpened = false;
                TargetDoor.Close();
                animator.SetTrigger("UnPress");
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