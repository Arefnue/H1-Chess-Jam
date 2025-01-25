using System.Text;
using _NueCore.SaveSystem;

namespace __Project.Systems.LevelSystem
{
    public class LevelSave : NBaseSave
    {
        #region Setup
        private static readonly StringBuilder Str = new StringBuilder();
        protected override string GetSavePath()
        {
            Str.Clear();
            Str.Append("LevelSave");
            return Str.ToString();
        }
        public override void Save()
        {
            NSaver.SaveData<LevelSave>();
        }
        public override void Load()
        {
            NSaver.GetSaveData<LevelSave>();
        }
        public override void ResetSave()
        {
            NSaver.ResetSave<LevelSave>();
        }

        public override SaveTypes GetSaveType()
        {
            return SaveTypes.Global;
        }

        #endregion

        public int CompletedLevelCount;

    }
}