using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __Project.Systems.LevelSystem._MissionSubSystem
{
    public class MissionStep : MonoBehaviour
    {
        [SerializeField] private float imageRevealY = 2f;
        [SerializeField] private List<MissionInfo> missionList = new List<MissionInfo>();
        public List<MissionInfo> MissionList => missionList;
        public int CurrentMissionIndex { get; private set; }
        public float ImageRevealY => imageRevealY;
        public void Build()
        {
            CurrentMissionIndex = 0;
            foreach (var missionInfo in missionList)
                missionInfo.SetTargetSpriteAsInactive();
        }
        public void CompleteMission()
        {
            CurrentMissionIndex++;
        }
        public bool TryGetCurrentMission(out MissionInfo info)
        {
            info = null;
            if (CurrentMissionIndex < missionList.Count)
            {
                info = missionList[CurrentMissionIndex];
                return true;
            }
            return false;
        }

#if UNITY_EDITOR
        [Button]
        private void FindAllSprites(bool apply)
        {
            if (!apply)
            {
                return;
            }
            missionList.Clear();
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                var missionInfo = new MissionInfo
                {
                    targetSprite = sprite
                };
                missionList.Add(missionInfo);
            }
        }
#endif
    }
}