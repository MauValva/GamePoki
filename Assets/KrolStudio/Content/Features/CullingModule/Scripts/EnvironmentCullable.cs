using UnityEngine;

namespace KrolStudio
{
    public class EnvironmentCullable : MonoBehaviour, ICullable
    {
        public bool IsCulled { get; private set; }
        public Bounds Bounds { get; private set; }
        public bool IsHuge { get; private set; } // Automatic detection of large objects

        private Renderer[] _renderers;
        private CullingRegistry _registry;

        public void Initialize(Renderer[] renderers, CullingRegistry registry)
        {
            _renderers = renderers;
            _registry = registry;

            if (_renderers == null || _renderers.Length == 0)
                return;

            RecalculateBounds();
            DetermineIfHuge();
        }

        private void RecalculateBounds()
        {
            bool hasBounds = false;
            Bounds b = default;

            for (int i = 0; i < _renderers.Length; i++)
            {
                var r = _renderers[i];
                if (r == null) continue;

                var size = r.bounds.size;
                if (size == Vector3.zero) continue; // Protection against invalid/garbage bounds

                if (!hasBounds)
                {
                    b = r.bounds;
                    hasBounds = true;
                }
                else
                {
                    b.Encapsulate(r.bounds);
                }
            }

            if (hasBounds)
                Bounds = b;
        }

        private void DetermineIfHuge()
        {
            var size = Bounds.size;
            float maxSide = Mathf.Max(size.x, size.y, size.z);

            // threshold can be adjusted per level/camera
            IsHuge = maxSide > 50f;
        }

        private void OnDisable()
        {
            if (IsCulled) Restore();
            _registry.Unregister(this);
        }

        public void Cull()
        {
            IsCulled = true;
            if (IsHuge) return; // Do not frustum‑cull large objects.
            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].enabled = false;
        }

        public void Restore()
        {
            IsCulled = false;
            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].enabled = true;
        }

        public bool ShouldCull(Vector3 camPos, Plane[] frustumPlanes, CullingConfig config)
        {
            float distSq = (Bounds.center - camPos).sqrMagnitude;

            if (IsHuge)
            {
                // Large objects use only distance culling (no frustum culling)
                return distSq > config.EnvMaxDistanceSq;
            }

            if (distSq > config.EnvMaxDistanceSq)
                return true;

            if (distSq < config.EnvMinDistanceSq)
                return false;

            // the standard logic for medium‑sized objects
            return !GeometryUtility.TestPlanesAABB(frustumPlanes, Bounds);
        }
    }
}