// LevelFactory.cs
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class LevelFactory : ILevelFactory
    {
        private readonly DiContainer _container;
        private readonly CullingRegistry _cullingRegistry;
        private readonly CullingConfig _cullingConfig;

        [Inject]
        public LevelFactory(
            DiContainer container,
            CullingRegistry cullingRegistry,
            CullingConfig cullingConfig)
        {
            _container = container;
            _cullingRegistry = cullingRegistry;
            _cullingConfig = cullingConfig;
        }

        public GameObject Spawn(Level level)
        {
            var go = _container.InstantiatePrefab(level.gameObject, Vector3.zero, Quaternion.identity, null);

            SetupEnvironmentCulling(go);

            return go;
        }

        private void SetupEnvironmentCulling(GameObject levelRoot)
        {
            // Group renderers by their parent object 
            // — one EnvironmentCullable per logical environment object.
            var renderers = levelRoot.GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in renderers)
            {
                // Small objects are not culled — the overhead outweighs the benefit
                if (renderer.bounds.size.sqrMagnitude < 1f) continue;


                var cullable = renderer.gameObject.AddComponent<EnvironmentCullable>();
                cullable.Initialize(new Renderer[] { renderer }, _cullingRegistry);
                _cullingRegistry.Register(cullable);
            }
        }
    }
}