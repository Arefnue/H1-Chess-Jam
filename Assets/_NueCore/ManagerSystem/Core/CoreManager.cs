using DG.Tweening;

namespace _NueCore.ManagerSystem.Core
{
    public class CoreManager : NManagerBase
    {
        public static CoreManager Instance { get; private set; }
        public override void NAwake()
        {
            Instance = InitSingleton<CoreManager>();
            base.NAwake();
            DOTween.SetTweensCapacity(300,100);
            
        }
    }
}