using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _NueExtras.PoolSystem
{
    public class GenericPool<T> : IPool<T> where T : MonoBehaviour, IPoolObject<T>
    {
        #region Cache

        public Action<T> PullAction { get;  set; } = null;
        public Action<T> PushAction { get;  set; } = null;
        public Transform SpawnParent { get;  set; } = null;
        public bool CanDead { get;  set; } = false;
        public int PooledCount => _poolStack.Count;
        
        private readonly Stack<T> _poolStack = new Stack<T>();
        private readonly GameObject _prefab;
        #endregion

        #region Setup
        
        public GenericPool(GameObject pooledObject, int numToSpawn)
        {
            this._prefab = pooledObject;
            Spawn(numToSpawn);
        }
        #endregion

        #region Spawn
        private void Spawn(int numToSpawn)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var parent = SpawnParent;
            var targetType = Object.Instantiate(_prefab, parent).GetComponent<T>();
            _poolStack.Push(targetType);
            targetType.gameObject.SetActive(false);
        }
        #endregion

        #region Pull
        public T Pull()
        {
            T targetComponent;
            if (PooledCount > 0)
                targetComponent = _poolStack.Pop();
            else
            {
                var parent = SpawnParent;
                targetComponent = Object.Instantiate(_prefab, parent).GetComponent<T>();
            }

            if (CanDead)
            {
                if (targetComponent == null)
                    return Pull();
            }

            targetComponent.gameObject.SetActive(true);
            targetComponent.BuildPoolObject(Push);
            PullAction?.Invoke(targetComponent);

            return targetComponent;
        }
        #endregion

        #region Push
        public void Push(T t)
        {
            _poolStack.Push(t);
            PushAction?.Invoke(t);
            t.gameObject.SetActive(false);
        }
        #endregion
        
    }
}