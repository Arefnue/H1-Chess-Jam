using System;
using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.ChessSystem._Utils;
using __Project.Systems.GridSystem;
using __Project.Systems.LevelSystem;
using _NueCore.Common.NueLogger;
using _NueCore.Common.Utility;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class GridController_Chess : GridController<NTile_Chess>
    {
        [SerializeField] private LayerMask pieceLayer;
        [SerializeField] private LayerMask highlightLayer;
        
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
                var ray = CameraStatic.GetMouseRay();
                "Click".NLog();
                if (CastHelper.TryRayCast(out var hit,ray,1000,pieceLayer))
                {
                    "X".NLog();
                    if (hit.collider.attachedRigidbody.gameObject.TryGetComponent<ChessPieceBase>(out var piece))
                    {
                        "A".NLog();
                        var chessLayer =ActiveLayer as GridLayer_Chess;
                        if (chessLayer)
                        {
                            "Chess selected".NLog();
                            chessLayer.DeselectPiece();
                            chessLayer.SelectPiece(piece.OccupiedTilePosition);
                        }
                    }
                }

                if (CastHelper.TryRayCast(out var hit2,ray,1000,highlightLayer))
                {
                    if (hit2.collider.attachedRigidbody.gameObject.TryGetComponent<SelectorHighlight>(out var highlight))
                    {
                        var chessLayer =ActiveLayer as GridLayer_Chess;
                        if (chessLayer)
                        {
                            "Highlight selected".NLog();
                            chessLayer.MoveSelectedPiece(highlight.Position);
                        }
                    }
                }
            }
           
        }
    }
}