using System.Collections.Generic;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.GridSystem._PathSubSystem
{
    public static class PathHelper
    {
        #region Common
        private static List<BaseNode> GetNeighbors(BaseNode baseNode, Dictionary<Vector3Int, BaseNode> nodes)
        {
            var neighbors = new List<BaseNode>();
            var directions = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, -1, 0)
            };

            foreach (var direction in directions)
            {
                var neighborPos = baseNode.NodePosition + direction;
                if (nodes.ContainsKey(neighborPos))
                {
                    neighbors.Add(nodes[neighborPos]);
                }
            }

            return neighbors;
        }

        public static List<BaseNode> GetNeighbors(Vector3Int currentNodePosition)
        {
            var neighbors = new List<BaseNode>
            {
                new BaseNode(currentNodePosition + Vector3Int.up),
                new BaseNode(currentNodePosition + Vector3Int.down),
                new BaseNode(currentNodePosition + Vector3Int.left),
                new BaseNode(currentNodePosition + Vector3Int.right)
            };

            return neighbors;
        }
        public static Dictionary<Vector3Int, BaseNode> ConvertTileNodeToPathNoteDict(Dictionary<Vector3Int,BaseNodeTile> dict)
        {
            var pathNodeDict = new Dictionary<Vector3Int, BaseNode>();
            foreach (var tileNode in dict)
            {
                pathNodeDict.Add(tileNode.Key,tileNode.Value);
            }

            return pathNodeDict;
        }
        #endregion
        
        #region AStar
        public const int MOVE_STRAIGHT_COST = 10;
        public const int MOVE_DIAGONAL_COST = 14;
        public static bool TryGetPath(Vector3Int start, Vector3Int end, Dictionary<Vector3Int, BaseNode> nodes, out List<Vector3Int> path)
        {
            path = FindPathAStar(start, end,nodes);
            return path != null;
        }
        public static bool HasPath(Vector3Int start, Vector3Int end, Dictionary<Vector3Int, BaseNode> nodes)
        {
            return FindPathAStar(start, end,nodes) != null;
        }
        
        public static List<Vector3Int> FindPathAStar(Vector3Int start, Vector3Int end, Dictionary<Vector3Int, BaseNode> nodes)
        {
            var openList = new PriorityQueue<BaseNode>();
            var closedList = new HashSet<Vector3Int>();
            var startNode = nodes[start];
            var endNode = nodes[end];

            openList.Enqueue(startNode);

            while (openList.Count > 0)
            {
                var currentNode = openList.Dequeue();
                if (currentNode.GetNodePosition() == endNode.GetNodePosition())
                {
                    return GetFinalizedPath(currentNode, startNode);
                }
               
                closedList.Add(currentNode.GetNodePosition());

                var neighbours = GetNeighbors(currentNode, nodes);
                foreach (var neighbor in neighbours)
                {
                    if (closedList.Contains(neighbor.GetNodePosition()))
                        continue;
                    if (!neighbor.GetIsWalkable())
                    {
                        closedList.Add(neighbor.GetNodePosition());
                        continue;
                    }
                    var newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.ConnectedBaseNode = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Enqueue(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private static List<Vector3Int> GetFinalizedPath(BaseNode currentBaseNode, BaseNode startBaseNode)
        {
            var path = new List<Vector3Int>();
            var currentNodeNode = currentBaseNode;
            var breakCount = 100;
            while (currentNodeNode != startBaseNode)
            {
                path.Add(currentNodeNode.NodePosition);
                currentNodeNode = currentNodeNode.ConnectedBaseNode;
                breakCount--;
                if (breakCount <= 0)
                {
                    "Break Count Reached".NLog(Color.red);
                    break;
                }
            }
            path.Reverse();
            return path;
        }

        private static int GetDistance(BaseNode a, BaseNode b)
        {
            int dstX = Mathf.Abs(a.NodePosition.x - b.NodePosition.x);
            int dstY = Mathf.Abs(a.NodePosition.y - b.NodePosition.y);
            return dstX + dstY;
        }
        #endregion
    }
}