using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [RequireComponent(typeof(TentacleMonoEntity))]
    public class TentaclePoolHandler : MonoBehaviour, IEnemy, IPoolReturnable<TentaclePoolHandler>
    {
        public event Action<TentaclePoolHandler> OnReturned;

        private EnemySpawnPointModel _spawnPointModel;
        private TentacleMonoEntity _monoEntity;

        public Vector3 SpawnPosition
        {
            set => _spawnPointModel.SpawnPosition = value;
        }

        [Inject]
        private void Construct(EnemySpawnPointModel spawnPointModel)
        {
            _spawnPointModel = spawnPointModel;
        }

        private void Awake() =>
            _monoEntity = GetComponent<TentacleMonoEntity>();

        public void ReturnToPool() =>
            OnReturned?.Invoke(this);

        public void SetDefaultState()
        {
            _monoEntity.SetDefaultState();
        }
    }
}