using _NueCore.Common.ReactiveUtils;
using _NueCore.SaveSystem;
using UniRx;

namespace __Project.Systems.LevelSystem
{
    public static class LevelStatic
    {
        public static BoolReactiveProperty IsInteractionEnabled { get; set; } = new BoolReactiveProperty(false);

        public static LevelBase CurrentLevel { get; private set; }

        public static void SetCurrentLevel(LevelBase level)
        {
            CurrentLevel = level;
        }
        public static int GetCurrentLevelNumber()
        {
            var levelSave = NSaver.GetSaveData<LevelSave>();
            return levelSave.CompletedLevelCount+1;
        }
        public static void LevelUp()
        {
            var levelSave = NSaver.GetSaveData<LevelSave>();
            levelSave.CompletedLevelCount++;
            levelSave.Save();
            RBuss.Publish(new LevelREvents.LevelUppedREvent(levelSave.CompletedLevelCount));
        }

        public static void WinLevel()
        {
            var levelSave = NSaver.GetSaveData<LevelSave>();
            var level = levelSave.CompletedLevelCount;
            RBuss.Publish(new LevelREvents.LevelWonREvent(level));
        }

        public static void LoseLevel()
        {
            var levelSave = NSaver.GetSaveData<LevelSave>();
            var level = levelSave.CompletedLevelCount;
            RBuss.Publish(new LevelREvents.LevelLostREvent(level));

        }

    }
}