using _NueCore.ManagerSystem;
using _NueCore.ManagerSystem.Core;

namespace _NueExtras.PopupSystem
{
    public class PopupManager : NManagerBase
    {
        public override void NAwake()
        {
            InitSingleton<PopupManager>();
            base.NAwake();
        }
    }
}