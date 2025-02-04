using System.Collections.Generic;
using System.Text;
using __Project.Systems.GridSystem._PathSubSystem;
using _NueCore.Common.NueLogger;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace __Project.Systems.GridSystem
{
    public abstract class GridLayer<TTile> : MonoBehaviour where TTile : NTileBase
    {
        [SerializeField,TabGroup("Settings")] private Grid grid;
        [SerializeField,TabGroup("Settings")] private GridLayerEnum gridLayerType;
        [SerializeField,TabGroup("Settings")] private Vector2 gridMin = new Vector2(0, 0);
        [SerializeField,TabGroup("Settings")] private Vector2 gridMax = new Vector2(10, 10);
        
        #region Cache
        [SerializeField,TabGroup("Debug"),ReadOnly] private List<NTileBase> allTileList = new List<NTileBase>();
        public GridLayerEnum GridLayerType => gridLayerType;
        public Grid Grid => grid;
        public int SizeX => (int)gridMax.x - (int)gridMin.x;
        [ShowInInspector,ReadOnly,TabGroup("Debug")]public Dictionary<Vector3Int,NodeGeneric<TTile>> TileDict { get; private set; } =
            new Dictionary<Vector3Int, NodeGeneric<TTile>>();
        #endregion

        #region Setup
        public virtual void Build()
        {
            TileDict.Clear();
            InitTiles();
            //FillEmptyTiles();
            Log();
        }
        private void InitTiles()
        {
            foreach (var tile in allTileList)
            {
                var cellPos = grid.LocalToCell(tile.transform.localPosition);
                tile.Build();
                tile.SetCellPosition(cellPos);
                // tile.SnapToCellWorld(grid.GetCellCenterWorld(cellPos));
                var tileNode = new NodeGeneric<TTile>(cellPos);
                tileNode.SetGridTile(tile as TTile);
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
                    var tileNode = new NodeGeneric<TTile>(cellPos);
                    tileNode.SetGridTile(null);
                    TileDict.Add(cellPos, tileNode);
                }
            }
        }
        #endregion

        #region Methods
        [Button,TabGroup("Editor")]
        public virtual void AddTile(TTile nTileBase)
        {
            var cellPos = grid.LocalToCell(nTileBase.transform.localPosition);
            nTileBase.Build();
            nTileBase.SetCellPosition(cellPos);
            var tileNode = new NodeGeneric<TTile>(cellPos);
            tileNode.SetGridTile(nTileBase);
            if (TileDict.ContainsKey(cellPos))
                TileDict[cellPos] = tileNode;
            else
                TileDict.Add(cellPos, tileNode);
        }
        [Button,TabGroup("Editor")]
        public virtual void RemoveTile(TTile nTileBase)
        {
            var cellPos = nTileBase.GetCellPosition();
            if (TileDict.ContainsKey(cellPos))
            {
                var tileNode = new NodeGeneric<TTile>(cellPos);
                tileNode.SetGridTile(null);
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

        #region Tile
        
        public NodeGeneric<TTile> GetNode(Vector3 worldPos)
        {
            var wp= grid.WorldToCell(worldPos);
            if (TileDict.ContainsKey(wp))
            {
                return TileDict[wp];
            }
            return null;
        }
        public NodeGeneric<TTile> GetNode(Vector3Int cellPos)
        {
            if (TileDict.ContainsKey(cellPos))
            {
                return TileDict[cellPos];
            }
            return null;
        }

        public Vector3Int GetGridLocalPosition(Vector3 localPos)
        {
            return grid.LocalToCell(localPos);
        }
        
        public Vector3Int GetGridWorldPosition(Vector3 worldPos)
        {
            return grid.WorldToCell(worldPos);
        }

        #endregion
        
        #region Editor
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (grid == null)
            {
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
            allTileList.Clear();
            var c = GetComponentsInChildren<NTileBase>();
            foreach (var tileBase in c)
                allTileList.Add(tileBase);
        }

        public void SetEditor()
        {
            FindTiles();
        }
#endif
        #endregion
    }
}