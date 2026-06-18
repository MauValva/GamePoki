using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace KrolStudio
{
    public static class RaycastUtility
    {
        private static readonly List<RaycastResult> _buffer = new();

        public static GameObject GetTopUIObject(Vector3 mousePosition)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                // The first object in the list is the topmost UI element under the cursor
                return results[0].gameObject;
            }

            return null; // Nothing was clicked on the UI
        }

        public static bool RaycastFromPosition(out List<RaycastResult> results, Vector2 screenPosition, LayerMask layerMask)
        {
            results = null;

            if (EventSystem.current == null)
            {
                Debug.LogWarning("EventSystem.current is null.");
                return false;
            }

            _buffer.Clear();

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            EventSystem.current.RaycastAll(pointerData, _buffer);

            results = _buffer
                .Where(r => ((1 << r.gameObject.layer) & layerMask) != 0)
                .ToList();

            return results.Count > 0;
        }
    }
}