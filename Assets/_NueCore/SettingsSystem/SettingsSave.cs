using _NueCore.SaveSystem;

namespace _NueCore.SettingsSystem
{
    public class SettingsSave : NBaseSave
    {
        #region Setup

        public override SaveTypes GetSaveType()
        {
            return SaveTypes.Global;
        }

        protected override string GetSavePath()
        {
            return "Settings";
        }

        public override void Save()
        {
            NSaver.SaveData<SettingsSave>();
        }

        public override void Load()
        {
            NSaver.GetSaveData<SettingsSave>();
        }

        public override void ResetSave()
        {
            NSaver.ResetSave<SettingsSave>();
        }

        #endregion


        public int qualityIndex =-1;
        public int resolutionIndex = -1;
        public int textureQualityIndex = -1;
        public int aaIndex = -1;
        public bool isFullscreen;
        public float masterVolume = 100;
        public float musicVolume = 100;
        public float sfxVolume = 100;
        public bool isHapticActive = true;
    }
}