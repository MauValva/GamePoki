using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class CullingService : ITickable
    {
        private readonly CullingRegistry _registry;
        private readonly PlayerCameraModel _cameraModel;
        private readonly CullingConfig _config;

        private int _tick;

        private readonly Plane[] _frustumPlanes = new Plane[6];

        [Inject]
        public CullingService(
            CullingRegistry registry,
            PlayerCameraModel cameraModel,
            CullingConfig config)
        {
            _registry = registry;
            _cameraModel = cameraModel;
            _config = config;
        }

        public void Tick()
        {
            if (++_tick % _config.CheckInterval != 0)
                return;

            var cam = _cameraModel.CurrentCamera;
            if (cam == null)
                return;

            GeometryUtility.CalculateFrustumPlanes(cam, _frustumPlanes);
            Vector3 camPos = cam.transform.position;

            var items = _registry.Items;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                bool shouldCull = ShouldCull(item, camPos);

                if (shouldCull && !item.IsCulled)
                    item.Cull();
                else if (!shouldCull && item.IsCulled)
                    item.Restore();
            }
        }

        private bool ShouldCull(ICullable cullable, Vector3 camPos) =>
            cullable.ShouldCull(camPos, _frustumPlanes, _config);        
    }
}