using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

namespace KrolStudio
{
    public class PlayerSplineMover : IMovementService, IInitializable
    {
        private readonly SplineContainer _splineContainer;
        private readonly PlayerTransformModel _transform;
        private readonly MovementServiceModel _model;

        private readonly float _splineLength;

        float speed;
        float t;

        private bool _isCompleted;
        public bool _isStarted;

        public event Action SplineMoveStarted;
        public event Action SplineMoveCompleted;
        public event Action<float> OnProgressChanged;

        public float NormalizedTime => t;
        public float SplineLength => _splineLength; 

        [Inject]
        public PlayerSplineMover(
            PlayerTransformModel transformProvider,
            LevelModel levelModel,
            MovementServiceModel model)
        {
            _transform = transformProvider;
            _splineContainer = levelModel.CurrentLevel().PathSpline;
            _splineLength = _splineContainer.CalculateLength();
            _model = model;
        }

        public void Initialize() =>
           _model.Value = this; // We register ourselves in the parent model.

        public bool HasReachedPoint(float destination, float threshold) =>
            Math.Abs(destination - t) < threshold;

        public void SetPosition()
        {
            float3 pos, tangent, up;
            SplineUtility.Evaluate(_splineContainer.Spline, t, out pos, out tangent, out up);

            _transform.Position = _splineContainer.transform.TransformPoint(pos);
            Quaternion rot = Quaternion.LookRotation(
                _splineContainer.transform.TransformDirection(tangent),
                _splineContainer.transform.TransformDirection(up)
            );
            _transform.Rotation = rot;
        }

        public void Tick()
        {
            if (_isCompleted) return;

            if(!_isStarted)
            {
                _isStarted = true;
                SplineMoveStarted?.Invoke();
            }

            // How much distance to cover along the normalized length this frame
            float tDelta = speed * Time.deltaTime / _splineLength;
            float newT = Mathf.Clamp01(t + tDelta);

            if (!Mathf.Approximately(newT, t)) // Only if it has changed.
            {
                t = newT;
                OnProgressChanged?.Invoke(t);
            }

            if (Mathf.Approximately(t, 1f))
            {
                _isCompleted = true;
                SplineMoveCompleted?.Invoke();
                return;
            }

            SetPosition();
        }

        public void Restart()
        {
            _isStarted = false;
            _isCompleted = false;
            t = 0;
            SetPosition();
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }    
    }
}