using System;
using System.Collections.Generic;
using System.Text;
using _NueCore.SaveSystem;
using UnityEngine;

namespace __Project.Systems.PowerUpSystem
{
    public class PowerUpSave : NBaseSave
    {
        #region Setup
        private static readonly StringBuilder Str = new StringBuilder();
        protected override string GetSavePath()
        {
            Str.Clear();
            Str.Append("PowerUpSave");
            return Str.ToString();
        }
        public override void Save()
        {
            NSaver.SaveData<PowerUpSave>();
        }
        public override void Load()
        {
            NSaver.GetSaveData<PowerUpSave>();
        }
        public override void ResetSave()
        {
            NSaver.ResetSave<PowerUpSave>();
        }

        public override SaveTypes GetSaveType()
        {
            return SaveTypes.Global;
        }

        #endregion

        [Serializable]
        public class PowerUpInfo
        {
            public string id;
            public bool isUnlocked;
            public int remainingCount;
        }

        public List<PowerUpInfo> PowerUpInfoList = new List<PowerUpInfo>();

        public PowerUpInfo GetPowerUpInfo(string id)
        {
            var info = PowerUpInfoList.Find(x => x.id == id);
            if (info == null)
            {
                info = new PowerUpInfo
                {
                    id = id,
                    remainingCount = 1
                };
                SetPowerUpInfo(info);
            }

            return info;
        }
        public void SetPowerUpInfo(PowerUpInfo info)
        {
            var oldInfo =PowerUpInfoList.Find(x => x.id == info.id);
            if (oldInfo != null)
            {
                PowerUpInfoList.Remove(oldInfo);
               
            }
            
            PowerUpInfoList.Add(info);
            
        }
    }
}