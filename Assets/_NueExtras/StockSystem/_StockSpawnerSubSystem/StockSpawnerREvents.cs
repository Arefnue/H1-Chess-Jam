using System;
using _NueCore.Common.ReactiveUtils;
using UnityEngine;

namespace _NueExtras.StockSystem._StockSpawnerSubSystem
{
    public abstract class StockSpawnerREvents
    {
        public class SetDefaultSpawnTargetREvent : REvent
        {
            public StockTypes StockType { get; private set; }
            public Transform TargetRoot { get; private set; }
            public SetDefaultSpawnTargetREvent(StockTypes stockType, Transform targetRoot)
            {
                StockType = stockType;
                TargetRoot = targetRoot;
            }
        }
        
        public class SpawnStockREvent : REvent
        {
            public StockTypes StockType { get; private set; }
            public Action<StockSpawnObject> FinishedAction { get; private set; }

            public bool HasStartPos { get; private set; }
            public Vector3 StartPos { get; private set; }
            
            public bool Spawn3D { get; set; }
            public SpawnStockREvent(StockTypes stockType, Action<StockSpawnObject> finishedAction)
            {
                StockType = stockType;
                FinishedAction = finishedAction;
            }
            
            public SpawnStockREvent(StockTypes stockType,Vector3 startPos, Action<StockSpawnObject> finishedAction)
            {
                StockType = stockType;
                FinishedAction = finishedAction;
                StartPos = startPos;
                HasStartPos = true;
            }
        }
        
        public class DeSpawnStockREvent : REvent
        {
            public StockTypes StockType { get; private set; }
            public Action<StockSpawnObject> FinishedAction { get; private set; }
            
            public Transform TargetTransform { get; private set; }
            public bool Spawn3D { get; set; }
            public bool IsLastParticle { get; set; }
            public DeSpawnStockREvent(StockTypes stockType,Transform targetTransform, Action<StockSpawnObject> finishedAction)
            {
                StockType = stockType;
                FinishedAction = finishedAction;
                TargetTransform = targetTransform;
            }
        }
    }
}