using System.Collections.Generic;
using _NueCore.Common.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _NueCore.AudioSystem
{
    [CreateAssetMenu(fileName = "AudioData_X", menuName = "Data/AudioSystem/AudioData", order = 0)]
    public class AudioData : ScriptableObject
    {
        [SerializeField,ReadOnly] private string audioID;
        [SerializeField] private AudioSourceTypes sourceType;
        [SerializeField] private List<AudioClip> targetClipList;
        [SerializeField] private bool playRandomly;

        #region Cache
        
        public AudioSourceTypes SourceType => sourceType;
        
        public string AudioID => audioID;

        private int _lastPlayedIndex;

        #endregion
        
        public void Play()
        {
            if (playRandomly)
            {
                AudioManager.Instance.PlayOneShot(targetClipList.RandomItem(),sourceType);
                return;
            }

            var clip =targetClipList[_lastPlayedIndex];
            AudioManager.Instance.PlayOneShot(clip,sourceType);
            _lastPlayedIndex++;
            if (_lastPlayedIndex>=targetClipList.Count)
                _lastPlayedIndex = 0;
        }

#if UNITY_EDITOR
        [Button,FoldoutGroup("Editor")]
        private void SetID()
        {
            audioID = name;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
#endif
    }
}