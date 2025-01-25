using UnityEditor;
using UnityEngine;

namespace _NueCore.SaveSystem.Editor
{
    public class SaveEditorMenu
    {
        [MenuItem("NueCore/Save/ResetAll")]
        public static void ResetAllSave()
        {
            NSaver.ResetAll();
        }
    }
}