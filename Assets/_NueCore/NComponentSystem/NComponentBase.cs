using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _NueCore.NComponentSystem
{
    public abstract class NComponentBase : MonoBehaviour
    {
        [SerializeField,TabGroup("Settings")] private UpdateTypeEnums updateType = UpdateTypeEnums.Normal;
        [SerializeField,TabGroup("Settings")] private ActivationTypeEnums activationType = ActivationTypeEnums.Enable;
        [SerializeField,TabGroup("Events")] private UnityEvent onActivatedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent onDeActivatedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent onTickedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent onPausedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent onResumedUnityEvent;

        #region Cache

        public UpdateTypeEnums UpdateType => updateType;

        public ActivationTypeEnums ActivationType => activationType;
        
        public bool IsComponentActive { get; private set; }
        public bool IsComponentPaused { get; private set; }
        #endregion

        #region Setup
        
        private void OnEnable()
        {
            NComponentManager.Instance.AddNComponent(this);
            if (activationType == ActivationTypeEnums.Enable)
                ActivateComponent();
        }

        private void OnDisable()
        {
            NComponentManager.Instance.RemoveNComponent(this);
            if (activationType == ActivationTypeEnums.Enable)
                DeActivateComponent();
        }

        private void Start()
        {
            if (activationType == ActivationTypeEnums.Start)
                ActivateComponent();
        }

        #endregion

        #region Abstracts
        public abstract void OnComponentActivated();
        public abstract void OnComponentDeActivated();
        public abstract void OnComponentTicked(float delta);

      
        public virtual void PauseComponent()
        {
            IsComponentPaused = true;
            onPausedUnityEvent?.Invoke();
        }

        public virtual void ResumeComponent()
        {
            IsComponentPaused = false;
            onResumedUnityEvent?.Invoke();
        }

        public virtual bool CanUpdateComponent()
        {
            if (!IsComponentActive)
                return false;

            if (IsComponentPaused)
                return false;
            return true;
        }
        #endregion

        #region Methods
        public void ActivateManuel()
        {
            if (activationType != ActivationTypeEnums.Manuel)
            {
                return;
            }
            ActivateComponent();
           
        }
       
        private void ActivateComponent()
        {
            if (IsComponentActive)
                return;
            
            IsComponentActive = true;
            OnComponentActivated();
            onActivatedUnityEvent?.Invoke();
        }

        private void DeActivateComponent()
        {
            if (!IsComponentActive)
                return;

          
            IsComponentActive = false;
            OnComponentDeActivated();
            onDeActivatedUnityEvent?.Invoke();
        }
        public void TickComponent(float delta)
        {
            OnComponentTicked(delta);
            onTickedUnityEvent?.Invoke();
        }

        #endregion
    }
}