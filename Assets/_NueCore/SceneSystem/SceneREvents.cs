using _NueCore.Common.ReactiveUtils;

namespace _NueCore.SceneSystem
{
    public abstract class SceneREvents
    {
        public class SceneChangeStartedREvent : REvent
        {
            public string NewSceneName { get; private set; }
            public SceneChangeStartedREvent(string newSceneName)
            {
                NewSceneName = newSceneName;
            }
        }
        
        public class SceneChangeFinishedREvent : REvent
        {
            public string NewSceneName { get; private set; }
            public SceneChangeFinishedREvent(string newSceneName)
            {
                NewSceneName = newSceneName;
            }
        }
    }
}