using _NueCore.Common.ReactiveUtils;

namespace _NueCore.SaveSystem
{
    public abstract class SaveREvents
    {
        public class SaveIndexChangedREvent : REvent
        {
            public int OldIndex { get; private set; }
            public int NewIndex { get; private set; }
            
            public SaveIndexChangedREvent(int oldIndex, int newIndex)
            {
                OldIndex = oldIndex;
                NewIndex = newIndex;
            }
        }
        
        public class SavedREvent : REvent
        {
            
        }
        
        public class LoadREvent : REvent
        {
            
        }
    }
}