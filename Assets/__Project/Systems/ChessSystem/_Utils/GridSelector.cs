using System.Collections.Generic;
using _NueCore.Common.KeyValueDict;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Utils
{
    public class GridSelector : MonoBehaviour
    {
        [SerializeField] private KeyValueDict<SelectionEnum,Material> materialDict = new KeyValueDict<SelectionEnum, Material>();
        
        [SerializeField] private SelectorHighlight selectorPrefab;
        
        private List<SelectorHighlight> SpawnedSelectors { get; set; } = new List<SelectorHighlight>();

        public enum SelectionEnum
        {
            Empty =0,
            Right = 1,
            Wrong = 2
        }
        public void ShowSelection(Dictionary<Vector3, SelectionEnum> squareData)
        {
            ClearSelection();
            foreach (var data in squareData)
            {
                var selector = Instantiate(selectorPrefab, data.Key, Quaternion.identity);
                SpawnedSelectors.Add(selector);
                var mat = materialDict[data.Value];
                selector.ChangeMaterial(mat);
            }
        }

        public void ClearSelection()
        {
            for (int i = 0; i < SpawnedSelectors.Count; i++)
                Destroy(SpawnedSelectors[i]);
            SpawnedSelectors.Clear();
        }
    }
}