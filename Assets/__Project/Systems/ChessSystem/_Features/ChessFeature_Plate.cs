
using System.Collections.Generic;
using System.Linq;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.ChessSystem._Pieces;
using _NueCore.AudioSystem;
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
        [SerializeField] private AudioData openSfx;
        [SerializeField] private AudioData closeSfx;
        
        
        #region Cache
        public ChessColorEnum TargetColor => targetColor;
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }
        public List<ChessFeature_Door> TargetDoorList { get; private set; } = new List<ChessFeature_Door>();
        public bool IsDoorOpened { get; private set; }
        #endregion
       
        public override void Build(GridLayer_Chess gridLayerChess)
        {
            base.Build(gridLayerChess);
            PlaceOnTile(GridLayerChess.GetWorldPos(transform.position));

            var tempList = GridLayerChess.FeatureList
                .OfType<ChessFeature_Door>().ToList();
            TargetDoorList.Clear();
            foreach (var door in tempList)
            {
                if (door.TargetColor != TargetColor)
                    continue;
                TargetDoorList.Add(door);
            }

            DOVirtual.DelayedCall(0.02f, () =>
            {
                foreach (var door in TargetDoorList)
                {
                    door.SetDoor(this);
                }
            }, false);
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
                openSfx?.Play();
                foreach (var door in TargetDoorList)
                {
                    door.Open();
                }
                IsDoorOpened = true;
                animator.SetTrigger("Press");
            }
            else
            {
                if (!IsDoorOpened) return;
                closeSfx?.Play();

                IsDoorOpened = false;
                foreach (var door in TargetDoorList)
                {
                    door.Close();
                }
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