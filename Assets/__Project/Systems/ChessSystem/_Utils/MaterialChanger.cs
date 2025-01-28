using System.Linq;
using __Project.Systems.ChessSystem._Pieces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Utils
{
    public class MaterialChanger : MonoBehaviour
    {
#if UNITY_EDITOR
      
        [Button("Change Color"),HideInPlayMode]
        private void ChangeMaterialEditor(ChessColorEnum target)
        {

            var t = GetComponent<ChessPieceBase>();
            t.SetColor(target);
        }
        
        [Button("Change Material"),HideInPlayMode]
        private void ChangeMaterialEditor(Material mat)
        {
            
            var t =transform.GetComponentsInChildren<Renderer>().ToList();
            foreach (var rend in t)
            {
                rend.sharedMaterial = mat;
            }
        }
#endif
    }
}