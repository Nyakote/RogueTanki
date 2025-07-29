using UnityEngine;
using UnityEditor;
using System.IO;

public class MoveAllPrefabsToProps
{
    [MenuItem("Tools/Move All Prefabs to Resources/MapElements/Props")]
    static void MovePrefabs()
    {
        string[] sourceRoots = new string[]
        {
            "Assets/Resources/MapElements/ExportedProject/Assets/prefabs/proplibs",
            "Assets/Resources/MapElements/ExportedProject/Assets/prefabs/proplibswinter"
        };

        string targetFolder = "Assets/Resources/MapElements/Props";

        // Ensure Props folder exists
        if (!AssetDatabase.IsValidFolder("Assets/Resources/MapElements"))
            AssetDatabase.CreateFolder("Assets/Resources", "MapElements");

        if (!AssetDatabase.IsValidFolder(targetFolder))
            AssetDatabase.CreateFolder("Assets/Resources/MapElements", "Props");

        int movedCount = 0;

        foreach (string root in sourceRoots)
        {
            if (!Directory.Exists(root))
            {
                Debug.LogWarning($"Source folder not found: {root}");
                continue;
            }

            string[] prefabPaths = Directory.GetFiles(root, "*.prefab", SearchOption.AllDirectories);

            foreach (string fullPath in prefabPaths)
            {
                string relativePath = fullPath.Replace("\\", "/");
                string fileName = Path.GetFileName(relativePath);
                string destPath = $"{targetFolder}/{fileName}";

                if (AssetDatabase.CopyAsset(relativePath, destPath))
                {
                    Debug.Log($"✅ Copied: {fileName}");
                    movedCount++;
                }
                else
                {
                    Debug.LogWarning($"❌ Failed to copy: {fileName}");
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"🎉 {movedCount} prefabs copied to Resources/MapElements/Props");
    }
}
