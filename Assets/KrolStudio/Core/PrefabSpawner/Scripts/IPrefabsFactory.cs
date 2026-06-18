using UnityEngine;

namespace KrolStudio
{
    public interface IPrefabsFactory
    {
        public GameObject Create(string prefabName);
        public GameObject Create(string prefabName, Vector3 position);
        public GameObject Create(string prefabName, Vector3 position, Transform parent);
        
        public T Create<T>(string prefabName) where T : Component;
        public T Create<T>(string prefabName, Vector3 position) where T : Component;
        public T Create<T>(string prefabName, Vector3 position, Transform parent) where T : Component;
    }
}