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
        public void ShowSelection(Dictionary<Vector3Int, SelectionEnum> tData)
        {
            ClearSelection();
            foreach (var data in tData)
            {
                var spawnPos = new Vector3(data.Key.x, 0, data.Key.y) + new Vector3(0.5f,0,0.5f);
                var selector = Instantiate(selectorPrefab, spawnPos, Quaternion.identity);
                selector.Build(data.Key);
                var mat = materialDict[data.Value];
                selector.ChangeMaterial(mat);
                SpawnedSelectors.Add(selector);
            }
        }

        public void ClearSelection()
        {
            for (int i = 0; i < SpawnedSelectors.Count; i++)
                Destroy(SpawnedSelectors[i].gameObject);
            SpawnedSelectors.Clear();
        }
    }
}