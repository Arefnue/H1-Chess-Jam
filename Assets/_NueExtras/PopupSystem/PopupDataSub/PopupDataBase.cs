using _NueCore.Common.Sandbox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueExtras.PopupSystem.PopupDataSub
{
    public abstract class PopupDataBase<TView> : NScriptableObject<PopupDataBase<TView>>
    {
        [SerializeField,BoxGroup("Core",Order = -1,CenterLabel = true),ReadOnly] private string popupID;
        [SerializeField,BoxGroup("Core"),AssetSelector] private TView popupPrefab;
       
        [PropertySpace(50)]
        
        #region Cache
        public string PopupID => popupID;
        public TView PopupPrefab => popupPrefab;
        #endregion

        #region Methods
        public abstract TView OpenPopup(Transform parent);
        
        public void DirectOpen()
        {
            OpenPopup(null);
        }
        
        public void DirectOpen(Transform spawnTransform)
        {
            OpenPopup(spawnTransform);
        }

        #endregion
        
        #region Editor
#if UNITY_EDITOR
        [Button,BoxGroup("Editor",Order = 99,CenterLabel = true)]
        private void OpenEditor()
        {
            OpenPopup(null);
        }
        
        [Button,BoxGroup("Editor")]
        public void SetID()
        {
            popupID = name;
        }
#endif
        #endregion
    }
}