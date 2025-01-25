using __Project.Systems.LevelSystem;
using __Project.Systems.PowerUpSystem._Actions;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using _NueCore.SaveSystem;
using _NueExtras.StockSystem;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace __Project.Systems.PowerUpSystem
{
    public class PowerUpButton : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private Transform lockRoot;
        [SerializeField] private Transform activeRoot;
        [SerializeField] private Transform priceRoot;
        [SerializeField] private Transform remainingRoot;
        [SerializeField] private TMP_Text remainingCountText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text lockDescription;
        [SerializeField] private Button useButton;
        [SerializeField] private PowerUpActionBase actionBase;
        [SerializeField] private int unlockLevel;
        [SerializeField] private int price = 25;
        
        #region Cache
        private string _cachedLockText;
        private string _cachedPriceText;
        public int UnlockLevel => unlockLevel;
        public bool IsLocked { get; private set; }
        public PowerUpSave.PowerUpInfo Info { get; private set; }
        private bool _isBuilt;
        
        #endregion


        #region Setup
        public void Build()
        {
            if (_isBuilt)
                return;

            _isBuilt = true;
            _cachedLockText = lockDescription.text;
            _cachedPriceText = priceText.text;
            var powerSave = NSaver.GetSaveData<PowerUpSave>();
            Info = powerSave.GetPowerUpInfo(GetID());
            actionBase.Build(this);
            
            useButton.onClick.AddListener(Use);
            CheckLock();
            UpdateButton();
            

            RBuss.OnEvent<StockREvents.StockValueChangedREvent>().Subscribe(ev =>
            {
                if (ev.StockType != StockTypes.Coin)
                    return;
                UpdateButton();
            }).AddTo(gameObject);

            RBuss.OnEvent<LevelREvents.LevelSpawnedREvent>().Subscribe(ev =>
            {
                actionBase.Reset();
                CheckLock();
                UpdateButton();
            }).AddTo(gameObject);
           
        }
        #endregion

        #region Methods
        public void UpdateButton()
        {
            CheckUseButton();
            UpdateRemainingCountText();
        }
        private void CheckLock()
        {
            var powerSave = NSaver.GetSaveData<PowerUpSave>();
            if (Info.isUnlocked)
            {
                Unlock();
            }
            else
            {
                var level = LevelStatic.GetCurrentLevelNumber();
                if (level >= UnlockLevel)
                {
                    Info.isUnlocked = true;
                    powerSave.Save();
                    Unlock();
                }
                else
                {
                    Lock();
                }
            }
        }
        [Button]
        public void AddUseCount(int value = 1)
        {
            var powerSave = NSaver.GetSaveData<PowerUpSave>();
            Info.remainingCount += value;
            powerSave.Save();
            CheckUseButton();
            UpdateRemainingCountText();
        }
        private bool IsUseButtonActive()
        {
            if (IsLocked)
            {
                return false;
            }
            if (Info == null)
            {
                return false;
            }

            if (!actionBase.CanUse())
            {
                return false;
            }

            return true;
        }
        public void CheckUseButton()
        {
            var isActive = IsUseButtonActive();
            useButton.interactable = isActive;
            if (Info.remainingCount<= 0)
            {
                priceRoot.gameObject.SetActive(true);
                remainingRoot.gameObject.SetActive(false);
                priceText.SetText(_cachedPriceText.Replace("#price",price.ToString()));
            }
            else
            {
                priceRoot.gameObject.SetActive(false);
                remainingRoot.gameObject.SetActive(true);
            }
        }
        private bool CanBuy()
        {
            var currentStock = StockStatic.GetStockRounded(StockTypes.Coin);
            if (currentStock>=price)
            {
                return true;
            }

            return false;
        }
        public void Use()
        {
            if (Info.remainingCount<=0)
            {
                if (!CanBuy())
                {
                    "Open shop".NLog(Color.yellow);
                    return;
                }
                StockStatic.DecreaseStock(StockTypes.Coin,price);
            }
            else
            {
                Info.remainingCount--;
            }
            actionBase.Apply();
            UpdateButton();
            UpdateRemainingCountText();
        }
        private void UpdateRemainingCountText()
        {
            remainingCountText.SetText(Info.remainingCount.ToString());
        }
        public string GetID()
        {
            return id;
        }
        public void Lock()
        {
            IsLocked = true;
            activeRoot.gameObject.SetActive(false);
            lockRoot.gameObject.SetActive(true);
            lockDescription.SetText(_cachedLockText.Replace("#level",UnlockLevel.ToString()));
        }
        public void Unlock()
        {
            IsLocked = false;
            lockRoot.gameObject.SetActive(false);
            activeRoot.gameObject.SetActive(true);
        }
        #endregion
    }
}