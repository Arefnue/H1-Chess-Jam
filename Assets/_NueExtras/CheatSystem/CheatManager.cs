using _NueCore.Common.ReactiveUtils;
using _NueCore.ManagerSystem.Core;
using _NueCore.SaveSystem;
using _NueExtras.StockSystem;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _NueExtras.CheatSystem
{
    public class CheatManager : NManagerBase
    {
        [SerializeField] private Button earnDiamondButton;
        [SerializeField] private Button earnCoinButton;
        [SerializeField] private Button earnEmeraldButton;
        [SerializeField] private Button timescaleDefaultButton;
        [SerializeField] private Button timescaleFastButton;
        [SerializeField] private Button timescaleVeryFastButton;
        [SerializeField] private Button disableCheatButton;
        [SerializeField] private Button levelButton;
        [SerializeField] private Button ageButton;
        [SerializeField] private Button resetSaveButton;
        [SerializeField] private Button multiplierButton;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private Button increaseButton;
        [SerializeField] private TMP_Text increaseText;
        [SerializeField] private Transform cheatRoot;
        
        private bool _isEnabled;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!_isEnabled)
                {
                    RBuss.Publish(new CheatREvents.EnableCheatREvent());
                }
                else
                {
                    RBuss.Publish(new CheatREvents.DisableCheatREvent());
                
                }
            }

        }

        private float _multiplier = 10f;
        private bool isIncrease = true;
        public override void NAwake()
        {
            base.NAwake();
            cheatRoot.gameObject.SetActive(false);
            
            increaseButton.onClick.AddListener(() =>
            {
                isIncrease = !isIncrease;
                increaseText.SetText(isIncrease ? "+" : "-");
            });
            increaseText.SetText(isIncrease ? "+" : "-");
            multiplierButton.onClick.AddListener(() =>
            {
                _multiplier *= 10f;
                if (_multiplier>=100000)
                {
                    _multiplier = 1f;
                }
                multiplierText.SetText("Multiplier: "+_multiplier);
            });
            multiplierText.SetText("Multiplier: "+_multiplier);
  
            levelButton.onClick.AddListener(() =>
            {
              
            });
            
            resetSaveButton.onClick.AddListener(() =>
            {
                NSaver.ResetAll();
            });
            
            ageButton.onClick.AddListener(() =>
            {
              
            });

            timescaleDefaultButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
            });
            
            timescaleFastButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1.5f;
            });
            
            timescaleVeryFastButton.onClick.AddListener(() =>
            {
                Time.timeScale = 3f;
            });
            
            earnCoinButton.onClick.AddListener(() =>
            {
                if (!isIncrease)
                {
                    StockStatic.DecreaseStock(StockTypes.Coin,_multiplier);
                    return;
                }
                StockStatic.IncreaseStock(StockTypes.Coin,_multiplier);
            });
            
            earnDiamondButton.onClick.AddListener(() =>
            {
                if (!isIncrease)
                {
                    StockStatic.DecreaseStock(StockTypes.Gem,_multiplier);
                    return;
                }
                StockStatic.IncreaseStock(StockTypes.Gem,_multiplier);
            });
            
            earnEmeraldButton.onClick.AddListener(() =>
            {
                if (!isIncrease)
                {
                    StockStatic.DecreaseStock(StockTypes.Emerald,_multiplier);
                    return;
                }
                StockStatic.IncreaseStock(StockTypes.Emerald,_multiplier);
            });
  
            disableCheatButton.onClick.AddListener(() =>
            {
                RBuss.Publish(new CheatREvents.DisableCheatREvent());
            });

            RBuss.OnEvent<CheatREvents.EnableCheatREvent>().Subscribe(ev =>
            {
                _isEnabled = true;
                cheatRoot.gameObject.SetActive(true);
            });

            RBuss.OnEvent<CheatREvents.DisableCheatREvent>().Subscribe(ev =>
            {
                _isEnabled = false;
                cheatRoot.gameObject.SetActive(false);
            });
        }

    }
}