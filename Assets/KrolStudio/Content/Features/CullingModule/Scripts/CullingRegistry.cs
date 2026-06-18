using System.Collections.Generic;

namespace KrolStudio
{
    public class CullingRegistry
    {
        private readonly List<ICullable> _items = new(128);

        public IReadOnlyList<ICullable> Items => _items;

        public void Register(ICullable cullable) =>
            _items.Add(cullable);

        public void Unregister(ICullable cullable) =>
            _items.Remove(cullable);
    }
}