using UnityEditor;
using UnityEngine;
using System.IO;

public class JpegMOver : EditorWindow
{
    string sourceRootFolder = "Assets/TO-Assets-main/Inventory/Paints";  
    string targetSkinFolder = "Assets/Resources/Paints/Skins";
    string targetPreviewFolder = "Assets/Resources/Paints/Previews";

    [MenuItem("Tools/Import Skins & Previews")]
    public static void ShowWindow()
    {
        GetWindow<JpegMOver>("Import Skins");
    }

    void OnGUI()
    {
        GUILayout.Label("Import and Rename PNGs", EditorStyles.boldLabel);

        sourceRootFolder = EditorGUILayout.TextField("Source Root Folder", sourceRootFolder);
        targetSkinFolder = EditorGUILayout.TextField("Target Skin Folder", targetSkinFolder);
        targetPreviewFolder = EditorGUILayout.TextField("Target Preview Folder", targetPreviewFolder);

        if (GUILayout.Button("Move and Rename PNGs"))
        {
            MoveAndRenamePNGs();
        }
    }

    void MoveAndRenamePNGs()
    {
        if (!Directory.Exists(sourceRootFolder))
        {
            Debug.LogError("Source folder does not exist: " + sourceRootFolder);
            return;
        }

        Directory.CreateDirectory(targetSkinFolder);
        Directory.CreateDirectory(targetPreviewFolder);

        var subfolders = Directory.GetDirectories(sourceRootFolder);

        int movedCount = 0;

        foreach (string folder in subfolders)
        {
            string folderName = new DirectoryInfo(folder).Name;

            string imagePath = Path.Combine(folder, "image.jpg");
            string previewPath = Path.Combine(folder, "preview.png");

            string targetImagePath = Path.Combine(targetSkinFolder, $"{folderName}_image.jpg");
            string targetPreviewPath = Path.Combine(targetPreviewFolder, $"{folderName}_preview.png");

            if (File.Exists(imagePath))
            {
                File.Copy(imagePath, targetImagePath, overwrite: true);
                Debug.Log($"Copied {imagePath} → {targetImagePath}");
                movedCount++;
            }
            else
            {
                Debug.LogWarning($"Missing image.png in folder {folderName}");
            }

            if (File.Exists(previewPath))
            {
                File.Copy(previewPath, targetPreviewPath, overwrite: true);
                Debug.Log($"Copied {previewPath} → {targetPreviewPath}");
                movedCount++;
            }
            else
            {
                Debug.LogWarning($"Missing preview.png in folder {folderName}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"✅ Moved and renamed {movedCount} files.");
    }
}
