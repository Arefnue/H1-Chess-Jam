using System.Collections.Generic;
using __Project.Systems.ChessSystem._Features;
using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.ChessSystem._Platforms;
using __Project.Systems.ChessSystem._Utils;
using __Project.Systems.GridSystem;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class GridLayer_Chess : GridLayer<NTile_Chess>
    {
        [SerializeField] private List<ChessPieceBase> pieceList;
        [SerializeField] private List<ColorPlatform> platformList;
        [SerializeField] private GridSelector selector;
        [SerializeField] private List<ChessFeatureBase> featureList;
        

        #region Cache
        
        public ChessPieceBase SelectedPiece { get; private set; }

        public List<ChessPieceBase> PieceList { get; private set; } = new List<ChessPieceBase>();
        public List<ColorPlatform> PlatformList { get; private set; } = new List<ColorPlatform>();
        public List<ChessFeatureBase> FeatureList { get; private set; } = new List<ChessFeatureBase>();

        #endregion
        
        #region Setup
        public override void Build()
        {
            base.Build();
            foreach (var platform in platformList)
                PlatformList.Add(platform);
            foreach (var pieceBase in pieceList)
                PieceList.Add(pieceBase);
            foreach (var feature in featureList)
                FeatureList.Add(feature);
            
            foreach (var pieceBase in PieceList)
                pieceBase.Build(this);
            
            foreach (var pieceBase in PieceList)
                pieceBase.UpdatePiece();
            
            foreach (var platform in PlatformList)
                platform.Build(this);
            
            foreach (var feature in FeatureList)
                feature.Build(this);

            foreach (var feature in FeatureList)
                feature.UpdateFeature();

        }

        public override void UpdateLayer()
        {
            base.UpdateLayer();
            foreach (var pieceBase in PieceList)
                pieceBase.UpdatePiece();
            
            foreach (var feature in FeatureList)
                feature.UpdateFeature();
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

        public ColorPlatform GetPlatform(Vector3Int pos)
        {
            foreach (var platform in PlatformList)
            {
                if (pos == platform.OccupiedTilePosition)
                {
                    return platform;
                }
            }

            return null;
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
        
        public void RemovePiece(ChessPieceBase piece)
        {
            if (piece)
            {
                PieceList.Remove(piece);
                
            }
        }

        public void DestroyPiece(ChessPieceBase piece)
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
                var targetPlatform =GetPlatform(position);
                if (targetPlatform)
                {
                    targetEnum = piece.ColorEnum == targetPlatform.TargetColor
                        ? GridSelector.SelectionEnum.Right
                        : GridSelector.SelectionEnum.Wrong;
                    
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
            var platform = transform.GetComponentsInChildren<ColorPlatform>();
            platformList.AddRange(platform);
            var feature = transform.GetComponentsInChildren<ChessFeatureBase>();
            featureList.AddRange(feature);
        }
#endif
        #endregion
    }
}