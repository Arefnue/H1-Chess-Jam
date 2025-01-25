using System;
using UnityEngine;

namespace _NueExtras.PoolSystem
{
    public class PoolObjectExample : MonoBehaviour,IPoolObject<PoolObjectExample>
    {
        private Action<PoolObjectExample> _returnToPool;
        private void OnDisable()
        {
            ReturnPoolObject();
        }

        public void BuildPoolObject(Action<PoolObjectExample> returnAction)
        {
            this._returnToPool = returnAction;
        }

        public void ReturnPoolObject()
        {
            _returnToPool?.Invoke(this);
        }
    }
}