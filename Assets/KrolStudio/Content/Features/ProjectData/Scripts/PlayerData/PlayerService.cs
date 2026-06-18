using System;
using System.Linq;
using Zenject;

namespace KrolStudio
{
    public class PlayerService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly UpgradeLevelController _upgradeLevelController;

        [Inject]
        public PlayerService(
            SignalBus signalBus, 
            UpgradeLevelController upgradeLevelController)
        {
            _signalBus = signalBus;
            _upgradeLevelController = upgradeLevelController;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayerDataLoadedSignal>(OnDataLoaded);
            _signalBus.Fire(new RequestPlayerDataSignal());
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDataLoadedSignal>(OnDataLoaded);
        }

        private void OnDataLoaded(PlayerDataLoadedSignal signal)
        {
            foreach (var item in signal.Entries)
                SetPart(item.type, item.level);
        }

        public void FireChanged() =>
           _signalBus.Fire(new PlayerDataChangedSignal
           {
               Entries = Enum.GetValues(typeof(PartType))
                    .Cast<PartType>()
                    .Where(t => t != PartType.None)
                    .Select(type => new PartTypeSettings
                    {
                        type = type,
                        level = _upgradeLevelController.GetLevel(type)
                    })
                    .ToList()
           });

        public void SetPart(PartType type, int level)
        {
            _upgradeLevelController.SetLevel(type, level);
            FireChanged();
        }
    }
}
