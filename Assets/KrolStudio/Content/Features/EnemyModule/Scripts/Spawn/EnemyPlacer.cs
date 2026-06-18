using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace KrolStudio
{
    public class EnemyPlacer
    {
        // Direct mapping: spawn point -> list of enemies (for DestroyEnemies)
        private readonly Dictionary<EntitySpawnData, List<IPoolReturnable>> _enemiesByPoint = new();
        // Reverse mapping: enemy -> its spawn point (for DestroyEnemy)
        private readonly Dictionary<IPoolReturnable, EntitySpawnData> _spawnDataByEnemy = new();

        private readonly IInteractorPoolContainer _interactorPool;
        private readonly SplineContainer _splineContainer;

        [Inject]
        public EnemyPlacer(
            IInteractorPoolContainer interactorPool,
            LevelModel levelModel)
        {
            _interactorPool = interactorPool;
            _splineContainer = levelModel.CurrentLevel().PathSpline;
        }

        public void SpawnEnemy(EntitySpawnData spawn)
        {
            _enemiesByPoint.Add(spawn, new List<IPoolReturnable>());

            for (int i = 0; i < spawn.entityCount; i++)
            { 
                if(spawn.zoneType == ZoneType.RectAlong)
                    Spawn(UnityEngine.Random.Range(spawn.startRect, spawn.endRect));
                else
                    Spawn(spawn.position);
            }

            void Spawn(float position)
            {
                CalculateOrientation(position, out var worldPos, out var worldTangent, out var worldUp);
                var rotation = Quaternion.LookRotation(worldTangent, worldUp);
                var enemy = SpawnMob(spawn.enemyType, worldPos, rotation, spawn);
            }
        }

        private IEnemy SpawnMob(EnemyType enemyType, Vector3 worldPos, Quaternion rotation, EntitySpawnData data)
        {
            return enemyType switch
            {
                EnemyType.Stickman => SpawnMob<EnemyPoolHandler>(worldPos, rotation, data),
                EnemyType.Tentacle => SpawnMob<TentaclePoolHandler>(worldPos, rotation, data),
                _ => default
            };
        }

        T SpawnMob<T>(Vector3 worldPos, Quaternion rotation, EntitySpawnData data) where T : MonoBehaviour, IEnemy
        {
            T newMob = _interactorPool.GetPool<T>().Get();
            newMob.transform.rotation = rotation;
            newMob.SpawnPosition = worldPos;
            newMob.SetDefaultState();

            if (data.zoneType == ZoneType.RectAlong)
            {
                newMob.transform.position = GetRandomPositionInRectAlong(worldPos, rotation, data.rectWidth);
            }
            else if (data.zoneType == ZoneType.Circle)
            {
                newMob.transform.position = GetRandomPointInCircle(worldPos, rotation, data.radius);
            }
            else
            {
                newMob.transform.position = GetRandomPositionInRect(worldPos, rotation, data.rectSize);
            }

            var poolReturnable = newMob.GetComponent<IPoolReturnable>();
            _enemiesByPoint[data].Add(poolReturnable);
            _spawnDataByEnemy[poolReturnable] = data;

            return newMob;
        }

        public void DestroyEnemy(IPoolReturnable enemy)
        {
            if (!_spawnDataByEnemy.TryGetValue(enemy, out var spawnData))
                return;

            enemy.ReturnToPool();
            _enemiesByPoint[spawnData].Remove(enemy);
            _spawnDataByEnemy.Remove(enemy);
        }

        public void DestroyEnemies(EntitySpawnData data)
        {
            if (!_enemiesByPoint.TryGetValue(data, out var enemyList))
                return;

            foreach (var enemy in enemyList)
            {
                enemy.ReturnToPool();
                _spawnDataByEnemy.Remove(enemy);
            }

            enemyList.Clear();
            _enemiesByPoint.Remove(data);
        }

        private void CalculateOrientation(float t, out float3 worldPos, out float3 worldTangent, out float3 worldUp)
        {
            SplineUtility.Evaluate(_splineContainer.Spline, t, out worldPos, out worldTangent, out worldUp);

            worldPos = _splineContainer.transform.TransformPoint(worldPos);
            worldTangent = _splineContainer.transform.TransformDirection(worldTangent);
            worldUp = _splineContainer.transform.TransformDirection(worldUp);
        }

        private Vector3 GetRandomPositionInRectAlong(Vector3 position, Quaternion rotation, float rectWidth)
        {
            Vector3 halfWidth = rotation * new Vector3(rectWidth * 0.5f, 0f, 0f);
            return Vector3.Lerp(position + halfWidth, position - halfWidth, UnityEngine.Random.value);
        }

        private Vector3 GetRandomPointInCircle(Vector3 center, Quaternion rotation, float radius)
        {
            Vector2 randomPoint2D = UnityEngine.Random.insideUnitCircle * radius;
            return center + rotation * new Vector3(randomPoint2D.x, 0f, randomPoint2D.y);
        }

        private Vector3 GetRandomPositionInRect(Vector3 center, Quaternion rotation, Vector2 size)
        {
            var randomX = UnityEngine.Random.Range(-size.x * 0.5f, size.x * 0.5f);
            var randomZ = UnityEngine.Random.Range(-size.y * 0.5f, size.y * 0.5f);
            return center + rotation * new Vector3(randomX, 0f, randomZ);
        }
    }
}