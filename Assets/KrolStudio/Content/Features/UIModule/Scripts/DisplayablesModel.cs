using System.Collections.Generic;

namespace KrolStudio
{
    public class DisplayablesModel
    {
        private readonly List<IDisplayable> _displayables = new();

        public IReadOnlyList<IDisplayable> All => _displayables;

        public void Register(IDisplayable displayable) =>
            _displayables.Add(displayable);

        public void Unregister(IDisplayable displayable) =>
            _displayables.Remove(displayable);
    }
}