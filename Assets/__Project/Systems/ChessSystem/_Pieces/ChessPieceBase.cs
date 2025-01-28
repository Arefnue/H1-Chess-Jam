using __Project.Systems.GridSystem;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public abstract class ChessPieceBase : MonoBehaviour
    {
        public Vector3Int CurrentPosition { get; private set; }
        protected NTile_Chess Current { get; private set; }

        public void PlaceOnTile(NTile_Chess nTileBase)
        {
            Current = nTileBase;
            CurrentPosition = nTileBase.GetCellPosition();
            transform.position = nTileBase.transform.position;
        }

        public void ChangePosition(Vector3Int pos)
        {
            CurrentPosition = pos;
        }

        public abstract void Move(Vector3Int newPosition);
    }
}