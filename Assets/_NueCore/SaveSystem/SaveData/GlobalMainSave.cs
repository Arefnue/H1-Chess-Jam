namespace _NueCore.SaveSystem
{
    public class GlobalMainSave : NBaseSave
    {
        #region Setup

        public override SaveTypes GetSaveType()
        {
            return SaveTypes.Global;
        }

        protected override string GetSavePath()
        {
            return  "GlobalMain";
        }

        public override void Save()
        {
            NSaver.SaveData<GlobalMainSave>();
        }

        public override void Load()
        {
            NSaver.GetSaveData<GlobalMainSave>();
        }
        
        public override void ResetSave()
        {
            NSaver.ResetSave<GlobalMainSave>();
        }

        #endregion

        public int activeSaveIndex = 0;
        public bool hasSave;
    }
}