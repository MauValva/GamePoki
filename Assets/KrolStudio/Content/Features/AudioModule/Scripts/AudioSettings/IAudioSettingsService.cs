
namespace KrolStudio
{
    public interface IAudioSettingsService
    {
        float GetMusicVolume();
        float GetSfxVolume();
        void SetMusicVolume(float value);
        void SetSfxVolume(float value);
    }
}