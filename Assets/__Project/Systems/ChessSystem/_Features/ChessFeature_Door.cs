using __Project.Systems.ChessSystem._Grid;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Features
{
    public class ChessFeature_Door : ChessFeatureBase
    {
        [SerializeField] private ChessColorEnum targetColor;
        [SerializeField] private Animator animator;
        
       
        #region Cache
        public ChessColorEnum TargetColor => targetColor;
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }
        public ChessFeature_Plate TargetPlate { get; private set; }

        public NTile_Chess OccupiedTile { get; private set; }

        #endregion
        public void SetDoor(ChessFeature_Plate plate)
        {
            TargetPlate = plate;
            PlaceOnTile(GridLayerChess.GetWorldPos(transform.position));
            var a =GridLayerChess.GetNode(OccupiedTilePosition);
            if (a == null)
            {
                "Null tilebase".NLog();
                return;
            }

            if (!a.NTileBase)
            {
                "Null tile".NLog();
                return;
            }

            OccupiedTile = a.NTileBase;
            OccupiedTile.BlockWalk = true;
            RBuss.OnEvent<ChessREvents.PieceMoveStartedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (!IsOpened && !OccupiedTile.BlockWalk)
                {
                    if (!GridLayerChess.IsPositionOccupied(OccupiedTilePosition))
                    {
                        animator.SetTrigger("UnPress");
                        OccupiedTile.BlockWalk = true;
                    }
                }
            });
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

        public bool IsOpened { get; private set; }
        public void Open()
        {
            IsOpened = true;
            OccupiedTile.BlockWalk = false;
            animator.SetTrigger("Press");
        }

        public void Close()
        {
            IsOpened = false;
            if (!GridLayerChess.IsPositionOccupied(OccupiedTilePosition))
            {
                animator.SetTrigger("UnPress");
                OccupiedTile.BlockWalk = true;
            }
           
        }
    }
}