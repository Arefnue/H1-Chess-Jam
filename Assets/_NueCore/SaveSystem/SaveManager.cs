using _NueCore.ManagerSystem;
using _NueCore.ManagerSystem.Core;

namespace _NueCore.SaveSystem
{
    public class SaveManager : NManagerBase
    {
        public static SaveManager Instance { get; private set; }
        private bool _isSaved;
        public override void NAwake()
        {
            Instance = InitSingleton<SaveManager>();
            base.NAwake();
            NSaver.LoadAll();
            var saveData = NSaver.GetSaveData<GlobalMainSave>();
            var savedIndex = saveData.activeSaveIndex;
            var hasSave = saveData.hasSave;
            SaveStatic.SetHasSave(hasSave);
            SaveStatic.SetActiveSaveIndex(savedIndex,true);
        }

        public override void NStart()
        {
            base.NStart();
            SaveStatic.Load();
        }

        private void OnApplicationQuit()
        {
            if (!_isSaved)
            {
                _isSaved = true;
                SaveStatic.Save();
                NSaver.SaveAll();
            }
           
        }
    }
}