using System;
using __Project.Systems.ChessSystem._Grid;
using __Project.Systems.ChessSystem._Platforms;
using __Project.Systems.LevelSystem;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using UniRx;
using UnityEngine;

namespace __Project.Systems.ChessSystem
{
    public class ChessController : MonoBehaviour
    {
        
        public int ActiveStepCount { get; private set; }
        public void Build()
        {
            
        }

        public void LevelUp()
        {
            ActiveStepCount++;
        }
        public bool IsAllStepsCompleted()
        {
            if (ActiveStepCount >= 1)
                return true;
            return false;
        }
    }
}