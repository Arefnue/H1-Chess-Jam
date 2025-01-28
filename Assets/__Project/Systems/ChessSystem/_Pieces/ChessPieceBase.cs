using __Project.Systems.GridSystem;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public abstract class ChessPieceBase : MonoBehaviour
    {
        public Vector3Int CurrentPosition { get; private set; }
        protected ChessTile CurrentTile { get; private set; }

        public void PlaceOnTile(ChessTile tile)
        {
            CurrentTile = tile;
            CurrentPosition = tile.GetCellPosition();
            transform.position = tile.transform.position;
        }

        public void ChangePosition(Vector3Int pos)
        {
            CurrentPosition = pos;
        }

        public abstract void Move(Vector3Int newPosition);
    }
}