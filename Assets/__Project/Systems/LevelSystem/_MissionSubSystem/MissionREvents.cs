using _NueCore.Common.ReactiveUtils;

namespace __Project.Systems.LevelSystem._MissionSubSystem
{
    public abstract class MissionREvents
    {
        public class ActiveStepSelectedREvent : REvent
        {
            public MissionStep MissionStep { get; }
            public ActiveStepSelectedREvent(MissionStep missionStep)
            {
                MissionStep = missionStep;
            }
        }
        public class ActiveMissionSelectedREvent : REvent
        {
            public MissionInfo MissionInfo { get; }
            public ActiveMissionSelectedREvent(MissionInfo missionInfo)
            {
                MissionInfo = missionInfo;
            }
        }
        
        public class StepCompletedREvent : REvent
        {
            public MissionStep MissionStep { get; }
            
            public StepCompletedREvent(MissionStep missionStep)
            {
                MissionStep = missionStep;
            }
        }
        
        public class MissionCompletedREvent : REvent
        {
            public MissionInfo MissionInfo { get; }
            public MissionCompletedREvent(MissionInfo info)
            {
                MissionInfo = info;
            }
        }
    }
}