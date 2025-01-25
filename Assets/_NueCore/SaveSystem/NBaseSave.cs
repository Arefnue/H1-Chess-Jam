using System;

namespace _NueCore.SaveSystem
{
    [Serializable]
    public abstract class NBaseSave
    {
        protected NDiscWriter Writer { get; set; }
        protected abstract string GetSavePath();
        public NDiscWriter GetWriter()
        {
            var path = GetSavePath();
#if UNITY_EDITOR
            path += "_EDITOR";
#endif
            return Writer ??= new NDiscWriter(path);
        }
        public abstract void Save();
        public abstract void Load();
        public abstract void ResetSave();
        public abstract SaveTypes GetSaveType();
    }
}