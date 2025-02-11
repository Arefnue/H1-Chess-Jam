using System.Collections.Generic;
using __Project.Systems.LevelSystem._LevelEndSubSystem;
using __Project.Systems.LevelSystem._TimerSubSystem;
using _NueCore.Common.ReactiveUtils;
using _NueCore.FaderSystem;
using _NueCore.ManagerSystem.Core;
using _NueCore.SaveSystem;
using _NueExtras.PopupSystem.PopupDataSub;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace __Project.Systems.LevelSystem
{
    public class LevelManager : NManagerBase
    {
        [SerializeField] private List<LevelBase> orderedLevelPrefabList = new List<LevelBase>();
        [SerializeField] private List<LevelBase> loopLevelPrefabList = new List<LevelBase>();
        [SerializeField] private Transform levelSpawnRoot;
        [SerializeField] private PopupDataDisplay winPopup;
        [SerializeField] private PopupDataDisplay losePopup;
        [SerializeField] private TimeController timeController;
        

        public LevelBase SpawnedLevel { get; private set; }

        private PopupDisplay _popup;

        #region Setup

        public override void NAwake()
        {
            base.NAwake();
            timeController.Build();
            RBuss.OnEvent<LevelREvents.LevelWonREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (_popup)
                    _popup.ClosePopup();
                _popup =winPopup.OpenPopup();
                if (_popup.TryGetComponent<LevelEnd_Win>(out var panel))
                {
                    var info = new LevelEndInfo
                    {
                        Level = ev.Level-1,
                        CoinReward = SpawnedLevel.CoinReward,
                        OnContinueButtonClickedAction = LoadNextLevelWithFader
                    };
                    panel.Build(info);
                }
                
            });
            
            RBuss.OnEvent<LevelREvents.LevelLostREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (_popup)
                    _popup.ClosePopup();
                _popup =losePopup.OpenPopup();
                if (_popup.TryGetComponent<LevelEnd_Lose>(out var panel))
                {
                    var info = new LevelEndInfo
                    {
                        Level = ev.Level,
                        CoinReward = SpawnedLevel.CoinReward,
                        OnContinueButtonClickedAction = LoadNextLevelWithFader
                    };
                    panel.Build(info);
                }
            });
        }

        public override void NStart()
        {
            base.NStart();
            LoadNextLevel();
        }

        #endregion


        #region Methods

        [Button]
        private void LevelUp()
        {
            LevelStatic.LevelUp();
            LoadNextLevel();
        }

        public void Restart()
        {
            LoadNextLevel();

        }

        private void LoadNextLevelWithFader()
        {
            var faderParams = new NFader.FaderParams
            {
                waitDuration = 0.2f,
                fadeInDuration = 0.5f,
                fadeOutDuration = 0.5f,
                fadeInFinishedAction = LoadNextLevel
            };
            NFader.Fade(FaderTypes.Default,faderParams);
        }
        public void LoadNextLevel()
        {
            var levelSave = NSaver.GetSaveData<LevelSave>();
            SpawnLevel(levelSave.CompletedLevelCount);
        }
        
        public void SpawnLevel(int targetLevel)
        {
            DestroyOldLevel();
            if (targetLevel < orderedLevelPrefabList.Count)
            {
                SpawnedLevel =Instantiate(orderedLevelPrefabList[targetLevel], levelSpawnRoot);
            }
            else
            {
                var diff = targetLevel - orderedLevelPrefabList.Count;
                SpawnedLevel =Instantiate(loopLevelPrefabList[diff % loopLevelPrefabList.Count], levelSpawnRoot);
            }
            SpawnedLevel.Build(targetLevel);
            RBuss.Publish(new LevelREvents.LevelSpawnedREvent(SpawnedLevel));
        }

       
        public void DestroyOldLevel()
        {
            if (!SpawnedLevel)
            {
                return;
            }
            Destroy(SpawnedLevel.gameObject);
        }


        #endregion
       
    }
}