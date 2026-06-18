using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace KrolStudio
{
    public class EnemyMarker : MonoBehaviour
    {
        [SerializeField] private SplineContainer playerPathSpline;
        [SerializeField] private List<EntitySpawnData> spawnData;

        public List<EntitySpawnData> SpawnData => spawnData;

        public void OnDrawGizmosSelected()
        {
            foreach (var item in spawnData)
            {
                switch (item.zoneType)
                {
                    case ZoneType.RectAlong:
                        DrawRectangleAlongSpline(item);
                        break;
                    case ZoneType.Circle:
                        DrawCircle(item);
                        break;
                    case ZoneType.Rect:
                    default:
                        DrawRectangle(item);
                        break;
                }

                // Draw spawn/despawn points for all zone types
                DrawSpawnDespawnEvents(item);
            }
        }

        #region Rectangle

        private void DrawRectangle(EntitySpawnData spawnData)
        {
            Vector3 worldPos, worldTangent, worldUp;
            CalculateOrientation(spawnData.position, out worldPos, out worldTangent, out worldUp);
            DrawRectangle(worldPos, spawnData.rectSize, worldTangent, worldUp, spawnData.sectionColor);
        }

        private void DrawRectangle(Vector3 center, Vector2 size, Vector3 tangent, Vector3 up, Color color)
        {
            Color currentColor = Gizmos.color;
            Gizmos.color = color;

            Quaternion rotation = Quaternion.LookRotation(tangent, up);

            Vector3 halfWidth = rotation * new Vector3(size.x * 0.5f, 0f, 0f);
            Vector3 halfHeight = rotation * new Vector3(0f, 0f, size.y * 0.5f);

            Vector3 topLeft = center + halfHeight + halfWidth;
            Vector3 topRight = center + halfHeight - halfWidth;
            Vector3 bottomLeft = center - halfHeight + halfWidth;
            Vector3 bottomRight = center - halfHeight - halfWidth;

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);

            Gizmos.color = currentColor;
        }

        #endregion

        #region Circle

        private void DrawCircle(EntitySpawnData spawnData)
        {
            Vector3 worldPos, worldTangent, worldUp;
            CalculateOrientation(spawnData.position, out worldPos, out worldTangent, out worldUp);
            DrawCircle(worldPos, spawnData, worldTangent, worldUp, spawnData.sectionColor);
        }

        private void DrawCircle(Vector3 center, EntitySpawnData spawnData, Vector3 tangent, Vector3 up, Color color)
        {
            Color currentColor = Gizmos.color;
            Gizmos.color = color;

            const int CircleSegments = 32;
            float angleStep = 360f / CircleSegments;

            Quaternion rotation = Quaternion.LookRotation(tangent, up);
            Vector3 prevPoint = center + rotation * new Vector3(Mathf.Cos(0f), 0f, Mathf.Sin(0f)) * spawnData.radius;

            for (int i = 1; i <= CircleSegments; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector3 nextPoint = center + rotation * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnData.radius;
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }

            Gizmos.color = currentColor;
        }

        #endregion

        #region RectangleAlongSpline

        private void DrawRectangleAlongSpline(EntitySpawnData spawnData)
        {
            Vector3 worldPos, worldTangent, worldUp;

            List<Vector3> points = new(spawnData.numberOfSegments * 2);

            Color color = Gizmos.color;
            Gizmos.color = spawnData.sectionColor;

            float time, t;

            // Middle lines
            for (int i = 0; i < spawnData.numberOfSegments; i++)
            {
                time = i / (float)(spawnData.numberOfSegments - 1);
                t = Mathf.Lerp(spawnData.startRect, spawnData.endRect, time);

                CalculateOrientation(t, out worldPos, out worldTangent, out worldUp);
                DrawLineSegment(worldPos, spawnData.rectWidth, worldTangent, worldUp, points);
            }

            // Side lines
            int iter = points.Count / 2 - 1;
            for (int i = 0, k = 0; i < iter; i++, k += 2)
            {
                Gizmos.DrawLine(points[k], points[k + 2]);
                Gizmos.DrawLine(points[k + 1], points[k + 3]);
            }

            Gizmos.color = color;

            void DrawLineSegment(Vector3 center, float width, Vector3 tangent, Vector3 up, List<Vector3> pointsList)
            {
                Quaternion rotation = Quaternion.LookRotation(tangent, up);
                Vector3 halfWidth = rotation * new Vector3(width * 0.5f, 0f, 0f);

                pointsList.Add(center + halfWidth);
                pointsList.Add(center - halfWidth);

                Gizmos.DrawLine(center + halfWidth, center - halfWidth);
            }
        }

        #endregion

        #region Spawn/Despawn Events

        private void DrawSpawnDespawnEvents(EntitySpawnData spawnData)
        {
            Vector3 worldPos, worldTangent, worldUp;

            Color prevColor = Gizmos.color;

            Gizmos.color = Color.cyan;
            CalculateOrientation(spawnData.spawnEvent, out worldPos, out worldTangent, out worldUp);
            Gizmos.DrawSphere(worldPos, spawnData.sphereSize);

            Gizmos.color = Color.red;
            CalculateOrientation(spawnData.despawnEvent, out worldPos, out worldTangent, out worldUp);
            Gizmos.DrawSphere(worldPos, spawnData.sphereSize);

            Gizmos.color = prevColor;
        }

        #endregion

        #region Helpers

        private void CalculateOrientation(float t, out Vector3 worldPos, out Vector3 worldTangent, out Vector3 worldUp)
        {
            float3 pos, tangent, up;
            SplineUtility.Evaluate(playerPathSpline.Spline, t, out pos, out tangent, out up);

            worldPos = playerPathSpline.transform.TransformPoint(pos);
            worldTangent = playerPathSpline.transform.TransformDirection(tangent);
            worldUp = playerPathSpline.transform.TransformDirection(up);
        }

        #endregion
    }

    [System.Serializable]
    public class EntitySpawnData
    {
        public EnemyType enemyType;
        public ZoneType zoneType;

        [Space]
        public int entityCount = 1;
        [Min(0.5f)] public float sphereSize = 1.2f;
        public Color sectionColor = Color.red;

        [Space]
        [Range(0f, 1f)] public float spawnEvent;
        [Range(0f, 1f)] public float despawnEvent;
        [Min(0.0001f)] public float threshold = 0.01f;

        [Space]
        [Range(0f, 1f)] public float position = 0f;
        // Circle
        [Min(1f)] public float radius = 1;
        // Rectangle
        public Vector2 rectSize;

        // RectAlong
        [Space]
        [Range(0f, 1f)] public float startRect = 0f;
        [Range(0f, 1f)] public float endRect = 0.5f;
        [Min(1f)] public float rectWidth = 10f;
        [Min(2)] public int numberOfSegments = 30;

        [HideInInspector] public bool hasSpawned;
        [HideInInspector] public bool hasDespawned;
    }
}