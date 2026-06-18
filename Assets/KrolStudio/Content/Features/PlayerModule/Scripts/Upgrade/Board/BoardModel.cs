using System.Collections.Generic;
using System.Linq;

namespace KrolStudio
{
    public class BoardModel
    {
        private readonly SlotModel[] _slots;

        public int TotalSlots => _slots.Length;
        public bool HasFreeSlot => GetFreeSlot() != null;

        public BoardModel(int slotCount)
        {
            _slots = new SlotModel[slotCount];
            for (int i = 0; i < slotCount; i++)
                _slots[i] = new SlotModel();
        }

        public int GetSlotIndex(SlotModel slot)
        {
            for (int i = 0; i < _slots.Length; i++)
                if (_slots[i] == slot) return i;
            return -1;
        }

        public SlotModel GetFreeSlot()
        {
            foreach (var slot in _slots)
                if (!slot.IsOccupied) return slot;
            return null;
        }

        public IReadOnlyList<SlotModel> GetOccupiedSlots() =>
            _slots.Where(s => s.IsOccupied).ToList();
    }
}