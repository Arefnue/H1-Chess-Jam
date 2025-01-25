using System;
using System.Collections.Generic;
using _NueCore.Common.NueLogger;
using _NueCore.Common.Utility;
using UnityEngine;

namespace _NueCore.SaveSystem
{
    public static class NSaver
    {
        private static Dictionary<Type, NDiscWriter> WriterDict { get; set; }= new Dictionary<Type, NDiscWriter>();
        private static Dictionary<Type, NBaseSave> SaveDataDict { get; set; }= new Dictionary<Type, NBaseSave>();

        private static List<NBaseSave> ReflectedList { get; set; } = new List<NBaseSave>();

        #region Setup
        private static void InitData<T>() where T : NBaseSave
        {
            var typeCast = typeof(T);
            if (!WriterDict.ContainsKey(typeCast))
            {
                var tList = GetReflectedSaveList();
                var item = (T) tList.Find(x => x.GetType() == typeCast);
                if (item != null)
                {
                    var writer = item.GetWriter();
                    WriterDict.Add(typeCast,writer);
                }
                else
                {
                    $"There is no {typeof(T).Name}. Check reflection!!".NLog(Color.red);
                }
            }

            if (!SaveDataDict.ContainsKey(typeCast))
            {
                var writer = WriterDict[typeCast];
                var saveData = writer.ReadDisc<T>();
                if (saveData == null)
                {
                    saveData = (T)Activator.CreateInstance(typeof(T));
                    writer.WriteDisc(saveData);
                    saveData = writer.ReadDisc<T>();
                }
                SaveDataDict.Add(typeCast, saveData);
            }
        }
        
        public static void SaveAll()
        {
            var itemsList = GetReflectedSaveList();
            foreach (var saveData in itemsList)
                saveData.Save();
        }
        
        public static void LoadAll()
        {
            var itemsList = GetReflectedSaveList();
            foreach (var saveData in itemsList)
                saveData.Load();
        }
        
        public static void ResetAll()
        {
            var itemsList = GetReflectedSaveList();
            foreach (var saveData in itemsList)
            {
                saveData.ResetSave();
                $"{saveData.GetType().Name} RESET FROM EDITOR".NLog(Color.gray);
            }
        }
        #endregion

        #region Methods

        private static List<NBaseSave> GetReflectedSaveList()
        {
            if (ReflectedList.Count<=0)
            {
                var itemsList = ReflectionUtils.GetItemsList<NBaseSave>();
                foreach (var save in itemsList)
                    ReflectedList.Add(save);
            }

            return ReflectedList;
        }

        public static T GetSaveData<T>() where T : NBaseSave
        {
            var typeCast = typeof(T);
            if (!SaveDataDict.ContainsKey(typeCast))
                InitData<T>();
          
            return (T) SaveDataDict[typeCast];
        }
        public static void SaveData<T>() where T : NBaseSave
        {
            var typeCast = typeof(T);
            if (!WriterDict.ContainsKey(typeCast))
                InitData<T>();

            var data =GetSaveData<T>();
            WriterDict[typeCast].WriteDisc(data);
        }
        public static void ResetSave<T>() where T : NBaseSave
        {
            var typeCast = typeof(T);
            if (SaveDataDict.TryGetValue(typeCast, out var saveData))
            {
                var writer = saveData.GetWriter();
                writer.DeleteDisc();
                WriterDict.Remove(typeCast);
                SaveDataDict.Remove(typeCast);
                InitData<T>();
            }
            else
            {
                InitData<T>();
            }
           
        }

        public static void ResetSaveByType(SaveTypes saveType)
        {
            List<NBaseSave> tempList = new List<NBaseSave>();
            foreach (var saveData in SaveDataDict)
            {
                if (saveData.Value.GetSaveType() != saveType)
                    continue;
                tempList.Add(saveData.Value);
            }

            if (tempList.Count<=0) return;
            foreach (var saveData in tempList)
                saveData.ResetSave();
        }
        #endregion
    }
}