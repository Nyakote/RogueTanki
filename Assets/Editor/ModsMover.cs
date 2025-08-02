using UnityEditor;
using UnityEngine;
using System.IO;

public class ModsMover : EditorWindow
{
    string sourceRootFolder = "Assets/TO-Assets-main/Turrets/";
    string targetSkinFolder = "Assets/Resources/Turrets/Skins";

    [MenuItem("Tools/Import Hull Mods")]
    public static void ShowWindow()
    {
        GetWindow<ModsMover>("Import Hull Mods");
    }

    void OnGUI()
    {
        GUILayout.Label("Import and Rename PNGs", EditorStyles.boldLabel);

        sourceRootFolder = EditorGUILayout.TextField("Source Root Folder", sourceRootFolder);
        targetSkinFolder = EditorGUILayout.TextField("Target Skin Folder", targetSkinFolder);

        if (GUILayout.Button("Move and Rename PNGs"))
        {
            MovePNGs();
        }
    }

    void MovePNGs()
    {
        if (!Directory.Exists(sourceRootFolder))
        {
            Debug.LogError("Source folder does not exist: " + sourceRootFolder);
            return;
        }

        Directory.CreateDirectory(targetSkinFolder);

        var hullFolders = Directory.GetDirectories(sourceRootFolder);

        int movedCount = 0;

        foreach (string hullFolder in hullFolders)
        {
            string hullName = new DirectoryInfo(hullFolder).Name;

            var modFolders = Directory.GetDirectories(hullFolder);

            foreach (string modFolder in modFolders)
            {
                string modName = new DirectoryInfo(modFolder).Name;

                string sourceImagePath = Path.Combine(modFolder, "details.png");

                if (File.Exists(sourceImagePath))
                {
                    // Compose destination filename: Hull_Mod.png
                    string destFileName = $"{hullName}_{modName}.png";
                    string destPath = Path.Combine(targetSkinFolder, destFileName);

                    try
                    {
                        File.Copy(sourceImagePath, destPath, overwrite: true);
                        Debug.Log($"Copied {sourceImagePath} → {destPath}");
                        movedCount++;
                    }
                    catch (IOException e)
                    {
                        Debug.LogError($"Failed to copy {sourceImagePath}: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Missing 'details.png' in {modFolder}");
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"✅ Moved and renamed {movedCount} files.");
    }
}
