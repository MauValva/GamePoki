
namespace KrolStudio
{
    public class SlotModel
    {
        public bool IsOccupied { get; private set; }
        public Part Part { get; private set; }

        public void Occupy(Part part)
        {
            Part = part;
            IsOccupied = true;
        }

        public void Free()
        {
            Part = null;
            IsOccupied = false;
        }
    }
}