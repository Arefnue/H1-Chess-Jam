using System.Collections.Generic;
using System.Linq;
using _NueCore.Common.NueLogger;
using _NueCore.Common.Utility;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _NueCore.AudioSystem
{
    [CreateAssetMenu(fileName = "Audio Catalog", menuName = "Data/AudioSystem/Catalog", order = 0)]
    public class AudioCatalog : ScriptableObject
    {
        [SerializeField] private List<AudioData> audioDataList;
        [SerializeField] private string audioDataPath;

        public List<AudioData> AudioDataList => audioDataList;
        
        public AudioData GetAudioData(string id)
        {
            var targetData = audioDataList.FirstOrDefault(x => x.name == id);
            if (targetData == null)
            {
                $"There is no {id} audio data!!!!".NLog(Color.red);
            }
            return targetData;
        }

#if UNITY_EDITOR
        [Button,FoldoutGroup("Editor")]
        private void FindAudioData()
        {
            if (string.IsNullOrEmpty(audioDataPath) || string.IsNullOrWhiteSpace(audioDataPath)) return;
            
            audioDataList = AssetHelper.FindAllAssets(audioDataPath,audioDataList);
            
            // var assets = AssetDatabase.FindAssets($"t:{nameof(AudioData)}", new[] {audioDataPath});
            // audioDataList.Clear();
            // foreach (var asset in assets)
            // {
            //     var targetPath = AssetDatabase.GUIDToAssetPath(asset);
            //     var loadedAsset = (AudioData)AssetDatabase.LoadAssetAtPath(targetPath,typeof(AudioData));
            //    
            //     audioDataList.Add(loadedAsset);
            // }
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
        }
#endif
    }
}