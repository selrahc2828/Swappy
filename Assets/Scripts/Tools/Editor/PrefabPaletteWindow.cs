using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class PrefabPaletteWindow : EditorWindow
{
    private GameObject[] _prefabsComportements;
    private GameObject[] _prefabsLD;
    private GameObject[] _prefabsAssetEnviro;
    
    [MenuItem("Tools/Prefab Palette")]
    private static void ShowWindow()
    {
        var window = GetWindow<PrefabPaletteWindow>();
        window.titleContent = new GUIContent("Prefab Palette");
        window.Show();
    }

    private void OnEnable()
    {
        LoadAllAssetsOfType(out _prefabsComportements, "Assets/Prefabs/Comportement");
    }

    private void OnGUI()
    {
        //Debug.Log($"Open Prefab Palette et lenght: {_prefabs.Length}");
        
        foreach (GameObject prefab in _prefabsComportements)
        {
            Debug.Log(prefab.name);
        }
    }
    
    private static void LoadAllAssetsOfType<T>(out T[] assets, string path) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] {path});
        // string[] guids = AssetDatabase.FindAssets("t:"+typeof(T));
        assets = new T[guids.Length];
        Debug.Log($"Load All Assets of type: {typeof(T)}, {guids.Length}");
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }
}
