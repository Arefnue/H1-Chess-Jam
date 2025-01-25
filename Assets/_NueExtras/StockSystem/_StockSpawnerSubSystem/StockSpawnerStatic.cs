using System;
using _NueCore.Common.ReactiveUtils;
using DG.Tweening;
using UnityEngine;

namespace _NueExtras.StockSystem._StockSpawnerSubSystem
{
    public static class StockSpawnerStatic
    {
        #region Cache
        private static bool _isSfxPlaying;
        private static Tween _delayTween;
        private static bool _isInit;
        #endregion

        #region Common Methods
        public static void SetStockMoveRoot(StockTypes stockType, Transform targetRoot)
        {
            RBuss.Publish(new StockSpawnerREvents.SetDefaultSpawnTargetREvent(stockType,targetRoot));
        }
        private static void PlaySfx()
        {
            if (_isSfxPlaying) return;
            _isSfxPlaying = true;
            _delayTween?.Kill();
            _delayTween = DOVirtual.DelayedCall(0.1f, () =>
            {
                _isSfxPlaying = false;
            },false);
        }
        #endregion

        #region SpawnUI
        public static void SpawnStock(StockTypes stockType, Action<StockSpawnObject> finishedAction)
        {
            PlaySfx();
            var rEvent = new StockSpawnerREvents.SpawnStockREvent(stockType, finishedAction)
            {
                Spawn3D = false
            };
            
            RBuss.Publish(rEvent);
        }
        
        public static void SpawnStock(StockTypes stockType,Vector3 startPos, Action<StockSpawnObject> finishedAction)
        {
            PlaySfx();
            var rEvent = new StockSpawnerREvents.SpawnStockREvent(stockType, startPos, finishedAction)
            {
                Spawn3D = false
            };
            RBuss.Publish(rEvent);
        }
        
        public static void DeSpawnStock(StockTypes stockType,Transform targetRoot, Action<StockSpawnObject> finishedAction,bool isLastParticle = false)
        {
            PlaySfx();
            var rEvent = new StockSpawnerREvents.DeSpawnStockREvent(stockType, targetRoot, finishedAction)
            {
                Spawn3D = false,
                IsLastParticle = isLastParticle
            };
            
            RBuss.Publish(rEvent);
        }
        #endregion

        #region Spawn3D
        public static void SpawnStock3D(StockTypes stockType, Action<StockSpawnObject> finishedAction)
        {
            PlaySfx();
            var rEvent = new StockSpawnerREvents.SpawnStockREvent(stockType, finishedAction)
            {
                Spawn3D = true
            };
            
            RBuss.Publish(rEvent);
        }
        public static void SpawnStock3D(StockTypes stockType,Vector3 startPos, Action<StockSpawnObject> finishedAction)
        {
            PlaySfx();
            var rEvent = new StockSpawnerREvents.SpawnStockREvent(stockType, startPos, finishedAction)
            {
                Spawn3D = true
            };
            RBuss.Publish(rEvent);
        }
        public static void DeSpawnStock3D(StockTypes stockType,Transform targetRoot, Action<StockSpawnObject> finishedAction)
        {
            PlaySfx();
            var rEvent = new StockSpawnerREvents.DeSpawnStockREvent(stockType, targetRoot, finishedAction)
            {
                Spawn3D = true
            };
            
            RBuss.Publish(rEvent);
        }
        #endregion
        
    }
}