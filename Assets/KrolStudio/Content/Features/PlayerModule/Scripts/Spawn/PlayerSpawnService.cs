using Global.Scripts.Generated;
using Zenject;

namespace KrolStudio
{
    public class PlayerSpawnService : IInitializable
    {
        private readonly IPrefabsFactory _prefabsFactory;

        [Inject]
        public PlayerSpawnService(IPrefabsFactory prefabsFactory)
        {
            _prefabsFactory = prefabsFactory;
        }

        public void Initialize()
        {
            _prefabsFactory.Create<PlayerMonoEntity>(Address.Prefabs.Player);
            _prefabsFactory.Create(Address.Prefabs.PlayerCamera);
        }
    }
}