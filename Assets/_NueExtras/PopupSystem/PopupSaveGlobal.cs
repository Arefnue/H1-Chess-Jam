using System.Text;
using _NueCore.SaveSystem;

namespace _NueExtras.PopupSystem
{
    public class PopupSaveGlobal : NBaseSave
    {
        #region Setup
        private static readonly StringBuilder Str = new StringBuilder();
        protected override string GetSavePath()
        {
            Str.Clear();
            Str.Append("Popup_Global");
            return Str.ToString();
        }
        public override void Save()
        {
            NSaver.SaveData<PopupSaveGlobal>();
        }
        public override void Load()
        {
            NSaver.GetSaveData<PopupSaveGlobal>();
        }
        public override void ResetSave()
        {
            NSaver.ResetSave<PopupSaveGlobal>();
        }

        public override SaveTypes GetSaveType()
        {
            return SaveTypes.Global;
        }

        #endregion
        
    }
}