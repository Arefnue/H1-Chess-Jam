using _NueCore.ManagerSystem.Core;
using UnityEngine;

namespace _NueExtras.PoolSystem
{
    public class PoolManager : NManagerBase
    {
        [SerializeField] private Transform poolRoot;
        
        public static PoolManager Instance { get; private set; }

        public Transform PoolRoot => poolRoot;

        public override void NAwake()
        {
            Instance = InitSingleton<PoolManager>();
            base.NAwake();
        }
    }
}