using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [RequireComponent(typeof(EnemyMonoEntity))]
    public class EnemyPoolHandler : MonoBehaviour, IEnemy, IPoolReturnable<EnemyPoolHandler>
    {
        public event Action<EnemyPoolHandler> OnReturned;

        private EnemySpawnPointModel _spawnPointModel;
        private EnemyMonoEntity _monoEntity;

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
            _monoEntity = GetComponent<EnemyMonoEntity>();

        public void ReturnToPool() =>
            OnReturned?.Invoke(this);

        public void SetDefaultState()
        {
            _monoEntity.SetDefaultState();
        }
    }
}