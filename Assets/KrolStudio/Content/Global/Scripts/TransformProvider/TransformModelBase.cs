using System;
using UnityEngine;

namespace KrolStudio
{ 
    public abstract class TransformModelBase : ITransformProvider
    {
        protected Transform _transform;

        public virtual Transform Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                OnTransformChanged?.Invoke(value);
            }
        }

        public Vector3 Position
        {
            get => _transform != null ? _transform.position : default;
            set
            {
                if (_transform != null)
                    _transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get => _transform != null ? _transform.rotation : default;
            set
            {
                if (_transform != null)
                    _transform.rotation = value;
            }
        }

        public Vector3 Scale
        {
            get => _transform != null ? _transform.localScale : default;
            set
            {
                if (_transform != null)
                    _transform.localScale = value;
            }
        }

        public float DistanceToSq(Vector3 target) =>
            (Position - target).sqrMagnitude;

        public float DistanceTo(Vector3 target) =>
            _transform != null
                ? Vector3.Distance(_transform.position, target)
                : float.MaxValue;

        public event Action<Transform> OnTransformChanged;
    }
}