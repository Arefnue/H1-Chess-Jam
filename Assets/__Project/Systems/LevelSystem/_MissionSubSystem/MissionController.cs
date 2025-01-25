using System.Collections.Generic;
using _NueCore.Common.ReactiveUtils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace __Project.Systems.LevelSystem._MissionSubSystem
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField,TabGroup("Steps")] private List<MissionStep> missionStepList = new List<MissionStep>();
        [SerializeField,TabGroup("References")] private Transform imageRoot;
        [SerializeField,TabGroup("References")] private Transform imageMoveRoot;
        
        [SerializeField,TabGroup("UI")] private TMP_Text levelText;
        [SerializeField,TabGroup("UI")] private Image stepFillImage;
        [SerializeField,TabGroup("UI")] private List<Image> stepNodeList;
        [SerializeField,TabGroup("UI")] private Sprite activeNodeSprite;
        [SerializeField,TabGroup("UI")] private Sprite passiveNodeSprite;
        [SerializeField,TabGroup("UI")] private Sprite nextNodeSprite;

        #region Cache
        public int CompletedStepCount { get; private set; }
        public List<MissionStep> MissionStepList => missionStepList;
        public Transform ImageRoot => imageRoot;
        public Transform ImageMoveRoot => imageMoveRoot;
        public MissionStep ActiveMissionStep { get; private set; }
        public LevelBase Level { get; private set; }
        #endregion

        #region Setup
        public void Build(LevelBase level)
        {
            Level = level;
            CompletedStepCount = 0;
            UpdateStepVisuals();
            levelText.SetText($"Level {Level.TargetLevel+1}");
            foreach (var step in MissionStepList)
                step.Build();

            RegisterREvents();
        }
        #endregion

        #region Reactive
        private void RegisterREvents()
        {
            RBuss.OnEvent<MissionREvents.MissionCompletedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActiveMissionStep.CompleteMission();
                CheckMission();
            });
        }
        #endregion

        #region Methods
        public void SetCurrentMission()
        {
            ActiveMissionStep = MissionStepList[CompletedStepCount];;
        }

        public void SelectMission()
        {
            if (CompletedStepCount<=0)
            {
                CheckMission();
                return;
            }
            if (ActiveMissionStep.TryGetCurrentMission(out var mission))
                RBuss.Publish(new MissionREvents.ActiveMissionSelectedREvent(mission));
        }
        private void CheckMission()
        {
            if (ActiveMissionStep.TryGetCurrentMission(out var mission))
                RBuss.Publish(new MissionREvents.ActiveMissionSelectedREvent(mission));
            else
                CompleteStep();
        }

        public bool IsAllStepsCompleted()
        {
            return CompletedStepCount >= MissionStepList.Count;
        }

  
        [Button,TabGroup("Editor"),HideInEditorMode]
        public void CompleteStep()
        {
            CompletedStepCount++;
            UpdateStepVisuals();
            RBuss.Publish(new MissionREvents.StepCompletedREvent(ActiveMissionStep));
        }

        
        private void UpdateStepVisuals()
        {
            foreach (var image in stepNodeList)
            {
                image.gameObject.SetActive(false);
            }

            for (int i = 0; i < MissionStepList.Count; i++)
            {
                if (i>=stepNodeList.Count)
                    break;
                stepNodeList[i].gameObject.SetActive(true);
            }
            foreach (var image in stepNodeList)
                image.sprite = passiveNodeSprite;

            for (int i = 0; i <= CompletedStepCount; i++)
            {
                if (i>=stepNodeList.Count)
                    break;
                var node = stepNodeList[i];
                node.sprite = i == CompletedStepCount ? nextNodeSprite : activeNodeSprite;
            }
            stepFillImage.fillAmount = Mathf.InverseLerp(0,MissionStepList.Count,CompletedStepCount);
        }
        #endregion

        #region Async
        public async UniTask<bool> AdjustImageAsync()
        {
            var targetY =ActiveMissionStep.ImageRevealY;
            await ImageMoveRoot.DOLocalMoveY(targetY, 2f).SetEase(Ease.InSine);
            return true;
        }
        #endregion

        #region Editor

#if UNITY_EDITOR
        
        [Button("Find All Mission Steps"),TabGroup("Editor"),HideInPlayMode]
        public void FindAllStepsEditor(bool apply)
        {
            if (!apply)
            {
                return;
            }
            missionStepList.Clear();
            var steps = GetComponentsInChildren<MissionStep>();
            foreach (var step in steps)
            {
                missionStepList.Add(step);
            }
        }
#endif

        #endregion

    }
}