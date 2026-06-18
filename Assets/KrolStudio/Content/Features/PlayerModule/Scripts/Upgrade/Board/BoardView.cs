using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private Transform[] _slotTransforms;
        [Space]
        [SerializeField] float levelDecayExponent = 1.5f;   // Controls the rate at which the chance decreases
        [SerializeField] float minWeight = 0.1f;            // Minimum chance below which it does not decrease

        public float LevelDecayExponent => levelDecayExponent;
        public float MinWeight => minWeight;
        public int TotalSlots => _slotTransforms.Length;

        public void ClearBoardView()
        {
            foreach (var slot in _slotTransforms)
            {
                if(slot.childCount > 0)
                    Destroy(slot.GetChild(0).gameObject);
            }
        }

        public void PlacePartInSlot(PartInteractionHandler part, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slotTransforms.Length) return;
            part.transform.SetParent(_slotTransforms[slotIndex]);
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.identity;
        }

        public void PlacePartInSlot(PartInteractionHandler part, Transform cell)
        {
            part.transform.SetParent(cell, false);
            part.transform.localPosition = Vector3.zero;
            part.transform.localRotation = Quaternion.identity;
        }

        public List<BoardEntry> GetBoardEntry()
        {
            List<BoardEntry> entries = new();
            PartInteractionHandler part;

            for (int i = 0; i < _slotTransforms.Length; i++)
            {
                if(_slotTransforms[i].childCount != 0)
                {
                    part = _slotTransforms[i].GetComponentInChildren<PartInteractionHandler>();
                    entries.Add(new BoardEntry()
                    {
                        type = part.PartType,
                        level = part.Level,
                        cellIndex = i,
                    });
                }
            }

            return entries;
        }

        public int GetFreeCellIndex()
        {
            for (int i = 0; i < _slotTransforms.Length; i++)
                if (_slotTransforms[i].childCount == 0) return i;
            return -1;
        }

        public Transform GetNearestFreeCell(Vector3 position)
        {
            Transform nearest = null;
            float minDistSqr = float.MaxValue;

            foreach (var cell in _slotTransforms)
            {
                if (cell.childCount > 0) continue;

                float distSqr = (cell.position - position).sqrMagnitude;
                if (distSqr >= minDistSqr) continue;

                minDistSqr = distSqr;
                nearest = cell;
            }

            return nearest;
        }

        public IEnumerable<PartInteractionHandler> GetAllParts()
        {
            foreach (var cell in _slotTransforms)
            {
                if (cell.childCount == 0) continue;
                var part = cell.GetComponentInChildren<PartInteractionHandler>();
                if (part != null) yield return part;
            }
        }
    }
}

