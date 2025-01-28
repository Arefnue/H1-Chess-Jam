using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.GridSystem
{
    public abstract class GridController<TTile> : MonoBehaviour where TTile : NTileBase
    {
        [SerializeField] private List<GridLayer<TTile>> gridLayerList = new List<GridLayer<TTile>>();
        [SerializeField] private Transform moveRoot;

        #region Cache

        public GridLayer<TTile> ActiveLayer { get; private set; }
        public void SetActiveLayer(int index)
        {
            ActiveLayer = gridLayerList[index];
        }

        #endregion

        #region Setup

        public void Build()
        {
            foreach (var gridLayer in gridLayerList)
                gridLayer.Build();
        }
        #endregion

        #region Async
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
        #endregion
        
        #region Editor
#if UNITY_EDITOR
        [Button("Find Grid Layers"),HideInPlayMode]
        public void FindGridLayersEditor(bool apply)
        {
            if (!apply)
            {
                return;
            }
            gridLayerList.Clear();
            var gridLayers = GetComponentsInChildren<GridLayer<TTile>>();
            foreach (var gridLayer in gridLayers)
            {
                gridLayerList.Add(gridLayer);
                gridLayer.SetEditor();
            }
        }
#endif
        #endregion
    }
}