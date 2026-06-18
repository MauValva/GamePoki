using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PrefabsFactory : IPrefabsFactory
    {
        private readonly IAddressablesAssetLoaderService _addressablesAssetLoaderService;
        private readonly DiContainer _diContainer;

        public PrefabsFactory(IAddressablesAssetLoaderService addressablesAssetLoaderService, DiContainer diContainer)
        {
            _addressablesAssetLoaderService = addressablesAssetLoaderService;
            _diContainer = diContainer;
        }


        public GameObject Create(string prefabName)
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab);
        }

        public GameObject Create(string prefabName, Vector3 position)
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, null);
        }

        public GameObject Create(string prefabName, Vector3 position, Transform parent)
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, parent);
        }


        public T Create<T>(string prefabName) where T : Component
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab).GetComponent<T>();
        }

        public T Create<T>(string prefabName, Vector3 position) where T : Component
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, null).GetComponent<T>();
        }

        public T Create<T>(string prefabName, Vector3 position, Transform parent) where T : Component
        {
            GameObject prefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(prefabName);
            return _diContainer.InstantiatePrefab(prefab, position, Quaternion.identity, parent).GetComponent<T>();
        }
    }
}