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
    public class ChessFeature_Teleport : ChessFeatureBase
    {
        [SerializeField] private ChessColorEnum targetColor;
        [SerializeField] private Renderer rend;
        
        
        public ChessColorEnum TargetColor => targetColor;
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }
        public ChessFeature_Teleport MatchedTeleport { get; private set; }

        public override void Build(GridLayer_Chess gridLayerChess)
        {
            base.Build(gridLayerChess);
            PlaceOnTile(GridLayerChess.GetWorldPos(transform.position));
            
            MatchedTeleport = GridLayerChess.FeatureList
                .OfType<ChessFeature_Teleport>()
                .FirstOrDefault(teleport => teleport != this && teleport.TargetColor == this.TargetColor);
            
            RBuss.OnEvent<ChessREvents.PieceMoveFinishedREvent>().Subscribe(ev =>
            {
                UpdateFeature();
                if (ev.Piece.IsTeleported)
                    return;
                if (!CanTeleport())
                    return;
                if (GridLayerChess.IsPositionsMatched(ev.Piece.OccupiedTilePosition,OccupiedTilePosition))
                {
                    Teleport(ev.Piece);
                }
            }).AddTo(gameObject);
        }

        public override void UpdateFeature()
        {
            base.UpdateFeature();
            if (CanTeleport())
            {
                rend.material.DisableKeyword("GREYSCALE_ON");
            }
            else
            {
                rend.material.EnableKeyword("GREYSCALE_ON");
            }
        }

        public bool CanTeleport()
        {
            if (MatchedTeleport == null)
                return false;
            // if (GridLayerChess.IsPositionOccupied(OccupiedTilePosition))
            // {
            //     return false;
            // }
            //
            if (GridLayerChess.IsPositionOccupied(MatchedTeleport.OccupiedTilePosition))
            {
                return false;
            }
            return true;
        }

        private Sequence _sequence;
        public void Teleport(ChessPieceBase piece)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOPunchScale(new Vector3(0.2f,0,0.2f), 0.5f, 1, 0.2f));
            _sequence.Join(MatchedTeleport.transform.DOPunchScale(new Vector3(0.2f,0,0.2f), 0.5f, 1, 0.2f));
            _sequence.OnComplete(() =>
            {
                transform.localScale = Vector3.one;
                MatchedTeleport.transform.localScale = Vector3.one;
            });
            piece.Teleport(MatchedTeleport.OccupiedTilePosition);
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