using System;
using Zenject;

namespace KrolStudio
{
    public class LevelModel : IInitializable, IDisposable
    {
        private readonly LevelDatabase _levelDatabase;
        private readonly IProgressService _progressService;
        private readonly SignalBus _signalBus;

        [Inject]
        public LevelModel(
            LevelDatabase levelDatabase, 
            SignalBus signalBus,
            IProgressService progressService)
        {
            _levelDatabase = levelDatabase;
            _signalBus = signalBus;
            _progressService = progressService;
        }

        public int CurrentIndex => _progressService.GetLevel();

        public Level CurrentLevel()
        {
            int nextIndex = _progressService.GetLevel();
            return _levelDatabase.GetLevel(nextIndex >= _levelDatabase.Count ? 0 : nextIndex);
        }

        public void Initialize() =>
            _signalBus.Subscribe<NextLevelSignal>(NextLevel);

        public void Dispose() =>
            _signalBus.Unsubscribe<NextLevelSignal>(NextLevel);

        public void NextLevel(NextLevelSignal _) =>
            _progressService.SetLevel(_progressService.GetLevel() + 1);
    }
}