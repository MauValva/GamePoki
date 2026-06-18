using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace KrolStudio
{
    public class Level : MonoBehaviour
    {
        [Header("PathSpline")]
        [HideInInspector] public SplineContainer playerPathSpline;

        public SplineContainer PathSpline => playerPathSpline;

        public void CreatePathSpline()
        {
#if UNITY_EDITOR
            playerPathSpline = new GameObject("PlayerPathSpline").AddComponent<SplineContainer>();
            Undo.RegisterCreatedObjectUndo(playerPathSpline.gameObject, "Create Player Path Spline");   // For Undo in the editor
            playerPathSpline.transform.SetParent(transform, false);                                     // Set parent to the current object
                                                                                                        // Refresh scenes to reflect changes
            EditorUtility.SetDirty(this);  // Mark the object as modified
#endif
        }
    }
}