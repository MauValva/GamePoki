using UnityEditor;
using UnityEngine;

namespace KrolStudio
{
    public class ToolsMenu
    {
        [MenuItem("Tools/", false)]
        static void Separator() { }

        [MenuItem("Tools/🗑️ Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            bool confirm = EditorUtility.DisplayDialog(
                "Clear PlayerPrefs",
                "Are you sure you want to delete ALL PlayerPrefs?\nThis action cannot be undone.",
                "Yes",
                "No"
            );

            if (!confirm)
                return;

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}