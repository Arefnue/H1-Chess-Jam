using _NueCore.ManagerSystem.SingletonSystems;

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
    }
}