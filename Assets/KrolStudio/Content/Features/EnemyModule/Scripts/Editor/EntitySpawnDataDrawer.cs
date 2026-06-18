#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace KrolStudio
{
    [CustomPropertyDrawer(typeof(EntitySpawnData))]
    public class EntitySpawnDataDrawer : PropertyDrawer
    {
        private const float Spacing = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            float height = EditorGUIUtility.singleLineHeight + Spacing; // Foldout

            height += GetBasePropertiesHeight(property);
            height += EditorGUIUtility.singleLineHeight + Spacing; // "Zone Settings" label
            height += GetZonePropertiesHeight(property);

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float y = position.y;

            // Foldout
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded,
                label,
                true);

            y += EditorGUIUtility.singleLineHeight + Spacing;

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            // Basic fields
            y = DrawProperty(position, y, property, "enemyType");
            y = DrawProperty(position, y, property, "zoneType");
            y = DrawProperty(position, y, property, "entityCount");
            y = DrawProperty(position, y, property, "sectionColor");
            y = DrawProperty(position, y, property, "sphereSize");
            y = DrawProperty(position, y, property, "spawnEvent");
            y = DrawProperty(position, y, property, "despawnEvent");
            y = DrawProperty(position, y, property, "threshold");

            // Title
            EditorGUI.LabelField(
                new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight),
                "Zone Settings",
                EditorStyles.boldLabel);

            y += EditorGUIUtility.singleLineHeight + Spacing;

            // zoneType
            var zoneTypeProp = property.FindPropertyRelative("zoneType");
            if (zoneTypeProp == null)
            {
                EditorGUI.LabelField(position, "zoneType not found");
                EditorGUI.EndProperty();
                return;
            }

            var zoneType = (ZoneType)zoneTypeProp.intValue;

            // Specific fields
            switch (zoneType)
            {
                case ZoneType.Circle:
                    y = DrawProperty(position, y, property, "position");
                    y = DrawProperty(position, y, property, "radius");
                    break;

                case ZoneType.Rect:
                    y = DrawProperty(position, y, property, "position");
                    y = DrawProperty(position, y, property, "rectSize");
                    break;

                case ZoneType.RectAlong:
                    y = DrawProperty(position, y, property, "startRect");
                    y = DrawProperty(position, y, property, "endRect");
                    y = DrawProperty(position, y, property, "rectWidth");
                    y = DrawProperty(position, y, property, "numberOfSegments");
                    break;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        // -----------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------

        private float DrawProperty(Rect position, float y, SerializedProperty parent, string name)
        {
            var prop = parent.FindPropertyRelative(name);
            if (prop == null) return y;

            float height = EditorGUI.GetPropertyHeight(prop, true);

            EditorGUI.PropertyField(
                new Rect(position.x, y, position.width, height),
                prop,
                true);

            return y + height + Spacing;
        }

        private float GetBasePropertiesHeight(SerializedProperty property)
        {
            string[] props =
            {
                "enemyType",
                "zoneType",
                "entityCount",
                "sectionColor",
                "sphereSize",
                "spawnEvent",
                "despawnEvent",
                "threshold"
            };

            float height = 0f;

            foreach (var name in props)
            {
                var prop = property.FindPropertyRelative(name);
                if (prop != null)
                    height += EditorGUI.GetPropertyHeight(prop, true) + Spacing;
            }

            return height;
        }

        private float GetZonePropertiesHeight(SerializedProperty property)
        {
            var zoneTypeProp = property.FindPropertyRelative("zoneType");
            if (zoneTypeProp == null) return 0;

            var zoneType = (ZoneType)zoneTypeProp.intValue;

            string[] props = zoneType switch
            {
                ZoneType.Circle => new[] { "position", "radius" },
                ZoneType.Rect => new[] { "position", "rectSize" },
                ZoneType.RectAlong => new[] { "startRect", "endRect", "rectWidth", "numberOfSegments" },
                _ => null
            };

            if (props == null) return 0;

            float height = 0f;

            foreach (var name in props)
            {
                var prop = property.FindPropertyRelative(name);
                if (prop != null)
                    height += EditorGUI.GetPropertyHeight(prop, true) + Spacing;
            }

            return height;
        }
    }
}
#endif