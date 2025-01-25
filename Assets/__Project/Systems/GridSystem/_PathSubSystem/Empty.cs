using System;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.GridSystem._PathSubSystem
{
    [Serializable]
    public class Empty : BaseNodeTile
    {
        public Empty(Vector3Int currentNodeNodePosition) : base(currentNodeNodePosition)
        {
        }


        public override bool GetIsWalkable()
        {
            return true;
        }

        public override string GetNodeName()
        {
            return "EMPTY";
        }
    }
}