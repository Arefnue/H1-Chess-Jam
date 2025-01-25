using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueExtras.PopupSystem.PopupDataSub
{
    [CreateAssetMenu(fileName = "Popup_Display_X", menuName = "PopupSystem/PopupData/Display", order = 0)]
    public class PopupDataDisplay : PopupDataBase<PopupDisplay>
    {
        [SerializeField,TabGroup("Tabs","Custom")] private bool hideCloseButton;
        public override PopupDisplay OpenPopup(Transform parent = null)
        {
            if (PopupPrefab == null) return null;
           
            var pop =Instantiate(PopupPrefab,parent);
            pop.Data = this;
            pop.OpenPopup();
            if (hideCloseButton)
                pop.HideCloseButton();
            else
                pop.ShowCloseButton();
            return pop;
        }
    }
}