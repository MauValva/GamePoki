using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class GameplayPoolsInitializer : IInitializable
    {
        private readonly IInteractorPoolContainer _interactorPoolContainer;
        private readonly GameplayPoolsConfig _config;
        private readonly IPrefabsFactory _prefabsFactory;

        public GameplayPoolsInitializer(
            IInteractorPoolContainer interactorPoolContainer,
            GameplayPoolsConfig config,
            IPrefabsFactory prefabsFactory)
        {
            _interactorPoolContainer = interactorPoolContainer;
            _config = config;
            _prefabsFactory = prefabsFactory;
        }

        public void Initialize()
        {
            CreatePoolContainers();
        }

        private void CreatePoolContainers()
        {
            CreateProjectilePoolContainer();
            CreateEnemyPoolContainer();
            CreateDamagePopupPoolContainer();
            CreateEnemyKillRewardTextPoolContainer();
            CreateTentaclePoolContainer();
            CreateSoundEmitterPoolContainer();
        }

        private void CreateSoundEmitterPoolContainer()
        {
            var factory = new ObjectFactory<SoundEmitter>(_prefabsFactory, Address.Prefabs.SoundEmitter);
            _interactorPoolContainer.CreatePool<SoundEmitter, ObjectFactory<SoundEmitter>>(factory, _config.initialSoundEmitterPoolCount, Address.Prefabs.SoundEmitter, true);
        }

        private void CreateProjectilePoolContainer()
        {
            var factory = new ObjectFactory<ProjectilePoolHandler>(_prefabsFactory, Address.Prefabs.Projectile);
            _interactorPoolContainer.CreatePool<ProjectilePoolHandler, ObjectFactory<ProjectilePoolHandler>>(factory, _config.initialProjectilePoolCount, Address.Prefabs.Projectile, true);
        }

        void CreateEnemyPoolContainer()
        {
            var factory = new ObjectFactory<EnemyPoolHandler>(_prefabsFactory, Address.Prefabs.Stickman);
            _interactorPoolContainer.CreatePool<EnemyPoolHandler, ObjectFactory<EnemyPoolHandler>>(factory, _config.initialEnemyPoolCount, Address.Prefabs.Stickman, true);
        }

        void CreateDamagePopupPoolContainer()
        {
            var factory = new ObjectFactory<DamagePopup>(_prefabsFactory, Address.Prefabs.DamagePopup);
            _interactorPoolContainer.CreatePool<DamagePopup, ObjectFactory<DamagePopup>>(factory, _config.initialDamagePopupPoolCount, Address.Prefabs.DamagePopup, true);
        }

        void CreateEnemyKillRewardTextPoolContainer()
        {
            var factory = new ObjectFactory<EnemyKillRewardText>(_prefabsFactory, Address.Prefabs.EnemyKillRewardText);
            _interactorPoolContainer.CreatePool<EnemyKillRewardText, ObjectFactory<EnemyKillRewardText>>(factory, _config.initialEnemyKillRewardTextPoolCount, Address.Prefabs.EnemyKillRewardText, true);
        }

        void CreateTentaclePoolContainer()
        {
            var factory = new ObjectFactory<TentaclePoolHandler>(_prefabsFactory, Address.Prefabs.Tentacle);
            _interactorPoolContainer.CreatePool<TentaclePoolHandler, ObjectFactory<TentaclePoolHandler>>(factory, _config.initialTentaclePoolCount, Address.Prefabs.Tentacle, true);
        }
    }
}