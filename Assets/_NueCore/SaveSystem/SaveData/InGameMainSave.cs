namespace _NueCore.SaveSystem
{
    public class InGameMainSave : NBaseSave
    {
        public override SaveTypes GetSaveType()
        {
            return SaveTypes.InGame;
        }

        protected override string GetSavePath()
        {
            return "IngameMain";
        }

        public override void Save()
        {
            NSaver.SaveData<InGameMainSave>();
        }

        public override void Load()
        {
            NSaver.GetSaveData<InGameMainSave>();
        }
        
        public override void ResetSave()
        {
            NSaver.ResetSave<InGameMainSave>();
        }
        
       
       
    }
}