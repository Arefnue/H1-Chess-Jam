
using _NueCore.ManagerSystem.Core;

namespace _NueCore.ManagerSystem
{
    public class ExampleManager : NManagerBase
    {
        public static ExampleManager Instance { get; private set; }
        
        public override void NAwake()
        {
            base.NAwake();
            Instance = InitSingleton<ExampleManager>();
        }
    }
}