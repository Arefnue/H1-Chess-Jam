using System;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _NueExtras.StockSystem._StockUISubSystem
{
    public class StockCounterCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text counterTextField;
        [SerializeField] private Transform scaleRoot;
        [SerializeField] private StockTypes stockType;
        [SerializeField] private bool convertToDigit;
        [SerializeField] private bool useCustomImage;
        [SerializeField,HideIf(nameof(useCustomImage))] private StockSpriteCatalog stockSpriteCatalog;
        [SerializeField,HideIf(nameof(useCustomImage))] private Image stockImage;
        

        #region Cache
        public TMP_Text CounterTextField => counterTextField;
        private Tween _updateTween;
        private IDisposable _disposable;
        private bool _isBuilt;
        #endregion

        #region Setup
        private void Awake()
        {
            Build();
        }
        private void Start()
        {
            
            UpdateStockCounter();
        }
        public void Build()
        {
            _disposable?.Dispose();
            _disposable =RBuss.OnEvent<StockREvents.StockValueChangedREvent>().Subscribe(ev =>
            {
                if (stockType == ev.StockType)
                {
                    UpdateStockCounter();
                }
            });

            if (!useCustomImage)
            {
                stockImage.sprite = stockSpriteCatalog.GetStockSprite(stockType);
            }
        }
        public void Dispose()
        {
            _disposable?.Dispose();
        }
        #endregion

        #region Methods
        public void UpdateStockCounter()
        {
            Punch();

            var targetValue = StockStatic.GetStockRounded(stockType);
            if (convertToDigit)
            {
                var convertedValue = NumberHelper.ToAbbreviate(targetValue);
                CounterTextField.SetText(convertedValue);
                return;
            }
            CounterTextField.SetText(targetValue.ToString());
        }
        
        private void Punch()
        {
            _updateTween?.Kill(true);

            _updateTween = scaleRoot.DOPunchScale(Vector3.one * 0.25f, 0.25f, 1, 0.1f).OnComplete(() =>
            {
                scaleRoot.localScale = Vector3.one;
            });
        }
        #endregion
      
      

        
    }
}