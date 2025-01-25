using System;
using _NueCore.Common.NueLogger;
using DG.Tweening;
using UnityEngine;

namespace _NueCore.FaderSystem
{
    public static class NFader
    {
        #region Cache
        private static FaderManager FaderManager => FaderManager.Instance;
        private static Sequence _fadeSeq;
        private static readonly int FadeInAnimKey = Animator.StringToHash("FadeIn");
        private static readonly int FadeOutAnimKey = Animator.StringToHash("FadeOut");
        private static readonly int MotionKey = Animator.StringToHash("Motion");

        #endregion
        
        #region Commons
        private static void SetFaderLayer(FaderTypes faderType)
        {
            FaderManager.SetFaderLayer(faderType);
        }
        private static void TriggerFaderAnim(int animKey)
        {
            FaderManager.FaderAnimator.SetTrigger(animKey);
        }
        #endregion

        #region Fades
        public static void Fade(FaderTypes faderType, FaderParams @params = default)
        {
            SetFaderLayer(faderType);
            _fadeSeq?.Kill();
            _fadeSeq.SetUpdate(UpdateType.Normal, true);
            _fadeSeq = DOTween.Sequence();
            _fadeSeq.Append(FadeIn(faderType, @params));
        
            _fadeSeq.Append(DOVirtual.DelayedCall(@params.waitDuration, () =>
            {
                @params.waitFinishedAction?.Invoke();
            }).SetUpdate(true));
            _fadeSeq.Append(FadeOut(faderType, @params));
        }
        public static Tween FadeIn(FaderTypes faderType, FaderParams @params = default)
        {
          
            return DOVirtual.Float(0, 1, @params.fadeInDuration, value =>
            {
                FaderManager.FaderAnimator.SetFloat(MotionKey, value);
            }).SetUpdate(true).OnStart(() =>
            {
             
                SetFaderLayer(faderType);
                TriggerFaderAnim(FadeInAnimKey);
                FaderManager.FaderRoot.gameObject.SetActive(true);
                FaderManager.FaderAnimator.SetFloat(MotionKey, 0);
            }).OnComplete(() =>
            {
              
                @params.fadeInFinishedAction?.Invoke();
               
            });
        }
        
        public static Tween FadeOut(FaderTypes faderType,FaderParams @params = default)
        {
            
            return DOVirtual.Float(0, 1, @params.fadeOutDuration, value =>
            {
                FaderManager.FaderAnimator.SetFloat(MotionKey, value);
            }).SetUpdate(true).OnStart(() =>
            {
                SetFaderLayer(faderType);
                TriggerFaderAnim(FadeOutAnimKey);
                FaderManager.FaderRoot.gameObject.SetActive(true);
                FaderManager.FaderAnimator.SetFloat(MotionKey, 0);
              
            }).OnComplete(() =>
            {
                @params.fadeOutFinishedAction?.Invoke();
                FaderManager.FaderRoot.gameObject.SetActive(false);
            });
        }
        #endregion
        
        #region Structs
        public struct FaderParams
        {
            public float fadeInDuration;
            public float fadeOutDuration;
            public float waitDuration;
            public Action fadeInFinishedAction;
            public Action fadeOutFinishedAction;
            public Action waitFinishedAction;
            public FaderTypes fadeInType;
            public FaderTypes fadeOutType;

            public static FaderParams Default => new FaderParams()
            {
                fadeInDuration = 0f,
                fadeOutDuration = 1f,
                waitDuration = 0.5f,
                fadeInType = FaderTypes.Default,
                fadeOutType = FaderTypes.Default
            };
            public FaderParams(float fadeInDuration = 1f, float fadeOutDuration = 1f)
            {
                this.fadeInDuration = fadeInDuration;
                this.fadeOutDuration = fadeOutDuration;
                waitDuration = 0;
                fadeInFinishedAction = null;
                fadeOutFinishedAction = null;
                waitFinishedAction = null;
                fadeInType = FaderTypes.Default;
                fadeOutType = FaderTypes.Default;
            }
        }
        #endregion
    }
}