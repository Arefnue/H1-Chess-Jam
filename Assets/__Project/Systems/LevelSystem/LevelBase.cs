using System;
using System.Threading;
using __Project.Systems.BlockSystem;
using __Project.Systems.GridSystem;
using __Project.Systems.LevelSystem._MissionSubSystem;
using _NueCore.Common.ReactiveUtils;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace __Project.Systems.LevelSystem
{
    public class LevelBase : MonoBehaviour
    {
        [SerializeField,TabGroup("Settings")] private int coinReward = 4;
        
        [SerializeField,TabGroup("References")] private MissionController missionController;
        [SerializeField,TabGroup("References")] private GridController gridController;
        [SerializeField,TabGroup("References")] private BlockController blockController;
       
        #region Cache
        public int TargetLevel { get; private set; }
        private CancellationToken CancellationToken => gameObject.GetCancellationTokenOnDestroy();
        public int CoinReward => coinReward;
        public int TotalColorReward { get; private set; }

        public GridController GridController => gridController;

        public MissionController MissionController => missionController;

        public BlockController BlockController => blockController;

        #endregion

        #region Setup
        public void Build(int targetLevel)
        {
            TargetLevel = targetLevel;
            LevelStatic.SetCurrentLevel(this);
            LevelStatic.IsInteractionEnabled.Value = true;
            BlockController.Build();
            MissionController.Build(this);
            GridController.Build(this);
            CalculateColorReward();
            RegisterREvents();
            CheckMissionSteps();
        }
        #endregion

        #region Reactive
        private void RegisterREvents()
        {
            RBuss.OnEvent<MissionREvents.StepCompletedREvent>()
                .TakeUntilDisable(gameObject)
                .Subscribe(ev =>
                {
                    CheckMissionSteps();
                });
        }
        #endregion

        #region Methods
        private void CalculateColorReward()
        {
            TotalColorReward = 0;
            foreach (var step in MissionController.MissionStepList)
                TotalColorReward += (step.MissionList.Count*3);
        }
     
        private void CheckMissionSteps()
        {
            ProcessStepAsync().AttachExternalCancellation(CancellationToken).Forget();
        }
        
        [Button,TabGroup("Editor"),HideInEditorMode]
        public void CompleteStep()
        {
            MissionController.CompleteStep();
        }
        #endregion
        
        #region Async
        private async UniTask ProcessStepAsync()
        {
            LevelStatic.IsInteractionEnabled.Value = false;
            if (MissionController.IsAllStepsCompleted())
            {
                await FinishLevelAsync();
                return;
            }
            MissionController.SetCurrentMission();
            GridController.SetActiveLayer(MissionController.CompletedStepCount);
            //TODO activate grid layer
            GridController.ActiveLayer.ActivateLayer();
            MissionController.SelectMission();
            
            await UniTask.WhenAll(MissionController.AdjustImageAsync(), GridController.MoveGridLayersAsync(MissionController.CompletedStepCount));
            LevelStatic.IsInteractionEnabled.Value = true;
            //TODO check faces
            GridController.ActiveLayer.UpdateLayer();
        }
        
        
        private async UniTask FinishLevelAsync()
        {
            LevelStatic.LevelUp();
            LevelStatic.IsInteractionEnabled.Value = false;
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: CancellationToken);
            LevelStatic.WinLevel();
        }
        #endregion

        #region Editor

#if UNITY_EDITOR
        [Button("Set Level"),TabGroup("Editor"),HideInPlayMode]
        public void SetupLevelEditor(bool apply)
        {
            if (!apply)
                return;
            MissionController.FindAllStepsEditor(true);
            GridController.FindGridLayersEditor(true);
        }
#endif

        #endregion

    }
}