using System;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.ChessSystem._Pieces;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Platforms
{
    public class ColorPlatform : MonoBehaviour
    {
        [SerializeField] private ChessColorEnum targetColor;
        [SerializeField] private int count = 3;
        [SerializeField] private TMP_Text countText;
        
        [ShowInInspector,ReadOnly]public Vector3Int OccupiedTilePosition { get; private set; }

        #region Cache

        public ChessColorEnum TargetColor => targetColor;
        public GridLayer_Chess GridLayerChess { get; private set; }


        #endregion

        #region Setup

        public void Build(GridLayer_Chess gridLayerChess)
        {
            GridLayerChess = gridLayerChess;
            OccupiedTilePosition =GridLayerChess.GetNode(transform.position).GetNodePosition();
            RegisterREvents();
            UpdateCounter();
        }

        #endregion

        #region Reactive
        private void RegisterREvents()
        {
            RBuss.OnEvent<ChessREvents.PieceMoveFinishedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                var piece = ev.Piece;
                if (piece.OccupiedTilePosition == OccupiedTilePosition)
                {
                    CheckCollect(piece);
                }
            });
        }
        

        #endregion

        #region Methods

        private void UpdateCounter()
        {
            countText.text = count.ToString();
        }

        private bool _isProcessing;
        public void CheckCollect(ChessPieceBase piece)
        {
            if (_isProcessing)
            {
                return;
            }
            if (count<=0)
            {
                return;
            }
            if (piece.ColorEnum != TargetColor)
            {
                return;
            }

            _isProcessing = true;
            piece.IgnoreInteraction = true;
            GridLayerChess.RemovePiece(piece);
            piece.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _isProcessing = false;

                count--;
                UpdateCounter();
                RBuss.Publish(new ColorsMatchedREvent(this,piece,TargetColor));
                GridLayerChess.DestroyPiece(piece);
                if (count<=0)
                {
                    Destroy(gameObject);
                }
            });
            
            UpdateCounter();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            countText.SetText(count.ToString());
        }
#endif
        #endregion
        
        public class ColorsMatchedREvent : REvent
        {
            public ColorPlatform Platform { get; private set; }
            public ChessPieceBase Piece { get; private set; }
            public ChessColorEnum Color { get; private set; }
            public ColorsMatchedREvent(ColorPlatform platform,ChessPieceBase piece,ChessColorEnum color)
            {
                Platform = platform;
                Piece = piece;
                Color = color;
            }
        }
        
    }
}