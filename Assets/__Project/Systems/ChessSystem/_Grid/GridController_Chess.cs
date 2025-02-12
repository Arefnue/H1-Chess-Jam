using System;
using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.ChessSystem._Platforms;
using __Project.Systems.ChessSystem._Utils;
using __Project.Systems.GridSystem;
using __Project.Systems.LevelSystem;
using _NueCore.AudioSystem;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using UniRx;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class GridController_Chess : GridController<NTile_Chess>
    {
        [SerializeField] private LayerMask pieceLayer;

        private void Awake()
        {
            RBuss.OnEvent<ChessREvents.PieceMoveFinishedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActiveLayer.UpdateLayer();
            });
            
            RBuss.OnEvent<ChessREvents.PieceMoveStartedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActiveLayer.UpdateLayer();
            });

            RBuss.OnEvent<ColorPlatform.ColorsMatchedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActiveLayer.UpdateLayer();
                var chessLayer =ActiveLayer as GridLayer_Chess;
                if (chessLayer != null && chessLayer.PieceList.Count<=0)
                {
                    RBuss.Publish(new ChessREvents.AllPiecesFinishedREvent());
                }
            });
        }

        private void Update()
        {
            if (!LevelStatic.CurrentLevel)
            {
                return;
            }

            if (!ActiveLayer)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Cast();
            }
           
        }

        private void Cast()
        {
            var ray = CameraStatic.GetMouseRay();
            var chessLayer =ActiveLayer as GridLayer_Chess;
            if (chessLayer == null)
            {
                return;
            }

            if (UIHelper.IsMouseOverUI())
            {
                return;
            }
            if (CastHelper.TryRayCast(out var hit,ray,1000,pieceLayer))
            {
                var ht = hit.collider.attachedRigidbody;
               
                if (ht.TryGetComponent<ChessPieceBase>(out var piece))
                {
                   
                    chessLayer.DeselectPiece();
                    if (!piece.IgnoreInteraction)
                    {
                        chessLayer.SelectPiece(piece.OccupiedTilePosition);
                    }
                    return;
                }
              
                if (ht.TryGetComponent<SelectorHighlight>(out var highlight))
                {
                    chessLayer.MoveSelectedPiece(highlight.Position);
                }
               
            }

            if (chessLayer.SelectedPiece)
            {
                AudioStatic.PlayFx(DefaultAudioDataTypes.Deselect);
            }

            chessLayer.DeselectPiece();
        }
    }
}