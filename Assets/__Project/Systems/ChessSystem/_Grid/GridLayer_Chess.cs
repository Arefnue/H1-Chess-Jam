using System.Collections.Generic;
using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.ChessSystem._Utils;
using __Project.Systems.GridSystem;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class GridLayer_Chess : GridLayer<NTile_Chess>
    {
        [SerializeField] private List<ChessPieceBase> pieceList;
        [SerializeField] private GridSelector selector;


        #region Cache
        
        public ChessPieceBase SelectedPiece { get; private set; }

        public List<ChessPieceBase> PieceList { get; private set; } = new List<ChessPieceBase>();

        #endregion
        
        #region Setup
        public override void Build()
        {
            base.Build();
            foreach (var pieceBase in pieceList)
            {
                PieceList.Add(pieceBase);
            }
            foreach (var pieceBase in PieceList)
                pieceBase.Build(this);
            
            foreach (var pieceBase in PieceList)
                pieceBase.UpdatePiece();
        }

        public override void UpdateLayer()
        {
            base.UpdateLayer();
            foreach (var pieceBase in PieceList)
                pieceBase.UpdatePiece();
        }

        #endregion
        
        #region Methods
        public bool IsPositionOnGrid(Vector3Int nextPos)
        {
            if (!TileDict.ContainsKey(nextPos))
            {
                return false;
            }
            return true;
        }

        public bool IsPositionOccupied(Vector3Int nextPos, out ChessPieceBase piece)
        {
            piece = null;
            foreach (var pieceBase in PieceList)
            {
                if (nextPos == pieceBase.OccupiedTilePosition)
                {
                    piece = pieceBase;
                    return true;
                }
            }
            return false;
        }
        public bool IsPositionOccupied(Vector3Int nextPos)
        {
            return IsPositionOccupied(nextPos, out var piece);
        }

        public ChessPieceBase GetPiece(Vector3Int pos)
        {
            foreach (var pieceBase in PieceList)
            {
                if (pos == pieceBase.OccupiedTilePosition)
                {
                    return pieceBase;
                }
            }

            return null;
        }

        public void MoveSelectedPiece(Vector3Int target)
        {
            if (!SelectedPiece)
            {
                return;
            }
            
            SelectedPiece.Move(target);
            DeselectPiece();
        }
        public void SetSelectedPiece(ChessPieceBase piece)
        {
            SelectedPiece = piece;
            List<Vector3Int> moves = SelectedPiece.GetAvailableMoves();
            ShowSelection(moves,piece);
        }
        
        public void SelectPiece(Vector3Int pos)
        {
            var piece = GetPiece(pos);
            SelectPiece(piece);
        }
        
        public void SelectPiece(ChessPieceBase piece)
        {
            SetSelectedPiece(piece);    
        }
        
        private void RemovePiece(ChessPieceBase piece)
        {
            if (piece)
            {
                PieceList.Remove(piece);
                Destroy(piece.gameObject);
            }
        }

        
        public void DeselectPiece()
        {
            SelectedPiece = null;
            selector.ClearSelection();
        }
        
        public void ShowSelection(List<Vector3Int> selection,ChessPieceBase piece)
        {
            Dictionary<Vector3Int, GridSelector.SelectionEnum> dt = new Dictionary<Vector3Int, GridSelector.SelectionEnum>();
            for (int i = 0; i < selection.Count; i++)
            {
                var target = selection[i];
                var position = GetNode(target).GetNodePosition();
                bool isSquareFree = IsPositionOccupied(position,out var occupiedPiece);
                var targetEnum = GridSelector.SelectionEnum.Empty;
                if (!isSquareFree)
                {
                    targetEnum = GridSelector.SelectionEnum.Right;
                }
                dt.Add(position, targetEnum);
            }
            selector.ShowSelection(dt);
        }
        
        #endregion
        #region Editor
#if UNITY_EDITOR
        public override void FindTiles()
        {
            base.FindTiles();
            pieceList.Clear();
            var piece = transform.GetComponentsInChildren<ChessPieceBase>();
            pieceList.AddRange(piece);
        }
#endif
        #endregion
    }
}