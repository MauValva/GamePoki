using UnityEngine;

namespace KrolStudio
{
    public class ObjectFactory<T> : IObjectFactory<T> where T : Component
    {
        private readonly IPrefabsFactory _prefabsFactory;
        private readonly string _key;

        public ObjectFactory(IPrefabsFactory prefabsFactory, string key)
        {
            _prefabsFactory = prefabsFactory;
            _key = key;
        }

        public T Create()
        {
            return _prefabsFactory.Create<T>(_key);
        }
    }
}