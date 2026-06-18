using UnityEngine;

namespace KrolStudio
{
    public class ExplosionFragmentLauncher
    {
        private readonly float _angleInDegrees;

        public ExplosionFragmentLauncher(float angleInDegrees)
        {
            _angleInDegrees = angleInDegrees;
        }

        private float CalculateFlightTime(float speed, float angleRad, float heightDifference)
        {
            float vy = speed * Mathf.Sin(angleRad);
            float g = -Physics.gravity.y;

            // Quadratic equation: h = vy * t - 0.5 * g * t^2
            // Solve for the time to reach height h (there may be 2 roots — take the greater one)
            float discriminant = vy * vy + 2 * g * heightDifference;
            if (discriminant < 0f) return 0f;

            float t1 = (vy + Mathf.Sqrt(discriminant)) / g;
            float t2 = (vy - Mathf.Sqrt(discriminant)) / g;

            return Mathf.Max(t1, t2);
        }

        private Vector3 ApplyRandomConeDeviation(Vector3 axis, float maxAngleDeg)
        {
            float maxAngleRad = maxAngleDeg * Mathf.Deg2Rad;

            // Generate a random direction within a cone around the original axis
            float z = Mathf.Cos(Random.Range(0f, maxAngleRad));
            float theta = Random.Range(0f, 2f * Mathf.PI);
            float r = Mathf.Sqrt(1f - z * z);

            Vector3 localDir = new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), z);
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, axis.normalized);

            return rot * localDir;
        }

        public void Launch(
            Rigidbody projectile,
            Vector3 origin,
            Vector3 target,
            int rotationCount = 1,
            bool enableRotation = true,
            Vector3? customRotationAxis = null,
            float rotationAxisDeviation = 0f)
        {
            Vector3 fromTo = target - origin;
            Vector3 dirXZ = new Vector3(fromTo.x, 0, fromTo.z).normalized;
            float angleRad = _angleInDegrees * Mathf.Deg2Rad;

            Vector3 shootDirection = Quaternion.AngleAxis(
                _angleInDegrees,
                Vector3.Cross(dirXZ, Vector3.up)) * dirXZ;

            float speed = CalculateShotSpeed(origin, target, angleRad);
            float flightTime = CalculateFlightTime(speed, angleRad, fromTo.y);

            projectile.linearVelocity = shootDirection * speed;

            if (enableRotation && flightTime > 0f)
            {
                Vector3 rotationAxis = customRotationAxis.HasValue
                    ? customRotationAxis.Value.normalized
                    : GetRotationAxis(shootDirection);

                if (rotationAxisDeviation > 0f)
                    rotationAxis = ApplyRandomConeDeviation(rotationAxis, rotationAxisDeviation);

                projectile.angularVelocity = rotationAxis * (2f * Mathf.PI * rotationCount / flightTime);
            }

            projectile.isKinematic = false;
        }

        public void Launch(
            Rigidbody projectile,
            Transform origin,
            Transform target,
            float rotationCount = 1f,
            bool enableRotation = true,
            Vector3? customRotationAxis = null,
            float rotationAxisDeviation = 0f) =>
            Launch(projectile, origin.position, target.position,
                    (int)rotationCount, enableRotation,
                    customRotationAxis, rotationAxisDeviation);

        private Vector3 GetRotationAxis(Vector3 shootDirection)
        {
            Vector3 axis = Vector3.Cross(shootDirection, Vector3.up).normalized;
            return axis == Vector3.zero
                ? Vector3.Cross(shootDirection, Vector3.right).normalized
                : axis;
        }

        private float CalculateShotSpeed(Vector3 from, Vector3 to, float angleRad)
        {
            Vector3 fromTo = to - from;
            float x = new Vector3(fromTo.x, 0f, fromTo.z).magnitude;
            float y = fromTo.y;
            float denominator = 2f * (y - Mathf.Tan(angleRad) * x);

            if (Mathf.Approximately(denominator, 0f))
            {
                Debug.LogWarning("CalculateShotSpeed: denominator is zero — invalid trajectory.");
                return 0f;
            }

            float vSquared = (Physics.gravity.y * x * x) /
                                (denominator * Mathf.Pow(Mathf.Cos(angleRad), 2f));

            if (vSquared < 0f)
            {
                Debug.LogWarning($"CalculateShotSpeed: vSquared is negative ({vSquared}). Check angle/distance.");
                return 0f;
            }

            return Mathf.Sqrt(vSquared);
        }
    }
}