using System;
using System.Collections.Generic;
using UnityEngine;

namespace __Project.Systems.GridSystem._PathSubSystem
{
    [Serializable]
    public class BaseNode : IComparable<BaseNode>
    {
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;
        public Vector3Int NodePosition { get; set; }
        public BaseNode ConnectedBaseNode { get; set; }
        public virtual bool GetIsWalkable()
        {
            return true;
        }
        public BaseNode(Vector3Int currentNodeNodePosition)
        {
            NodePosition = currentNodeNodePosition;
        }
        public Vector3Int GetNodePosition()
        {
            return NodePosition;
        }

        public int CompareTo(BaseNode other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var gCostComparison = GCost.CompareTo(other.GCost);
            if (gCostComparison != 0) return gCostComparison;
            var hCostComparison = HCost.CompareTo(other.HCost);
            if (hCostComparison != 0) return hCostComparison;
            return Comparer<BaseNode>.Default.Compare(ConnectedBaseNode, other.ConnectedBaseNode);
        }
    }
}