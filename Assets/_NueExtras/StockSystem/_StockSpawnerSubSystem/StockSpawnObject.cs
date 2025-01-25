using System;
using System.Collections.Generic;
using _NueCore.AudioSystem;
using _NueCore.Common.ReactiveUtils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _NueExtras.StockSystem._StockSpawnerSubSystem
{
    public class StockSpawnObject : MonoBehaviour
    {
        [SerializeField] private bool usePhysic;
        [SerializeField] private bool ignoreDisable;
        [SerializeField] private AudioData claimEndSfxData;
        [SerializeField,ShowIf(nameof(usePhysic))] private Rigidbody rb;
        [SerializeField,ShowIf(nameof(usePhysic))] private Collider col;

        private bool _isDead;
        private Action<StockSpawnObject> FinishedAction { get; set; }

        public bool UsePhysic => usePhysic;

        public Rigidbody Rb => rb;

        public Collider Col => col;
        private List<IDisposable> _disposableList = new List<IDisposable>();

        public void ActivateDisable()
        {
            ignoreDisable = false;
        }

        private bool _hasBuilt;

        public void Build(Action<StockSpawnObject> onFinishedAction)
        {
            if(_hasBuilt)
                return;
            _hasBuilt = true;
            FinishedAction = onFinishedAction;
        }

        private void DisposeAll()
        {
            foreach (var disposable in _disposableList)
            {
                disposable?.Dispose();
            }
        }


        public void DestroyYourself()
        {
            if (ignoreDisable)
                return;
            
            //"Destroyed".Debug(Color.red);
            DisposeAll();
            //"DestroyYourself".Debug();
            if (!_isDead)
            {
                if(claimEndSfxData)
                    claimEndSfxData.Play();
                _isDead = true;
                FinishedAction?.Invoke(this);
                StockStatic.SaveStocks();
            }

            if (this == null)
                return;
            if (gameObject != null)
                Destroy(gameObject);
        }
  
        private void OnApplicationFocus(bool hasFocus)
        {
            if (ignoreDisable)
                return;
            if (!hasFocus)
            {
                TryKill();
                DestroyYourself();
            }
        }
        
        private void OnApplicationQuit()
        {
            if (ignoreDisable)
                return;
            TryKill();
        }

        private void OnDisable()
        {
            if (ignoreDisable)
                return;
            TryKill();
        }

        private void TryKill()
        {
            if (ignoreDisable)
                return;
            //"TryKill".Debug();
            DisposeAll();
            if (_isDead)
                return;
            _isDead = true;
           
            FinishedAction?.Invoke(this);
            StockStatic.SaveStocks();
            if (gameObject)
            {
                gameObject.SetActive(false);
            }
        }
    }
}