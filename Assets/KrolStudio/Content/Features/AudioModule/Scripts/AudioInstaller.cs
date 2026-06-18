using UnityEngine;
using Zenject;
using UnityEngine.Audio;
using Global.Scripts.Generated;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/AudioModule/" + nameof(AudioInstaller),
    fileName = nameof(AudioInstaller), order = 0)]
    public class AudioInstaller : ScriptableObjectInstaller<AudioInstaller>
    {
        [SerializeField] AudioMixer audioMixer;

        public override void InstallBindings()
        {
            Container.Bind<AudioMixer>().FromInstance(audioMixer).AsSingle();
            Container.Bind<IAudioSettingsService>().To<AudioSettingsService>().AsSingle();
           
            // Config
            var _addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.BindInterfacesAndSelfTo<AudioLibrary>()
               .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<AudioLibrary>(Address.Configurations.AudioLibrary))
               .AsSingle();

            Container.BindInterfacesTo<MusicController>().AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        }
    }
}