using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.GridSystem
{
    public abstract class NTileBase : MonoBehaviour
    {
       
        public Vector3Int CellPos { get; private set; }
        public virtual void Build()
        {
        }
        public void SetCellPosition(Vector3Int cellPosition)
        {
            CellPos = cellPosition;
        }

        public virtual bool IsWalkable()
        {
            "Base".NLog();
            return true;
        }
        public Vector3Int GetCellPosition()
        {
            return CellPos;
        }
    }
}