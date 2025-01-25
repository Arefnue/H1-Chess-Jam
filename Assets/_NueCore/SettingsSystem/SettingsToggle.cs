using System;
using _NueCore.Common.NueLogger;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _NueCore.SettingsSystem
{
    public class SettingsToggle : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private Image activeImage;
        [SerializeField] private Image deActiveImage;

        public bool IsActive { get; private set; }
        public Action<bool> ChangeAction;

        public void Activate()
        {
            IsActive = true;
            deActiveImage.gameObject.SetActive(false);
            activeImage.gameObject.SetActive(true);
            ChangeAction?.Invoke(true);
        }
        
        public void DeActivate()
        {
            IsActive = false;
            deActiveImage.gameObject.SetActive(true);
            activeImage.gameObject.SetActive(false);
            ChangeAction?.Invoke(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsActive)
            {
                DeActivate();
            }
            else
            {
                Activate();
            }
        }
    }
}