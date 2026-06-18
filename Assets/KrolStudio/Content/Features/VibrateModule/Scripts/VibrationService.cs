using UnityEngine;

namespace KrolStudio
{
    public class VibrationService : IVibrationService
    {
#if !UNITY_EDITOR
#if UNITY_IOS
    [DllImport ( "__Internal" )]
    private static extern bool _HasVibrator ();

    [DllImport ( "__Internal" )]
    private static extern void _Vibrate ();

    [DllImport ( "__Internal" )]
    private static extern void _VibratePop ();

    [DllImport ( "__Internal" )]
    private static extern void _VibratePeek ();

    [DllImport ( "__Internal" )]
    private static extern void _VibrateNope ();
#endif

#if UNITY_ANDROID
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    public static AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
#endif
#endif

        bool _vibrate;

        private bool IsMobile() =>
                Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        public void Vibrate()
        {
            if (!_vibrate)
                return;

#if UNITY_IOS || UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        public void Vibrate(long milliseconds)
        {
            if (!(IsMobile() && _vibrate)) return;

#if !UNITY_EDITOR
#if UNITY_ANDROID
        vibrator.Call("vibrate", milliseconds);
#elif UNITY_IOS
        _Vibrate();
#endif
#endif
        }

        public void Cancel()
        {
            if (!IsMobile()) return;

#if !UNITY_EDITOR
#if UNITY_ANDROID
        vibrator.Call("cancel");
#endif
#endif
        }

        public void SetVibrate(bool value) =>
            _vibrate = value;

        public bool GetVibrate() => _vibrate;
    }
}