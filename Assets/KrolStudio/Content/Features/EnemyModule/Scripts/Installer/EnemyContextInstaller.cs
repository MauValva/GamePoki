using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class EnemyContextInstaller : MonoInstaller<EnemyContextInstaller>
    {
        public override void InstallBindings()
        {
            // StateMachine
            EnemyStateMachineInstaller.Install(Container);
            Container.Bind<EnemyStatesFactory>().AsSingle();

            // Config
            var _addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            Container.BindInterfacesAndSelfTo<EnemyConfig>()
               .FromScriptableObject(_addressablesAssetLoaderService.LoadAsset<EnemyConfig>(Address.Configurations.EnemyConfig))
               .AsSingle();

            // Health bar
            Container.Bind<HealthBarView>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyHealthBarPresenter>().AsSingle();

            // Stats
            Container.Bind<EnemyStatsModel>().AsSingle();
            Container.Bind<EnemyStatsCalculator>().AsSingle();

            // Dependencies
            Container.Bind<IEnemyAnimator>().To<EnemyAnimator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemyEffects>().To<EnemyEffects>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IEnemyRagdoll>().To<EnemyRagdoll>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyDamageable>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyAudioController>().AsSingle();


            Container.BindInterfacesAndSelfTo<EnemyTransformModel>().AsSingle();
            Container.Bind<EnemyChaseTransitionChecker>().AsSingle();
            Container.Bind<EnemySpawnPointModel>().AsSingle();
            Container.Bind<EnemyMover>().AsSingle();
            Container.Bind<EnemyAttackHandler>().AsSingle();
        }
    }
}