using System;

namespace KrolStudio
{
    [Serializable]
    public class SettingsData : IDataModel
    {
        public bool Vibrate;
        public float MusicVolume;
        public float SfxVolume;

        public SettingsData(bool vibrate, float musicVolume, float sfxVolume)
        {
            Vibrate = vibrate;
            MusicVolume = musicVolume;
            SfxVolume = sfxVolume;
        }
    }
}