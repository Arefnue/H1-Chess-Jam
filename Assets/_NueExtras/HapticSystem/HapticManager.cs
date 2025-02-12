using _NueCore.Common.KeyValueDict;
using _NueCore.ManagerSystem.Core;
using _NueCore.SaveSystem;
using _NueCore.SettingsSystem;
using Lofelt.NiceVibrations;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

namespace _NueExtras.HapticSystem
{
    public class HapticManager : NManagerBase
    {
        [SerializeField] private KeyValueDict<HapticPatterns.PresetType, HapticClip> hapticClipDict =
            new KeyValueDict<HapticPatterns.PresetType, HapticClip>();

        public bool IsDisable { get; set; }
        private bool _canGive = true;

        public static HapticManager Instance { get; private set; }
        public override void NAwake()
        {
            Instance = InitSingleton<HapticManager>();
            base.NAwake();
            SettingsUIController.IsHapticActive.Subscribe(status =>
            {
                IsDisable = !status;
            }).AddTo(gameObject);
          
        }

        public override void NStart()
        {
            base.NStart();
            var save = NSaver.GetSaveData<SettingsSave>();
            IsDisable = !save.isHapticActive;
        }

        private bool SetEnableHaptic()
        {
            if (!_canGive) return false;
            _canGive = false;
            Invoke(nameof(SetCanGiveTrue), .5f);
            return true;
        }

        private void SetCanGiveTrue()
        {
            _canGive = true;
        }

        public void GiveHaptic(HapticPatterns.PresetType hapticType)
        {
            if (IsDisable) return;
            if (hapticType is HapticPatterns.PresetType.None)
            {
                return;
            }
            if (SetEnableHaptic())
            {
                HapticController.fallbackPreset = hapticType;
                HapticController.Play(hapticClipDict.TryGetValue(hapticType, out var value)
                    ? value
                    : hapticClipDict[HapticPatterns.PresetType.SoftImpact]);
            }
        }

    }
}