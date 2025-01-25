using System;
using _NueCore.Common.ReactiveUtils;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace _NueExtras.StockSystem
{
    public class StockReceiver : MonoBehaviour
    {
        [SerializeField] private StockTypes targetStockType;
        [SerializeField] private UnityEvent stockValueChangedUnityEvent;
        [SerializeField] private UnityEvent onStartedUnityEvent;

        private IDisposable _stockValueDisposable;

        private void Start()
        {
            onStartedUnityEvent?.Invoke();
        }

        private void OnEnable()
        {
            _stockValueDisposable?.Dispose();
            _stockValueDisposable =RBuss.OnEvent<StockREvents.StockValueChangedREvent>().Subscribe(ev =>
            {
                if (ev.StockType != targetStockType)
                    return;
                
                stockValueChangedUnityEvent?.Invoke();
            });
        }

        private void OnDisable()
        {
            _stockValueDisposable?.Dispose();
        }
    }
}