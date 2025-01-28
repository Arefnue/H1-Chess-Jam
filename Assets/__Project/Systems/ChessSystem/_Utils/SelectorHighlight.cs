using UnityEngine;

namespace __Project.Systems.ChessSystem._Utils
{
    public class SelectorHighlight : MonoBehaviour
    {
        [SerializeField] private Renderer rend;

        public Vector3Int Position { get; private set; }
        public void Build(Vector3Int position)
        {
            Position = position;
        }
        
        public void ChangeMaterial(Material mat)
        {
            rend.material = mat;
        }
        
    }
}