using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SceneManagerWindow : EditorWindow
{
    private SceneData[] scenes;
    private Vector2 scrollPos;
    private bool playMode;
    
    [MenuItem("Tools/SceneManager")]
    private static void ShowWindow()
    {
        var window = GetWindow<SceneManagerWindow>();
        window.titleContent = new GUIContent("Scene Manager");
        window.Show();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += PlayMode;
        LoadAllAssetsOfType<SceneData>(out scenes);
        DetectNewScenes();
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        EditorApplication.playModeStateChanged -= PlayMode;
    }
    private void OnProjectChange()
    {
        // Detect new scenes on project change
        DetectNewScenes();
    }

    private void DetectNewScenes()
    {
        // Get the current list of all scene files in the project
        string[] allScenePaths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".unity")).ToArray();

        // Load all SceneData ScriptableObjects
        LoadAllAssetsOfType<SceneData>(out scenes);

        // Check for scenes without corresponding ScriptableObject
        foreach (string scenePath in allScenePaths)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        
            // Check if a corresponding SceneData ScriptableObject exists
            bool hasScriptableObject = scenes.Any(scene => 
                System.IO.Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(scene)) == sceneName);

            if (!hasScriptableObject)
            {
                AddMissingScriptableSenes(sceneName);
                LoadAllAssetsOfType<SceneData>(out scenes);
            }
        }
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        //foreach SceneData scriptable object
        foreach(var sceneData in scenes)
        {
            // Create a SerializedObject for the current scene : 
            SerializedObject so = new SerializedObject(sceneData);
            so.Update();
            
            SerializedProperty isOpen = so.FindProperty("isOpen");
            SerializedProperty isPersistent = so.FindProperty("isPersistent");
            
            string sceneDataPath = $"Assets/Data/Scene/{sceneData.name}.asset";

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(sceneData.name, GUILayout.Width(100));
            if (isOpen.boolValue)
            {
                Color original = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                // Add buttons or functionality here for managing scenes (e.g., open, close, load)
                if (GUILayout.Button("Close", GUILayout.Width(100)))
                {
                    isOpen.boolValue = false;
                    EditorSceneManager.CloseScene(SceneManager.GetSceneByName(sceneData.name), true);
                }
                GUI.backgroundColor = original;
            }
            else
            {
                Color original = GUI.backgroundColor;
                GUI.backgroundColor = Color.green;
                // Add buttons or functionality here for managing scenes (e.g., open, close, load)
                if (GUILayout.Button("Open", GUILayout.Width(100)))
                {
                    isOpen.boolValue = true;
                    LoadSceneByName(sceneData.name);
                }

                GUI.backgroundColor = original;
            }

            EditorGUILayout.LabelField("Persistent", GUILayout.Width(70));
            isPersistent.boolValue = EditorGUILayout.Toggle(isPersistent.boolValue);
            
            EditorGUILayout.EndHorizontal();
            so.ApplyModifiedProperties();
        }
        EditorGUILayout.EndScrollView();
    }
    
    private static void LoadAllAssetsOfType<T>(out T[] assets) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets("t:"+typeof(T));
        assets = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }
    
    public void PlayMode(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            LoadAllAssetsOfType<SceneData>(out scenes);
            //foreach SceneData scriptable object
            foreach(var sceneData in scenes)
            {
                // Create a SerializedObject for the current scene : 
                SerializedObject so = new SerializedObject(sceneData);
            
                SerializedProperty isOpen = so.FindProperty("isOpen");
                SerializedProperty isPersistent = so.FindProperty("isPersistent");

                var sceneName = sceneData.name;
                Debug.Log(sceneName);
                if (isPersistent.boolValue)
                {
                    Debug.Log(sceneName);
                    LoadSceneByName(sceneName);
                }
                else
                {
                    EditorSceneManager.CloseScene(SceneManager.GetSceneByName(sceneName), true);
                }
            }
        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            LoadAllAssetsOfType<SceneData>(out scenes);
            //foreach SceneData scriptable object
            foreach(var sceneData in scenes)
            {
                // Create a SerializedObject for the current scene : 
                SerializedObject so = new SerializedObject(sceneData);
            
                SerializedProperty isOpen = so.FindProperty("isOpen");
                SerializedProperty isPersistent = so.FindProperty("isPersistent");
                var sceneName = sceneData.name;
                
                if (isOpen.boolValue)
                {
                    LoadSceneByName(sceneName);
                }
                else
                {
                    EditorSceneManager.CloseScene(SceneManager.GetSceneByName(sceneName), true);
                }
            }
        }
    }
    
    private void LoadSceneByName(string loadSceneName)
    {
        string[] guids = AssetDatabase.FindAssets(loadSceneName + " t:Scene");

        if (guids.Length == 0)
        {
            Debug.LogError("Scene not found: " + loadSceneName);
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
    }
    
    private void AddMissingScriptableSenes(string newScriptableName)
    {
        var sceneDataName = newScriptableName;
        var sceneDataPath = $"Assets/Data/Scene/{sceneDataName}.asset";
        
        SceneData newScene = CreateInstance<SceneData>();
        newScene.name = sceneDataName;
        newScene.isOpen = false;
        newScene.isPersistent = false;
        AssetDatabase.CreateAsset(newScene, $"Assets/Data/Scenes/{sceneDataName}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
