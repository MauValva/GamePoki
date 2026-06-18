using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class TentacleCullable : MonoBehaviour, ICullable
    {
        public bool IsCulled { get; private set; }
        public Vector3 Position => transform.position;
        public Bounds Bounds => _bounds;

        [SerializeField] private Animator _animator;
        [SerializeField] private Renderer[] _renderers;

        private Bounds _bounds;
        private CullingRegistry _registry;
        private ITentacleStateService _stateService;

        [Inject]
        private void Construct(CullingRegistry registry, ITentacleStateService stateService)
        {
            _registry = registry;
            _stateService = stateService;
        }

        private void LateUpdate()
        {
            if (!IsCulled)
                RecalculateBounds();
        }

        private void RecalculateBounds()
        {
            if (_renderers == null || _renderers.Length == 0) return;

            Bounds b = _renderers[0].bounds;
            for (int i = 1; i < _renderers.Length; i++)
                b.Encapsulate(_renderers[i].bounds);

            _bounds = b;
        }

        private void OnEnable()
        {
            RecalculateBounds();
            IsCulled = false;
            _registry.Register(this);
        }

        private void OnDisable()
        {
            if (IsCulled) Restore();
            _registry.Unregister(this);
        }

        public void Cull()
        {
            IsCulled = true;
            if (_animator != null) _animator.enabled = false;
            foreach (var r in _renderers) r.enabled = false;
        }

        public void Restore()
        {
            IsCulled = false;
            if (_animator != null) _animator.enabled = true;
            foreach (var r in _renderers) r.enabled = true;
            _stateService.EnterIdle().Forget();
        }

        public bool ShouldCull(Vector3 camPos, Plane[] frustumPlanes, CullingConfig config)
        {
            float distSq = (Position - camPos).sqrMagnitude;

            float cullDist = config.EnemyCullDistance;
            float restoreDist = config.EnemyCullDistance - config.EnemyRestoreOffset;

            float cullDistSq = cullDist * cullDist;
            float restoreDistSq = restoreDist * restoreDist;

            bool inView = GeometryUtility.TestPlanesAABB(frustumPlanes, Bounds);

            if (IsCulled)
            {
                if (inView)
                    return false;

                return distSq > restoreDistSq;
            }
            else
            {
                if (inView)
                    return false;

                return distSq > cullDistSq;
            }
        }
    }
}