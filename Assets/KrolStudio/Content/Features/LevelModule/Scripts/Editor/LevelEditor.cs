#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Splines;
using UnityEngine;

namespace KrolStudio
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        private Level map;

        public void OnEnable()
        {
            map = target as Level;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            map.playerPathSpline = (SplineContainer)EditorGUILayout.ObjectField("Player path spline", map.playerPathSpline, typeof(SplineContainer), true);
            if (GUILayout.Button("Create PlayerPathSpline"))
                map.CreatePathSpline();
        }
    }
}
#endif