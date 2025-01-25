using _NueCore.Common.ReactiveUtils;

namespace _NueExtras.CheatSystem
{
    public abstract class CheatREvents
    {
        public class CheatOpenChoiceREvent : REvent
        {
            
        }

        public class EnableCheatREvent : REvent
        {
            public EnableCheatREvent()
            {
                
            }
        }
        
        public class DisableCheatREvent : REvent
        {
        }
    }
}