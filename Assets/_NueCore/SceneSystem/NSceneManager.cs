using _NueCore.ManagerSystem;
using _NueCore.ManagerSystem.Core;


namespace _NueCore.SceneSystem
{
    public class NSceneManager : NManagerBase
    {
        public static NSceneManager Instance { get; private set; }
        public override void NAwake()
        {
            Instance = InitSingleton<NSceneManager>();
            base.NAwake();
        }
    }
}