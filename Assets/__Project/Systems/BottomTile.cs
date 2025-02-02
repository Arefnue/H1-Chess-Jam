using System;
using UnityEngine;

namespace __Project.Systems
{
    public class BottomTile : MonoBehaviour
    {
        [SerializeField] private Material whiteMat;
        [SerializeField] private Material blackMat;
        [SerializeField] private Renderer rend;


        private void Awake()
        {
            rend.material = (transform.position.x + transform.position.z) % 2 == 0 ? whiteMat : blackMat;
        }
    }
}