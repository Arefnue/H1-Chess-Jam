using _NueCore.Common.ReactiveUtils;

namespace __Project.Systems.LevelSystem
{
    public abstract class LevelREvents
    {
       public class LevelSpawnedREvent : REvent
       {
           public LevelBase LevelBase { get; private set; }
           public LevelSpawnedREvent(LevelBase levelBase)
           {
               LevelBase = levelBase;
           }
       }
       
       public class LevelUppedREvent : REvent
       {
           public int Level { get; private set; }
           public LevelUppedREvent(int level)
           {
               Level = level;
           }
       }
       
         public class LevelWonREvent : REvent
         {
              public int Level { get; private set; }
              public LevelWonREvent(int level)
              {
                Level = level;
              }
         }
         
            public class LevelLostREvent : REvent
            {
                public int Level { get; private set; }
                public LevelLostREvent(int level)
                {
                    Level = level;
                }
            }
    }
}