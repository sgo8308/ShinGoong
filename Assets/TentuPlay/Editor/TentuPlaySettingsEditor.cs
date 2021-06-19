using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TentuPlay
{
    [CustomEditor(typeof(TentuPlaySettings))]
    public class TentuPlaySettingsEditor : UnityEditor.Editor
    {
        GUIContent apiKeyLabel = new GUIContent("Api Key [?]:", "Create or get your project credential from Project Settings at https://console.tentuplay.io");
        GUIContent secretLabel = new GUIContent("Secret [?]:", "Create or get your project credential from Project Settings at https://console.tentuplay.io");
        GUIContent debugLabel = new GUIContent("TentuPlay Debug Mode [?]:", "Run TentuPlay in debug mode");
        GUIContent autoUploadLabel = new GUIContent("Auto Upload [?]:", "Check to automatically upload the data from the client to the server.");
        GUIContent deferredSendIntervalSecLabel = new GUIContent("Upload Interval (sec) [?]:", "Minimum server upload interval (default is 1200 seconds)");
        GUIContent advicesGetInterval = new GUIContent("Advice Sync Interval (sec) [?]:", "Minimum advice sync interval (default is 600 seconds)");
        GUIContent offersGetInterval = new GUIContent("Offer Sync Interval (sec) [?]:", "Minimum offer sync interval (default is 600 seconds)");


        const string UnityAssetFolder = "Assets";

        public static TentuPlaySettings GetOrCreateSettingsAsset()
        {
            string fullPath = Path.Combine(Path.Combine(UnityAssetFolder, TentuPlaySettings.tpSettingsPath),
                               TentuPlaySettings.tpSettingsAssetName + TentuPlaySettings.tpSettingsAssetExtension
                               );
            TentuPlaySettings instance = AssetDatabase.LoadAssetAtPath(fullPath, typeof(TentuPlaySettings)) as TentuPlaySettings;

            if (instance == null)
            {
                // No asset found, create one.
                if (!Directory.Exists(Path.Combine(UnityAssetFolder, TentuPlaySettings.tpSettingsPath)))
                {
                    AssetDatabase.CreateFolder(Path.Combine(UnityAssetFolder, "TentuPlay"), "Resources");
                }

                instance = CreateInstance<TentuPlaySettings>();
                AssetDatabase.CreateAsset(instance, fullPath);
                AssetDatabase.SaveAssets();
            }
            return instance;
        }

        [MenuItem("TentuPlay/Edit Settings")]
        public static void Edit()
        {
            Selection.activeObject = GetOrCreateSettingsAsset();

            ShowInspector();
        }
        [MenuItem("TentuPlay/TentuPlay Documentation")]
        public static void OpenDoc()
        {
            Application.OpenURL("https://tentuplay.io/docs/");
        }
        [MenuItem("TentuPlay/TentuPlay Console")]
        public static void OpenConsole()
        {
            Application.OpenURL("https://console.tentuplay.io/");
        }
        void OnDisable()
        {
            TentuPlaySettings.SetInstance(null);
        }

        static TentuPlaySettingsEditor()
        {
        }

        private static void RecursiveDeleteFolders(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.GetDirectories("*", SearchOption.AllDirectories))
            {
                RecursiveDeleteFolders(dir);
            }

            try
            {
                if (baseDir.GetFiles().Length == 0)
                {
                    baseDir.Delete(true);

                    AssetDatabase.Refresh();
                }
            }
            catch
            {
            }
        }

        public override void OnInspectorGUI()
        {
            TentuPlaySettings settings = (TentuPlaySettings)target;
            TentuPlaySettings.SetInstance(settings);

            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

            bool is_disabled = TentuPlaySettings.ClientKey != null & TentuPlaySettings.ClientKey != "";

            using (new EditorGUI.DisabledScope(is_disabled))
            {
                EditorGUILayout.BeginHorizontal();
                TentuPlayAuthSettings.ApiKey = EditorGUILayout.TextField(apiKeyLabel, TentuPlayAuthSettings.ApiKey).Trim();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                TentuPlayAuthSettings.Secret = EditorGUILayout.TextField(secretLabel, TentuPlayAuthSettings.Secret).Trim();
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Get Client Key"))
                {
                    TentuPlaySettings.ClientKey = CreateClientKey(TentuPlayAuthSettings.ApiKey, TentuPlayAuthSettings.Secret);
                    if (TentuPlaySettings.ClientKey == null) // Failed to load the client key.
                    {
                        EditorUtility.DisplayDialog("TentuPlay", "ERROR. Check your Api Key or Secret.", "OK");
                    }
                    else // Success  
                    {
                        EditorUtility.DisplayDialog("TentuPlay", "Successfully Done.", "OK");
                    }
                }
            }

            if (is_disabled)
            {
                EditorGUILayout.BeginFadeGroup(1);
                EditorGUILayout.LabelField("If you want to reset client key, delete TentuPlaySettings.asset in TentuPlay>Resources and try again.", EditorStyles.helpBox);
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            TentuPlaySettings.DEBUG = EditorGUILayout.Toggle(debugLabel, TentuPlaySettings.DEBUG);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            TentuPlaySettings.AutoUpload = EditorGUILayout.Toggle(autoUploadLabel, TentuPlaySettings.AutoUpload);
            EditorGUILayout.EndHorizontal();

            if (TentuPlaySettings.AutoUpload)
            {
                EditorGUILayout.BeginHorizontal();
                TentuPlaySettings.DeferredSendIntervalSec = EditorGUILayout.IntField(deferredSendIntervalSecLabel, TentuPlaySettings.DeferredSendIntervalSec);
                //trim 
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            TentuPlaySettings.AdvicesGetInterval = EditorGUILayout.IntField(advicesGetInterval, TentuPlaySettings.AdvicesGetInterval);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            TentuPlaySettings.OffersGetInterval = EditorGUILayout.IntField(offersGetInterval, TentuPlaySettings.OffersGetInterval);
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }

        }

        private static void ShowInspector()
        {
            try
            {
                var editorAsm = typeof(UnityEditor.Editor).Assembly;
                var type = editorAsm.GetType("UnityEditor.InspectorWindow");
                Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);

                if (findObjectsOfTypeAll.Length > 0)
                {
                    ((EditorWindow)findObjectsOfTypeAll[0]).Focus();
                }
                else
                {
                    EditorWindow.GetWindow(type);
                }
            }
            catch
            {
            }
        }
        public static string CreateClientKey(string apiKey, string secret)
        {
            return TentuPlayEditorRestApi.GetClientKey(apiKey, secret);
        }
    }
}