using UnityEngine;

namespace _NueExtras.PoolSystem
{
    public class PoolExample : MonoBehaviour
    {
        public static GenericPool<PoolObjectExample> Pool { get; private set; }
        [SerializeField] private GameObject objectPrefab;

        private void Awake()
        {
            Pool = new GenericPool<PoolObjectExample>(objectPrefab,10)
            {
                CanDead = true,
                PullAction = CallOnPull,
                PushAction = CallOnPush,
                SpawnParent = PoolManager.Instance.PoolRoot
            };
        }
        
        private void SomeFunction()
        {
            PoolObjectExample newExamplePoolObject = Pool.Pull();
        }

        private void CallOnPull(PoolObjectExample obj)
        {
        }

        private void CallOnPush(PoolObjectExample obj)
        {
            
        }
    }
}