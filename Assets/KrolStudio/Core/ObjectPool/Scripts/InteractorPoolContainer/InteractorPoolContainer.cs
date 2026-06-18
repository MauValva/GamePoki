using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class InteractorPoolContainer : IInteractorPoolContainer
    {
        Dictionary<Type, IPoolObject> container = new();

        /*readonly IPoolObjectsContainer objectsContainer;
        [Inject]
        public InteractorPoolContainer(IPoolObjectsContainer objectsContainer)
        {
            this.objectsContainer = objectsContainer;
        }*/

        public bool TryGet<T>(out T interactor) where T : IPoolObject<T>
        {
            if (container.TryGetValue(typeof(T), out IPoolObject value) && value is T typed)
            {
                interactor = typed;
                return true;
            }

            interactor = default;
            return false;
        }

        public IPoolObject<T> GetPool<T>() where T : Component
        {
            return container[typeof(IPoolObject<T>)] as PoolObject<T>;
        }

        public void CreatePool<T, T1>(T1 factory, int initialCount, string poolName, bool clearOnSpawn = false) where T : Component where T1 : IObjectFactory<T>
        {
            var type = typeof(IPoolObject<T>);
            if (clearOnSpawn && container.ContainsKey(type))
                container.Remove(type);

            container.Add(type, new PoolObject<T>(factory, initialCount, poolName/*, objectsContainer*/));
        }

        public void CleanUp()
        {
            container.Clear();
        }
    }
}