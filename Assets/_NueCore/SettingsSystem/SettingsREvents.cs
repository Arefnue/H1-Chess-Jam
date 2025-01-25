using _NueCore.Common.ReactiveUtils;

namespace _NueCore.SettingsSystem
{
    public abstract class SettingsREvents
    {
        public class AbandonButtonClickedREvent : REvent
        {
            public AbandonButtonClickedREvent()
            {
                
            }
        }
        
        public class ReturnToMainMenuButtonClickedREvent : REvent
        {
            
        }
        
        public class SaveAndQuitButtonClickedREvent : REvent
        {
            
        }

        
        public class SettingsClosedREvent : REvent
        {
            
        }
        
        public class SettingsOpenedREvent : REvent
        {
            
        }
    }
}