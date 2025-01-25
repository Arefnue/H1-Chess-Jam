using System;
using System.Text;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _NueExtras.StockSystem._StockUISubSystem
{
    public class StockButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private bool disableSpend;
        
       
        #region Cache
        public Action OnClickedAction { get; private set; }
        public Action OnUpdatedAction { get; private set; }
        public StockTypes StockType { get; private set; }
        public Func<bool> CanPurchaseConditionFunc { get; private set; }
        public Action UpdateConditionAction { get; private set; }
        public int Price { get; private set; }

        public Button Button => button;

        private bool _isRpButton;
        private IntReactiveProperty _rp;
        #endregion
       
        #region Setup
        public void Build(StockButtonInfo info)
        {
            OnClickedAction += info.onClickedAction;
            OnUpdatedAction += info.onButtonUpdatedAction;
            StockType = info.stockType;
            CanPurchaseConditionFunc += info.canPurchaseConditionFunc;
            UpdateConditionAction += info.updateConditionAction;
            Price = info.price;
            
            priceText.SetText(GetPriceText());
            Button.onClick.AddListener(Purchase);
            
            RBuss.OnEvent<StockREvents.StockValueChangedREvent>().Subscribe(ev =>
            {
                if (StockType != ev.StockType)
                    return;
                CheckButton();
            }).AddTo(gameObject);
            
            CheckButton();
        }
        public void Build(RPButtonInfo info)
        {
            _isRpButton = true;
            OnClickedAction += info.onClickedAction;
            OnUpdatedAction += info.onButtonUpdatedAction;
            Price = info.price;
            _rp = info.rp;
            priceText.SetText(GetPriceText());
            Button.onClick.AddListener(Purchase);

            _rp.Subscribe(value =>
            {
                CheckButton();
            }).AddTo(gameObject);
            CheckButton();
        }

        public void UpdatePrice(int price)
        {
            Price = price;
            priceText.SetText(GetPriceText());
            CheckButton();
        }

        #endregion

        #region Methods

        private StringBuilder _stringBuilder = new StringBuilder();
        private string GetPriceText()
        {
            _stringBuilder.Clear();
            if (Price<=0)
                return "Free";
            _stringBuilder.Append(Price);
            if (!_isRpButton)
            {
                _stringBuilder.Append(SpriteHelper.GetSpriteText(SpriteHelper.GetStockSpriteName(StockType)));
            }
            else
            {
                _stringBuilder.Append("$");
            }
            return _stringBuilder.ToString();
        }
        public void CheckButton()
        {
            if (Button == null)
            {
                "Call".NLog(Color.red,gameObject);
                return;
            }
            var condition = _isRpButton ? _rp.Value>=Price: StockStatic.CanPurchase(StockType, Price);
            if (CanPurchaseConditionFunc != null)
            {
                if (condition)
                    condition = CanPurchaseConditionFunc.Invoke();
            }
            if (disableSpend)
                condition = true;
            
            if (condition)
            {
                Button.interactable = true;
            }
            else
            {
                Button.interactable = false;
            }
            OnUpdatedAction?.Invoke();
        }
        private void Purchase()
        {
            if (Price>0)
            {
                if (!disableSpend)
                {
                    if (_isRpButton)
                        _rp.Value -= Price;
                    else
                        StockStatic.DecreaseStock(StockType,Price);
                }
              
            }
            OnClickedAction?.Invoke();
        }
        #endregion
       
        #region Info
        public struct StockButtonInfo
        {
            public Action onClickedAction;
            public Action onButtonUpdatedAction;
            public StockTypes stockType;
            public Func<bool> canPurchaseConditionFunc;
            public Action updateConditionAction;

            public int price;
        }
        
        public struct RPButtonInfo
        {
            public Action onClickedAction;
            public Action onButtonUpdatedAction;
            public IntReactiveProperty rp;
            public int price;
        }
        
        #endregion
    }
}