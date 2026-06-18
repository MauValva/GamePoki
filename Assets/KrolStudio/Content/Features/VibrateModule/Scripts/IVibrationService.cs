
namespace KrolStudio
{
    public interface IVibrationService
    {
        void SetVibrate(bool value);
        bool GetVibrate();
        void Vibrate(long milliseconds);
    }
}