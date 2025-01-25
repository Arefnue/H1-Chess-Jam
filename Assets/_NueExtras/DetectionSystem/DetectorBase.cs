using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueExtras.DetectionSystem
{
    public abstract class DetectorBase <T> : MonoBehaviour
    {
        [SerializeField] private bool useIFrame;
        [SerializeField,ShowIf(nameof(useIFrame))] private float iFrameDuration = 0.1f;

        #region Actions

        public Action<T> EnterAction;
        public Action<T> ExitAction;

        #endregion

        #region Cache
        
        private bool _iFrameActive;
        private Tween _iFrameTween;
        private bool _isExit;
        private T _lastTarget;
        #endregion

        #region Setup
        
        private void OnDisable()
        {
            if (!_isExit)
            {
                ApplyTriggerExit(_lastTarget);
            }
        }


        #endregion

        #region Methods
        protected virtual void ApplyTriggerEnter(T target)
        {
            if (_iFrameActive)
                return;
            
            if (useIFrame)
            {
                if (!_iFrameActive)
                {
                    _iFrameActive = true;
                    _iFrameTween = DOVirtual.DelayedCall(iFrameDuration, () =>
                    {
                        _iFrameActive = false;
                    },false);
                }
            }

            EnterAction?.Invoke(target);
            _isExit = false;
            _lastTarget = target;
        }

        protected virtual void ApplyTriggerExit(T target)
        {
            _isExit = true;
          
            ExitAction?.Invoke(target);
        }
        #endregion

        #region Events
        private void OnTriggerEnter(Collider other)
        {
            if (!other.attachedRigidbody)
                return;

            if (other.attachedRigidbody.TryGetComponent<T>(out var target))
                ApplyTriggerEnter(target);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.attachedRigidbody)
                return;

            var id = other.attachedRigidbody.gameObject.GetInstanceID();
            if (other.attachedRigidbody.TryGetComponent<T>(out var target))
                ApplyTriggerExit(target);
        }
        #endregion
        
    }
}