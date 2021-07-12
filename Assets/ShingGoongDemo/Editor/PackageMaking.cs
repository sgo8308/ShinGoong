
using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ExportPackage
{
    [MenuItem("Export/Export with tags and layers, Input settings")]
    public static void export()
    {
        string[] projectContent = new string[] { "Assets", "Packages/Cinemachine","ProjectSettings/TagManager.asset", "ProjectSettings/InputManager.asset", 
            "ProjectSettings/ProjectSettings.asset", "ProjectSettings/Physics2DSettings.asset","ProjectSettings/PackageManagerSettings.asset" };
        AssetDatabase.ExportPackage(projectContent, "Done2.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }
}