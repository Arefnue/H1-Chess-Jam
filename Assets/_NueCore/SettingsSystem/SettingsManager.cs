using _NueCore.ManagerSystem;
using _NueCore.ManagerSystem.Core;
using UnityEngine;

namespace _NueCore.SettingsSystem
{
    public class SettingsManager : NManagerBase
    {
        [SerializeField] private SettingsUIController settingsUIController;
        
        #region Cache
        public static SettingsManager Instance { get; private set; }

        #endregion

        #region Setup
        public override void NAwake()
        {
            Instance =InitSingleton<SettingsManager>();
            base.NAwake();
            settingsUIController.LoadSettings();
        }

        public override void NStart()
        {
            base.NStart();
        }

        #endregion
    }
}