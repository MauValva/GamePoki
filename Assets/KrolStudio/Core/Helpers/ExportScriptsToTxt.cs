#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace KrolStudio
{
    public class ExportScriptsToTxt : EditorWindow
    {
        private string[] foldersToSearch = new string[] { "Assets/Scripts"};
        private string outputFilePath = "Assets/AllScripts.txt";

        private ExportMode exportMode = ExportMode.FullScripts;

        private enum ExportMode
        {
            FullScripts,
            ClassNamesOnly
        }

        [MenuItem("Tools/Export Scripts to TXT")]
        public static void ShowWindow()
        {
            GetWindow<ExportScriptsToTxt>("Export Scripts");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Folders to search:", EditorStyles.boldLabel);

            int newSize = Mathf.Max(0, EditorGUILayout.IntField("Number of folders:", foldersToSearch.Length));
            if (newSize != foldersToSearch.Length)
            {
                System.Array.Resize(ref foldersToSearch, newSize);
            }

            for (int i = 0; i < foldersToSearch.Length; i++)
            {
                foldersToSearch[i] = EditorGUILayout.TextField($"Folder {i + 1}:", foldersToSearch[i]);
            }

            EditorGUILayout.Space();

            exportMode = (ExportMode)EditorGUILayout.EnumPopup("Export mode:", exportMode);

            outputFilePath = EditorGUILayout.TextField("Output TXT file:", outputFilePath);

            EditorGUILayout.Space();

            if (GUILayout.Button("Export Scripts"))
            {
                ExportScripts();
            }
        }

        private void ExportScripts()
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (string folder in foldersToSearch)
                {
                    string[] scripts = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories);

                    foreach (string script in scripts)
                    {
                        writer.WriteLine("===== " + script + " =====");

                        if (exportMode == ExportMode.FullScripts)
                        {
                            writer.WriteLine(File.ReadAllText(script));
                        }
                        else
                        {
                            WriteClassNames(script, writer);
                        }

                        writer.WriteLine("\n\n");
                    }
                }
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Export Completed", "All scripts have been saved to:\n" + outputFilePath, "OK");
        }

        private void WriteClassNames(string scriptPath, StreamWriter writer)
        {
            string content = File.ReadAllText(scriptPath);

            MatchCollection matches = Regex.Matches(content, @"\bclass\s+(\w+)");

            foreach (Match match in matches)
            {
                writer.WriteLine(match.Groups[1].Value);
            }
        }
    }
}
#endif