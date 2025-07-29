using UnityEditor;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class AnalyzeMapWindow : EditorWindow
{
    private TextAsset xmlFile;
    private DefaultAsset prefabFolder;

    [MenuItem("Tools/Map Prefab Analyzer")]
    public static void ShowWindow()
    {
        GetWindow<AnalyzeMapWindow>("Map Analyzer");
    }

    private void OnGUI()
    {
        GUILayout.Label("🔍 XML Map Prefab Analyzer", EditorStyles.boldLabel);

        xmlFile = (TextAsset)EditorGUILayout.ObjectField("Map XML File", xmlFile, typeof(TextAsset), false);
        prefabFolder = (DefaultAsset)EditorGUILayout.ObjectField("Prefabs Folder", prefabFolder, typeof(DefaultAsset), false);

        if (GUILayout.Button("Analyze Map"))
        {
            if (xmlFile == null || prefabFolder == null)
            {
                Debug.LogError("❌ Please assign both XML and prefab folder.");
                return;
            }

            AnalyzeMap(xmlFile, AssetDatabase.GetAssetPath(prefabFolder));
        }
    }

    private void AnalyzeMap(TextAsset xml, string prefabFolderPath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.text);

        XmlNodeList propNodes = xmlDoc.GetElementsByTagName("prop");
        HashSet<string> usedProps = new HashSet<string>();

        foreach (XmlNode prop in propNodes)
        {
            string name = prop.Attributes["name"].Value.Trim();
            usedProps.Add(name);
        }

        string[] prefabFiles = Directory.GetFiles(prefabFolderPath, "*.prefab", SearchOption.TopDirectoryOnly);
        HashSet<string> availablePrefabs = new HashSet<string>();

        foreach (string file in prefabFiles)
        {
            string prefabName = Path.GetFileNameWithoutExtension(file);
            availablePrefabs.Add(prefabName);
        }

        List<string> missing = new List<string>();

        foreach (string name in usedProps)
        {
            if (!availablePrefabs.Contains(name))
                missing.Add(name);
        }


        Debug.Log($"📦 Used in XML: {usedProps.Count}, Found: {usedProps.Count - missing.Count}, Missing: {missing.Count}");
        if (missing.Count > 0)
        {
            Debug.Log("❌ Missing prefabs:");
            foreach (var m in missing) Debug.Log($"❌ {m}");
        }

      

        Debug.Log("✅ Map analysis complete.");
    }
}
