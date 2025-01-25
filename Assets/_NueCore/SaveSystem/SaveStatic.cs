using _NueCore.Common.ReactiveUtils;

namespace _NueCore.SaveSystem
{
    public static class SaveStatic
    {
        public static int ActiveSaveIndex { get; private set; } = 0;
        public static bool HasSave { get; private set; }
        
        public static void SetActiveSaveIndex(int index,bool ignoreSave = false)
        {
            var oldIndex = ActiveSaveIndex;
            ActiveSaveIndex = index;
            if (ignoreSave)
                return;
            var saveData =NSaver.GetSaveData<GlobalMainSave>();
            saveData.activeSaveIndex = index;
            saveData.Save();
            RBuss.Publish(new SaveREvents.SaveIndexChangedREvent(oldIndex,index));
        }

        public static void SetHasSave(bool status)
        {
            HasSave = status;
            var saveData =NSaver.GetSaveData<GlobalMainSave>();
            saveData.hasSave = status;
            saveData.Save();
        }

        public static void Save()
        {
            RBuss.Publish(new SaveREvents.SavedREvent());
        }

        public static void Load()
        {
            RBuss.Publish(new SaveREvents.LoadREvent());
        }
    }
}