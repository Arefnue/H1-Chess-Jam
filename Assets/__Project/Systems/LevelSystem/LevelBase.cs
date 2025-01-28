using System;
using System.Threading;
using __Project.Systems.BlockSystem;
using __Project.Systems.ChessSystem._Pieces;
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
        [SerializeField,TabGroup("References")] private GridController_Chess gridController;
        [SerializeField,TabGroup("References")] private ChessController chessController;
       
        #region Cache
        public int TargetLevel { get; private set; }
        private CancellationToken CancellationToken => gameObject.GetCancellationTokenOnDestroy();
        public int CoinReward => coinReward;

        public GridController_Chess GridController => gridController;

        public ChessController ChessController => chessController;

        #endregion

        #region Setup
        public void Build(int targetLevel)
        {
            TargetLevel = targetLevel;
            LevelStatic.SetCurrentLevel(this);
            LevelStatic.IsInteractionEnabled.Value = true;
            GridController.Build();
            ChessController.Build();
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
       
        private void CheckMissionSteps()
        {
            ProcessStepAsync().AttachExternalCancellation(CancellationToken).Forget();
        }
        
        [Button,TabGroup("Editor"),HideInEditorMode]
        public void CompleteStep()
        {
            //MissionController.CompleteStep();
        }
        #endregion
        
        #region Async
        private async UniTask ProcessStepAsync()
        {
            LevelStatic.IsInteractionEnabled.Value = false;
            if (ChessController.IsAllStepsCompleted())
            {
                await FinishLevelAsync();
                return;
            }
            
            //TODO activate grid layer
            
            await UniTask.WhenAll(GridController.MoveGridLayersAsync(ChessController.ActiveStepCount));
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
            //MissionController.FindAllStepsEditor(true);
            GridController.FindGridLayersEditor(true);
        }
#endif

        #endregion

    }
}