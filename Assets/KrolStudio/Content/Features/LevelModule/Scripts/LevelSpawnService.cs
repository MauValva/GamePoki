using Zenject;

namespace KrolStudio
{
    public class LevelSpawnService : IInitializable
    {
        private readonly ILevelFactory _levelFactory;
        private readonly LevelModel _levelModel;

        [Inject]
        public LevelSpawnService(
            ILevelFactory levelFactory,
            LevelModel levelModel)
        {
            _levelFactory = levelFactory;
            _levelModel = levelModel;
        }

        public void Initialize() =>
            _levelFactory.Spawn(_levelModel.CurrentLevel());
    }
}