using _NueCore.ManagerSystem.SingletonSystems;
using _NueExtras.HapticSystem;
using Lofelt.NiceVibrations;
using Unity.VisualScripting;

namespace _NueCore.AudioSystem
{
    public static class AudioStatic
    {
        private static AudioManager Manager => SingletonExtension.Get<AudioManager>();
        
        public static void PlayFx(AudioData audioData)
        {
           audioData.Play();
        }
 
        public static void PlayFx(DefaultAudioDataTypes dataType)
        {
            var data =Manager.GetDefaultAudioData(dataType);
            if (data)
                data.Play();
        }
        
         public static void PlayHaptic(HapticPatterns.PresetType hapticType)
         {
             var hapticManager = HapticManager.Instance;
            if (!hapticManager) return;
            hapticManager.GiveHaptic(hapticType);
//
// #if UNITY_ANDROID || UNITY_EDITOR
//             hapticManager.GiveHaptic(hapticType);
// #endif
//
// #if UNITY_IOS 
//             switch (hapticType)
//             {
//                 case HapticPatterns.PresetType.Success:
//                     hapticManager.GiveHaptic(hapticType);
//                     break;
//                 case HapticPatterns.PresetType.Failure:
//                     hapticManager.GiveHaptic(hapticType);
//                     break;
//                 case HapticPatterns.PresetType.SoftImpact:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.HeavyImpact);
//                     break;
//                 case HapticPatterns.PresetType.Selection:
//                     hapticManager.GiveHaptic(hapticType);
//                     break;
//                 case HapticPatterns.PresetType.Warning:
//                     hapticManager.GiveHaptic(hapticType);
//                     break;
//                 case HapticPatterns.PresetType.LightImpact:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.MediumImpact);
//                     break;
//                 case HapticPatterns.PresetType.MediumImpact:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.HeavyImpact);
//                     break;
//                 case HapticPatterns.PresetType.HeavyImpact:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.Success);
//                     break;
//                 case HapticPatterns.PresetType.RigidImpact:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.HeavyImpact);
//                     break;
//                 case HapticPatterns.PresetType.None:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.SoftImpact);
//                     break;
//                 default:
//                     hapticManager.GiveHaptic(HapticPatterns.PresetType.SoftImpact);
//                     break;
//             }
// #endif
        }
    }
}