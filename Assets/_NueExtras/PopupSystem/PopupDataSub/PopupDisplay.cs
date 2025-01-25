
using _NueCore.AudioSystem;

namespace _NueExtras.PopupSystem.PopupDataSub
{
    public class PopupDisplay : PopupBase<PopupDataDisplay>
    {
        public override void OpenPopup()
        {
            //AudioStatic.PlayFx(DefaultAudioDataTypes.OpenPanel);
            base.OpenPopup();
        }

        public override void ClosePopup()
        {
            //AudioStatic.PlayFx(DefaultAudioDataTypes.ClosePanel);
            base.ClosePopup();
        }
    }
}