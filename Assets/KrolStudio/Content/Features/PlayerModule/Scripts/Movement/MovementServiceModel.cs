using System;

namespace KrolStudio
{
    public class MovementServiceModel
    {
        private IMovementService _value;

        public IMovementService Value
        {
            get => _value;
            set
            {
                _value = value;
                OnServiceRegistered?.Invoke(value);
            }
        }

        public event Action<IMovementService> OnServiceRegistered;
    }
}