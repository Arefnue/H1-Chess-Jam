using System;
using _NueCore.Common.Utility;
using UnityEngine;

namespace __Project.Systems.ChessSystem
{
    public class ChessController : MonoBehaviour
    {
        public int ActiveStepCount { get; private set; }
        public void Build()
        {

        }
        public bool IsAllStepsCompleted()
        {
            if (ActiveStepCount >= 3)
                return true;
            return false;
        }
    }
}