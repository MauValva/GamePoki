using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class SlideParticipant : MonoBehaviour
    {
        private SlideMoverContext _context;

        [Inject]
        private void Construct(SlideMoverContext context)
        {
            _context = context;
            context.Register(transform);
        }

        private void OnDestroy() =>
            _context.Unregister(transform);
    }
}