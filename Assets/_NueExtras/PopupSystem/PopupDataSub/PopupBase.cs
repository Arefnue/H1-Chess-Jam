using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _NueExtras.PopupSystem.PopupDataSub
{
    public abstract class PopupBase<TData> : MonoBehaviour
    {
        [SerializeField,TabGroup("References")] private Button closeButton;
       
        [SerializeField,TabGroup("Events")] private UnityEvent popupOpenedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent popupClosedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent popupRefreshedUnityEvent;
      
        #region Cache
        public bool DestroyOnClose { get; set; } = true;
        public Button CloseButton => closeButton;
        private bool _isOpened;
        private bool _isClosed;
        
        public TData Data { get; set; }
        #endregion
        
        #region Actions
        public Action PopupOpenedAction { get;  set; }
        public Action PopupClosedAction  { get;  set; }
        public Action RefreshedAction  { get;  set; }
        #endregion

        #region Setup
        protected virtual void Awake()
        {
            if (CloseButton)
            {
                CloseButton.onClick.AddListener(ClosePopup);
                CloseButton.gameObject.SetActive(false);
            }
        }
        protected virtual void Start()
        {
            
        }
        protected virtual void OnEnable()
        {
            
        }
        protected virtual void OnDisable()
        {
            ClearPopup();
        }
        #endregion

        #region Global Methods
        public virtual void OpenPopup()
        {
            if (_isOpened) return;
            _isOpened = true;
            popupOpenedUnityEvent?.Invoke();
            PopupOpenedAction?.Invoke();
        }
        public virtual void ClosePopup()
        {
            if (_isClosed) return;
            _isClosed = true;
            
            ClearPopup();
            popupClosedUnityEvent?.Invoke();
            PopupClosedAction?.Invoke();
            
            if (DestroyOnClose)
                DestroyPop();
            else
                HidePopup();
        }
        
        public void ShowCloseButton()
        {
            if(CloseButton)
                CloseButton.gameObject.SetActive(true);
        }
        public void HideCloseButton()
        {
            if (CloseButton)
                CloseButton.gameObject.SetActive(false);
           
        }
        public void AddCloseButtonClickAction(UnityAction action)
        {
            if (!CloseButton) return;
            
            CloseButton.onClick.AddListener(action);
        }
        private void HidePopup()
        {
            transform.localScale = Vector3.zero;
        }
        
        private void ShowPopup()
        {
            transform.localScale = Vector3.one;
        }
        
        public void DestroyPop()
        {
            Destroy(gameObject);
        }

        public virtual void Refresh()
        {
            popupRefreshedUnityEvent?.Invoke();
           RefreshedAction?.Invoke();
        }
        #endregion

        #region Local Methods
        protected virtual void ClearPopup()
        {
        }
        #endregion
    }
}