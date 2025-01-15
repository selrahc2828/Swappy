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
    
    Vector2 scrollPos;
    
    private GameObject selectedPrefab; // prefab selectionné qu'on va ajouter à la scene
    private GameObject parentObject;
    
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
        
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        //Debug.Log($"Open Prefab Palette et lenght: {_prefabs.Length}");
        
        if (GUILayout.Button("Create Parent",GUILayout.Width(150)))
        {
            if (parentObject == null)
                CreateParentObject();
        }
        
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Prefab Select",GUILayout.Width(150)))
        {
            ClearSelection();
        }
        if (selectedPrefab != null)
            EditorGUILayout.LabelField(selectedPrefab.gameObject.name, GUILayout.Width(100));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab Palette", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        
        // --- Liste déroulante ---
        
        EditorGUILayout.BeginHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(200));
        int prefabsInRow = 5;
        int currentIndex = 0;
        EditorGUILayout.BeginVertical(); // Pour lignes d'affichage des préfabs
        
        foreach (GameObject prefab in _prefabsComportements)
        {
            // tous les "prefabsInRow" prefabs, on fait une nouvelle ligne
            if (currentIndex % prefabsInRow == 0)
            {
                // nouvelle ligne
                if (currentIndex > 0)
                {
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.BeginHorizontal();
            }
            
            
            // Afficher l'aperçu
            EditorGUILayout.BeginVertical();
            
            Texture2D preview = AssetPreview.GetAssetPreview(prefab);
            if (preview != null)
            {
                if (GUILayout.Button(preview, GUILayout.Width(80), GUILayout.Height(80)))
                {
                    Debug.Log($"Prefab clicked: {prefab.name}");
                    selectedPrefab = prefab;
                }
                
            }
            GUILayout.Label(prefab.name, GUILayout.Width(80));
            EditorGUILayout.EndVertical();
            
            currentIndex++;
        }
        
        // ligne ouverte, on la ferme, car on a plus rien à afficher
        if (currentIndex % prefabsInRow != 0)
        {
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
        
    }
    
    private void OnSceneGUI(SceneView SceneView)
    {
        if (selectedPrefab == null) return; // Aucun prefab sélectionné
        
        Event e = Event.current;
        
        // verif si clic dans scène
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                
                // création instance du prefab à l'emplacement du clic
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                // InstantiatePrefab permet de concerver le lien prefab et pas une version indépendante (clone) comme le Instantiate de base
                instance.transform.up = hit.normal;
                instance.transform.position = hit.point;
                
                // set le parent
                if (parentObject)
                    instance.transform.SetParent(parentObject.transform, true);

                Undo.RegisterCreatedObjectUndo(instance, "Place Prefab"); // ctrl Z
                Debug.Log($"Prefab placed at: {hit.point}");
            }
        
            e.Use(); // on indique que l'event est utilisé
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
    private void CreateParentObject()
    {
        GameObject newParent = new GameObject("---Enviro---");
        Undo.RegisterCreatedObjectUndo(newParent, "Create Parent Object"); //ctrl Z

        parentObject = newParent;
        // Selection.activeGameObject = newParent; // select l'objet dans la hiérarchie
    }

    private void ClearSelection()
    {
        selectedPrefab = null;
    }
}
