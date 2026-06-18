using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace KrolStudio
{
    public class EnemySpawnService : ITickable
    {
        private readonly List<EntitySpawnData> _allSpawnData;
        private readonly EnemyPlacer _enemyPlacer;
        private readonly PlayerSplineMover _playerSplineMover;

        public EnemySpawnService(
            List<EnemyMarker> spawnMarkers,
            EnemyPlacer enemyPlacer,
            PlayerSplineMover playerSplineMover)
        {
            _enemyPlacer = enemyPlacer;
            _playerSplineMover = playerSplineMover;

            _allSpawnData = spawnMarkers
                .SelectMany(m => m.SpawnData)
                .ToList();
        }

        public void Tick()
        {
            foreach (var item in _allSpawnData)
            {
                if(!item.hasSpawned && _playerSplineMover.HasReachedPoint(item.spawnEvent, item.threshold))
                {
                    item.hasSpawned = true;
                    _enemyPlacer.SpawnEnemy(item);
                }

                if (!item.hasDespawned && _playerSplineMover.HasReachedPoint(item.despawnEvent, item.threshold))
                {
                    item.hasDespawned = true;
                    _enemyPlacer.DestroyEnemies(item);
                }
            }
        }
    }
}