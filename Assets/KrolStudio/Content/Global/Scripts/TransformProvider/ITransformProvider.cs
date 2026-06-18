using System;
using UnityEngine;

namespace KrolStudio
{
    public interface ITransformProvider
    {
        Transform Transform { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 Scale { get; set; }
        float DistanceTo(Vector3 target);

        event Action<Transform> OnTransformChanged;
    }
}