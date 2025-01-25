using System;

namespace __Project.Systems.LevelSystem._LevelEndSubSystem
{
    public struct LevelEndInfo
    {
        public int Level;
        public int CoinReward;
        public int ColorReward;
        public Action OnContinueButtonClickedAction;
    }
}