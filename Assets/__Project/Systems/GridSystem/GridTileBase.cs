using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.GridSystem
{
    public abstract class GridTileBase : MonoBehaviour
    {
        public GridLayer GridLayer { get; private set; }
        public virtual void Build(GridLayer gridLayer)
        {
            GridLayer = gridLayer;
        }
        public Vector3Int CellPos { get; private set; }
        public void SetCellPosition(Vector3Int cellPosition)
        {
            CellPos = cellPosition;
        }
        
        public void SnapToCellLocal(Vector3 cellCenter)
        {
            transform.localPosition = cellCenter;
        }
        
        public void SnapToCellWorld(Vector3 cellCenter)
        {
            transform.position = cellCenter;
        }

        public Vector3Int GetCellPosition()
        {
            return CellPos;
        }
    }
}