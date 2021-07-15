
using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ExportPackage
{
    [MenuItem("Export/Export with tags and layers, Input settings")]
    public static void export()
    {
        //string[] projectContent = new string[] { "Assets", "Packages/Cinemachine","ProjectSettings/TagManager.asset", "ProjectSettings/InputManager.asset", 
        //    "ProjectSettings/ProjectSettings.asset", "ProjectSettings/Physics2DSettings.asset","ProjectSettings/PackageManagerSettings.asset", "ProjectSettings/EditorBuildSettings.asset"  };

        string[] projectContent = new string[] { "Assets","ProjectSettings", };
        AssetDatabase.ExportPackage(projectContent, "Done.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies | ExportPackageOptions.IncludeLibraryAssets);
        Debug.Log("Project Exported");
    }
}