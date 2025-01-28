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
        

        public override void Build()
        {
            base.Build();
            foreach (var pieceBase in pieceList)
                pieceBase.Build(this);
            
            foreach (var pieceBase in pieceList)
                pieceBase.UpdatePiece();
        }

        public bool IsPositionOnGrid(Vector3Int nextPos)
        {
            if (!TileDict.ContainsKey(nextPos))
            {
                return false;
            }
            return true;
        }

        public bool IsPositionOccupied(Vector3Int nextPos)
        {
            foreach (var pieceBase in pieceList)
            {
                if (nextPos == pieceBase.OccupiedTilePosition)
                {
                    return true;
                }
            }

            return false;
        }
        
        private void ShowSelection(List<Vector3Int> selection)
        {
            Dictionary<Vector3, GridSelector.SelectionEnum> dt = new Dictionary<Vector3, GridSelector.SelectionEnum>();
            for (int i = 0; i < selection.Count; i++)
            {
                var target = selection[i];
                var position = GetNode(target).GetNodePosition();
                bool isSquareFree = IsPositionOccupied(position);
                var targetEnum = GridSelector.SelectionEnum.Empty;
                if (!isSquareFree)
                {
                    targetEnum = GridSelector.SelectionEnum.Right;
                }
                dt.Add(position, targetEnum);
            }
            selector.ShowSelection(dt);
        }


#if UNITY_EDITOR

        public override void FindTiles()
        {
            base.FindTiles();
            pieceList.Clear();
            var piece = transform.GetComponentsInChildren<ChessPieceBase>();
            pieceList.AddRange(piece);
        }
#endif
    }
}