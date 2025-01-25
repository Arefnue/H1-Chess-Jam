using System;
using System.Collections.Generic;
using __Project.Systems.LevelSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.GridSystem
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private List<GridLayer> gridLayerList = new List<GridLayer>();
        [SerializeField] private Transform moveRoot;
    
        public GridLayer ActiveLayer { get; private set; }
        public LevelBase Level { get; private set; }

        public void SetActiveLayer(int index)
        {
            ActiveLayer = gridLayerList[index];
        }
        public void Build(LevelBase level)
        {
            Level = level;
            foreach (var gridLayer in gridLayerList)
                gridLayer.Build();
        }
        
        public async UniTask<bool> MoveGridLayersAsync(int index)
        {
            if (index<=0)
                return true;
            if (index>=gridLayerList.Count)
                return true;
            
            var previousGrid = gridLayerList[index-1];
            var currentGrid = gridLayerList[index];
            var distanceToMove =  currentGrid.transform.position.x - previousGrid.transform.position.x;
            var targetPos = moveRoot.transform.position + new Vector3(-distanceToMove,0,0);
            await moveRoot.DOMoveX(targetPos.x, 2f).SetEase(Ease.InSine);
            return true;
        }

#if UNITY_EDITOR
        [Button("Find Grid Layers"),HideInPlayMode]
        public void FindGridLayersEditor(bool apply)
        {
            if (!apply)
            {
                return;
            }
            gridLayerList.Clear();
            var gridLayers = GetComponentsInChildren<GridLayer>();
            foreach (var gridLayer in gridLayers)
            {
                gridLayerList.Add(gridLayer);
                gridLayer.SetEditor();
            }
        }
#endif
    }
}