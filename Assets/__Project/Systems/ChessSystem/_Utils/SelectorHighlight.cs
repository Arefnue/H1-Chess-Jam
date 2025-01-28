using UnityEngine;

namespace __Project.Systems.ChessSystem._Utils
{
    public class SelectorHighlight : MonoBehaviour
    {
        [SerializeField] private Renderer rend;

        public void ChangeMaterial(Material mat)
        {
            rend.material = mat;
        }
        
    }
}