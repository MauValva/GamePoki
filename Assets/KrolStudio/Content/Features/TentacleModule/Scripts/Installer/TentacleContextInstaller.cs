using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class TentacleContextInstaller : MonoInstaller<EnemyContextInstaller>
    {
        public override void InstallBindings()
        {
            // StateMachine
            TentacleStateMachineInstaller.Install(Container);
            Container.Bind<TentacleStatesFactory>().AsSingle();

            // Config
            var _addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.BindInterfacesAndSelfTo<TentacleConfig>()
               .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<TentacleConfig>(Address.Configurations.TentacleConfig))
               .AsSingle();

            // Health bar
            Container.Bind<HealthBarView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyHealthBarPresenter>().AsSingle();

            // Dependencies
            Container.Bind<ITentacleAnimator>().To<TentacleAnimator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemyEffects>().To<EnemyEffects>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyDamageable>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<TentacleAudioController>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyTransformModel>().AsSingle();
            Container.Bind<EnemySpawnPointModel>().AsSingle();
            Container.Bind<TentacleTriggerChecker>().AsSingle();
            Container.Bind<TentacleRotator>().AsSingle();
            Container.Bind<TentacleAttackHandler>().AsSingle();

            Container.Bind<EnemyStatsModel>().AsSingle();
            Container.Bind<EnemyStatsCalculator>().AsSingle();
        }
    }
}