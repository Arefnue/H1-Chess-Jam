using System;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace __Project.Systems.LevelSystem._TimerSubSystem
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image fillImage;
        

        public LevelBase Level { get; private set; }
        public FloatReactiveProperty CurrentTimerRP { get; private set; } = new FloatReactiveProperty();
        public bool IsTimerStarted { get; private set; }
        private bool _isFinished;
        public void Build()
        {
            _isFinished = false;
            RBuss.OnEvent<LevelREvents.LevelSpawnedREvent>().Subscribe(ev =>
            {
                Level = ev.LevelBase;
                levelText.SetText($"Level {(Level.TargetLevel+1).ToString()}");
                StartTimer();
            }).AddTo(gameObject);
            
            CurrentTimerRP.Subscribe(value =>
            {
                if (Level == null)
                    return;
                if (_isFinished)
                {
                    return;
                }
                var tValue = CurrentTimerRP.Value;
                if (tValue<0)
                    tValue = 0;
                var dt = tValue;
                timerText.SetText(TimeHelper.ConvertToTimer(Mathf.RoundToInt(dt)));
                var inverse = Mathf.InverseLerp(0, GetMaxDuration(), tValue);
                fillImage.fillAmount = inverse;
                if (tValue<=0)
                {
                    StopTimer();
                    Finish();
                }
            }).AddTo(gameObject);
            
        }
        
        public int GetMaxDuration()
        {
            return Level.LevelDuration;
        }

        private IDisposable _timer;
        public void StartTimer()
        {
            _isFinished = false;
            CurrentTimerRP.Value = Level.LevelDuration;
            IsTimerStarted = true;
            _timer?.Dispose();
            _timer =Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(ev =>
            {
                CurrentTimerRP.Value -= 1;
            });
        }

        public void StopTimer()
        {
            IsTimerStarted = true;
            _timer?.Dispose();
        }

        private void Finish()
        {
            _isFinished = true;
            LevelStatic.LoseLevel();
        }
    }
}