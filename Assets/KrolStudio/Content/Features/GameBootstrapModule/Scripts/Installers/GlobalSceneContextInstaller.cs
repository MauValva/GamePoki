using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(GlobalSceneContextInstaller),
    fileName = nameof(GlobalSceneContextInstaller), order = 0)]
    public class GlobalSceneContextInstaller : ScriptableObjectInstaller<GlobalSceneContextInstaller>
    {
        public override void InstallBindings()
        {
            PrefabSpawnerInstaller.Install(Container);

            PersistanceModuleInstaller.Install(Container);

            CameraInstaller.Install(Container);

            PlayerStateMachineInstaller.Install(Container);

            GameplayInstaller.Install(Container);

            WalletInstaller.Install(Container); // WalletInstaller must run after PersistenceModuleInstaller; otherwise, RequestWalletSignal will not work. 

			PauseInstaller.Install(Container);

            VibrationInstaller.Install(Container);

            CullingInstaller.Install(Container);
        }
    }
}