using System.Collections.Generic;
using System.Text;
using __Project.Systems.GridSystem._PathSubSystem;
using _NueCore.Common.NueLogger;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace __Project.Systems.GridSystem
{
    public class GridLayer : MonoBehaviour
    {
        [SerializeField,TabGroup("Settings")] private Grid grid;
        [SerializeField,TabGroup("Settings")] private GridLayerEnum gridLayerType;
        [SerializeField,TabGroup("Settings")] private Vector2 gridMin = new Vector2(0, 0);
        [SerializeField,TabGroup("Settings")] private Vector2 gridMax = new Vector2(10, 10);
        
        #region Cache
        [SerializeField,TabGroup("Debug"),ReadOnly] private List<GridTileBase> tileList = new List<GridTileBase>();
        public GridLayerEnum GridLayerType => gridLayerType;
        public Grid Grid => grid;
        [ShowInInspector,ReadOnly,TabGroup("Debug")]public Dictionary<Vector3Int,BaseNodeTile> TileDict { get; private set; } =
            new Dictionary<Vector3Int, BaseNodeTile>();
        #endregion

        #region Setup
        public virtual void Build()
        {
            TileDict.Clear();
            InitTiles();
            FillEmptyTiles();
            Log();
        }
        private void InitTiles()
        {
            foreach (var tile in tileList)
            {
                var cellPos = grid.LocalToCell(tile.transform.localPosition);
                tile.Build(this);
                tile.SetCellPosition(cellPos);
                // tile.SnapToCellWorld(grid.GetCellCenterWorld(cellPos));
                var tileNode = new BaseNodeTile(cellPos);
                tileNode.SetGridTile(tile);
                TileDict.Add(cellPos, tileNode);
            }
        }
        private void FillEmptyTiles()
        {
            for (int x = (int)gridMin.x; x <= (int)gridMax.x; x++)
            {
                for (int y = (int)gridMin.y; y <= (int)gridMax.y; y++)
                {
                    var cellPos = new Vector3Int(x, y, 0);
                    if (TileDict.ContainsKey(cellPos))
                        continue;
                    var tileNode = new Empty(cellPos);
                    TileDict.Add(cellPos, tileNode);
                }
            }
        }
        #endregion

        #region Methods
        public virtual void AddTile(GridTileBase tile)
        {
            var cellPos = grid.LocalToCell(tile.transform.localPosition);
            tile.Build(this);
            tile.SetCellPosition(cellPos);
            var tileNode = new BaseNodeTile(cellPos);
            tileNode.SetGridTile(tile);
            if (TileDict.ContainsKey(cellPos))
                TileDict[cellPos] = tileNode;
            else
                TileDict.Add(cellPos, tileNode);
        }
        
        public virtual void RemoveTile(GridTileBase tile)
        {
            var cellPos = tile.GetCellPosition();
            if (TileDict.ContainsKey(cellPos))
            {
                var tileNode = new Empty(cellPos);
                TileDict[cellPos] = tileNode;
            }
        }
        public virtual void ActivateLayer()
        {
            
        }

        public virtual void UpdateLayer()
        {
            
        }
        private void Log()
        {
            var str = new StringBuilder();
            str.AppendLine($"GridLayer: {name}");
            foreach (var keyValue in TileDict)
            {
                var ct =$"Key: {keyValue.Key} Value: {keyValue.Value.GetNodeName()}";
                str.Append(ct);
                str.AppendLine();
            }
            str.NLog(Color.white);
        }


        #endregion
        
        #region Pathfinding
        public List<GridTileBase> GetNeighbours(GridTileBase tile)
        {
            //TODO Cache neighbours into tiles
            var neighbours = new List<GridTileBase>();
            var directions = new[]
            {
                new Vector3Int(-1, 0, 0), // left
                new Vector3Int(1, 0, 0), // right
                new Vector3Int(0, 1, 0), // up
                new Vector3Int(0, -1, 0) // down
            };

            foreach (var direction in directions)
            {
                var neighbourTile = GetTile(tile.GetCellPosition() + direction);
                if (neighbourTile != null)
                {
                    neighbours.Add(neighbourTile);
                }
            }

            return neighbours;
        }
        public List<GridTileBase> GetNeighbours(Vector3Int cellPos)
        {
            return GetNeighbours(GetTile(cellPos));
        }
        public List<GridTileBase> GetNeighbours(Vector3 worldPos)
        {
            return GetNeighbours(GetTile(worldPos));
        }
        public List<GridTileBase> GetNeighbours(int x, int y)
        {
            return GetNeighbours(new Vector3Int(x, y, 0));
        }
        #endregion

        #region Tile

        public BaseNodeTile GetNode(Vector3Int cellPos)
        {
            if (TileDict.ContainsKey(cellPos))
            {
                return TileDict[cellPos];
            }
            return null;
        }
        
        public BaseNodeTile GetNode(Vector3 worldPos)
        {
            var cellPos = grid.WorldToCell(worldPos);
            return GetNode(cellPos);
        }
        
        public GridTileBase GetTile(Vector3Int cellPos)
        {
            if (TileDict.ContainsKey(cellPos))
            {
                return TileDict[cellPos].GridTile;
            }
            return null;
        }
        
        public GridTileBase GetTile(Vector3 worldPos)
        {
            var cellPos = grid.WorldToCell(worldPos);
            return GetTile(cellPos);
        }
        
        public void SetTile(Vector3Int cellPos, GridTileBase tile)
        {
            if (TileDict.ContainsKey(cellPos))
            {
                TileDict[cellPos].SetGridTile(tile);
            }
        }
        
        public void SetTile(Vector3 worldPos, GridTileBase tile)
        {
            var cellPos = grid.WorldToCell(worldPos);
            SetTile(cellPos, tile);
        }
        
        public void RemoveTile(Vector3Int cellPos)
        {
            if (TileDict.ContainsKey(cellPos))
            {
                TileDict[cellPos] = null;
            }
        }
        
        public void RemoveTile(Vector3 worldPos)
        {
            var cellPos = grid.WorldToCell(worldPos);
            RemoveTile(cellPos);
        }
        #endregion
        
        #region Editor
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (grid == null)
            {
                FindGrid();
                //return;
            }
            Gizmos.color = Color.magenta;
            for (int x = (int)gridMin.x; x <= (int)gridMax.x; x++)
            {
                for (int y = (int)gridMin.y; y <= (int)gridMax.y; y++)
                {
                    var cellPos = new Vector3Int(x, y, 0);
                    
                    var cellCenter = grid.GetCellCenterLocal(cellPos);
                    Gizmos.DrawWireCube(grid.transform.position +cellCenter, grid.cellSize);
                    Handles.Label(
                        grid.transform.position +cellCenter,
                        new Vector2Int(cellPos.x, cellPos.y).ToString(),
                        new GUIStyle
                        {
                            fontSize = 12,
                            normal = new GUIStyleState
                            {
                                textColor = Color.white
                            }
                        }
                    );
                }
            }
        }

        [Button,TabGroup("Editor")]
        public virtual void FindTiles()
        {
            tileList.Clear();
            var c = GetComponentsInChildren<GridTileBase>();
            foreach (var tileBase in c)
                tileList.Add(tileBase);
        }

        private void FindGrid()
        {
            grid = GetComponentInChildren<Grid>();
        }

        public void SetEditor()
        {
            FindGrid();
            FindTiles();
        }
#endif
        #endregion
    }
}