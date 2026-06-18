using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    public class SlideMoverContext
    {
        private SlideMover _rootMover;

        private readonly List<Transform> _registered = new();

        public void RegisterRoot(SlideMover obj) =>
            _rootMover = obj;

        public void Register(Transform obj) =>
            _registered.Add(obj);

        public void Unregister(Transform obj) =>
          _registered.Remove(obj);

        public async UniTask StartSlide()
        {
            foreach (var obj in _registered)
                obj.SetParent(_rootMover.transform, worldPositionStays: true);

            await _rootMover.StartSlide();

            _rootMover = null;
            _registered.Clear();
        }
    }
}