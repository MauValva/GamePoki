#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace KrolStudio
{
    public class FolderStructureExporter : EditorWindow
    {
        private List<string> folders = new List<string>() { "Assets" };
        private string outputFile = "Assets/FolderStructure.txt";

        [MenuItem("Tools/Folder Structure Exporter")]
        public static void ShowWindow()
        {
            GetWindow<FolderStructureExporter>("Export Structure");
        }

        private void OnGUI()
        {
            GUILayout.Label("Folders to search:", EditorStyles.boldLabel);

            int newCount = Mathf.Max(1, EditorGUILayout.IntField("Number of folders:", folders.Count));

            while (newCount > folders.Count)
                folders.Add("Assets");

            while (newCount < folders.Count)
                folders.RemoveAt(folders.Count - 1);

            for (int i = 0; i < folders.Count; i++)
            {
                folders[i] = EditorGUILayout.TextField($"Folder {i + 1}:", folders[i]);
            }

            GUILayout.Space(10);

            outputFile = EditorGUILayout.TextField("Output TXT file:", outputFile);

            GUILayout.Space(10);

            if (GUILayout.Button("Export Structure"))
            {
                Export();
            }
        }

        private void Export()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var folder in folders)
            {
                if (Directory.Exists(folder))
                {
                    sb.AppendLine(folder);
                    WriteDirectory(folder, sb, 1);
                }
                else
                {
                    sb.AppendLine($"[NOT FOUND] {folder}");
                }
            }

            File.WriteAllText(outputFile, sb.ToString());
            AssetDatabase.Refresh();

            Debug.Log($"Structure exported to: {outputFile}");
        }

        private void WriteDirectory(string path, StringBuilder sb, int indent)
        {
            string indentStr = new string(' ', indent * 2);

            // Files
            foreach (var file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".meta")) continue;
                sb.AppendLine($"{indentStr}- {Path.GetFileName(file)}");
            }

            // Folders
            foreach (var dir in Directory.GetDirectories(path))
            {
                sb.AppendLine($"{indentStr}[{Path.GetFileName(dir)}]");
                WriteDirectory(dir, sb, indent + 1);
            }
        }
    }
}
#endif