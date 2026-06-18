#if UNITY_EDITOR

using UnityEngine;

namespace KrolStudio
{
    public static class GizmosUtility
    {
        public static void DebugSphereCast(Transform transform, float radius, float distance)
        {
            Vector3 origin = transform.position + transform.up;
            Vector3 direction = transform.forward;

            // Draw the central ray
            Debug.DrawRay(origin, direction * distance, Color.red);

            // Draw spheres at the start and end points (represented as circles)
            DrawCircle(origin, radius, transform.up, Color.green);
            DrawCircle(origin + direction * distance, radius, transform.up, Color.green);
        }

        public static void DrawCircle(Vector3 center, float radius, Vector3 up, Color color, int segments = 24)
        {
            float angleStep = 360f / segments;
            Quaternion rotation = Quaternion.AngleAxis(angleStep, up);
            Vector3 forward = Vector3.forward * radius;
            Vector3 prevPoint = center + forward;

            for (int i = 0; i <= segments; i++)
            {
                forward = rotation * forward;
                Vector3 nextPoint = center + forward;
                Debug.DrawLine(prevPoint, nextPoint, color);
                prevPoint = nextPoint;
            }
        }
    }
}
#endif