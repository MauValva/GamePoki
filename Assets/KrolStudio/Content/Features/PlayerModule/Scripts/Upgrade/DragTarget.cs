using Core.InputModule;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace KrolStudio
{
    public class DragTarget : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] float moveDuration = 0.35f;

        private Transform t;
        private Vector3 offset;
        private float fixedY;
        private Plane dragPlane;

        private Vector3 targetPosition, startPosition;
        private float moveTime;

        private bool isDragging;
        public bool IsPointerDown { get; set; }
        public bool IsPointerUp { get; set; }

        public event Action PointerDown;
        public event Action PointerUp;
        public event Action PointerDrag;

        private PlayerCameraModel _cameraModel;
        private IInputListener _inputListener;

        [Inject]
        private void Construct(
            PlayerCameraModel cameraModel,
            IInputListener inputListener)
        {
            _cameraModel = cameraModel;
            _inputListener = inputListener;
        }

        void Start()
        {
            isDragging = false;
            t = transform;
        }

        void Update()
        {
            if (!isDragging) return;

            moveTime += Time.deltaTime;
            float t01 = Mathf.Clamp01(moveTime / moveDuration);
            float eased = Easing.EaseOutQuart(t01);
            t.position = Vector3.Lerp(startPosition, targetPosition, eased);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsPointerDown) return;
            IsPointerDown = true;

            fixedY = t.position.y;

            Ray ray = _cameraModel.CurrentCamera.ScreenPointToRay(_inputListener.CurrentMousePosition);
            dragPlane = new Plane(Vector3.up, new Vector3(0, fixedY, 0)); // Plane at the object's Y level
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                PointerDown?.Invoke();
                offset = t.position - hitPoint;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            
            if (!IsPointerDown || IsPointerUp) return;
            IsPointerUp = true;
            PointerUp?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!IsPointerDown || IsPointerUp) return;
            
            isDragging = true;
            PointerDrag?.Invoke();

            Ray ray = _cameraModel.CurrentCamera.ScreenPointToRay(_inputListener.CurrentMousePosition);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                targetPosition = hitPoint + offset;
                targetPosition.y = t.position.y;

                moveTime = 0f;
                startPosition = t.position;
            }
        }
    }
}