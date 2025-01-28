using System.Collections.Generic;
using __Project.Systems.ChessSystem._Pieces;
using __Project.Systems.GridSystem;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Grid
{
    public class GridLayer_Chess : GridLayer<NTile_Chess>
    {
        [SerializeField] private List<ChessPieceBase> pieceList;


        public override void Build()
        {
            base.Build();
            foreach (var pieceBase in pieceList)
                pieceBase.Build(this);
        }

        public bool IsPositionValid(Vector3Int nextPos)
        {
            if (!TileDict.ContainsKey(nextPos))
            {
                return false;
            }
            return true;
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