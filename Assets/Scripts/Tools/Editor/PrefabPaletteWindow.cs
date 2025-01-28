using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad] // Cette annotation permet de s'assurer que le code s'exécute dès le démarrage de l'éditeur.
public class PrefabPaletteWindow : EditorWindow
{
    private GameObject[] _prefabsComportements;
    private GameObject[] _prefabsLD;
    private GameObject[] _prefabsAssetEnviro;
    
    Vector2 scrollPos;
    
    private GameObject selectedPrefab; // prefab selectionné qu'on va ajouter à la scene
    private GameObject parentObject;
    private Vector2 paletteScrollPos;
    private int paletteIndex = -1;    // Index de l'élément sélectionné dans la grille

    private float radius = 2f;
    private static Vector3? lastPlacedPosition = null; // Dernière position où une prefab a été placée
    private float placementOffset = 2.0f;      // Distance minimale entre deux placements
    
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
        
        
        EditorGUILayout.BeginHorizontal();
        placementOffset = EditorGUILayout.FloatField("Offset Placement",placementOffset,GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        radius = EditorGUILayout.FloatField("Radius brush",radius, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab Palette", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        
        // --- Liste déroulante ---
        
        #region Liste déroulante
        
        // EditorGUILayout.BeginHorizontal();
        // scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(200));
        // int prefabsInRow = 5;
        // int currentIndex = 0;
        // EditorGUILayout.BeginVertical(); // Pour lignes d'affichage des préfabs
        //
        // foreach (GameObject prefab in _prefabsComportements)
        // {
        //     // tous les "prefabsInRow" prefabs, on fait une nouvelle ligne
        //     if (currentIndex % prefabsInRow == 0)
        //     {
        //         // nouvelle ligne
        //         if (currentIndex > 0)
        //         {
        //             EditorGUILayout.EndHorizontal();
        //         }
        //         EditorGUILayout.BeginHorizontal();
        //     }
        //     
        //     
        //     // Afficher l'aperçu
        //     EditorGUILayout.BeginVertical();
        //     
        //     Texture2D preview = AssetPreview.GetAssetPreview(prefab);
        //     if (preview != null)
        //     {
        //         if (GUILayout.Button(preview, GUILayout.Width(80), GUILayout.Height(80)))
        //         {
        //             Debug.Log($"Prefab clicked: {prefab.name}");
        //             selectedPrefab = prefab;
        //         }
        //         
        //     }
        //     GUILayout.Label(prefab.name, GUILayout.Width(80));
        //     EditorGUILayout.EndVertical();
        //     
        //     currentIndex++;
        // }
        //
        // // ligne ouverte, on la ferme, car on a plus rien à afficher
        // if (currentIndex % prefabsInRow != 0)
        // {
        //     EditorGUILayout.EndHorizontal();
        // }
        //
        // EditorGUILayout.EndVertical();
        // //
        // EditorGUILayout.EndScrollView();
        // EditorGUILayout.EndHorizontal();
        // //
        //
        // EditorGUILayout.BeginVertical();
        
        #endregion
        
        
        // liste avec Grid
        List<GUIContent> paletteIcons = new List<GUIContent>();
        foreach (GameObject prefab in _prefabsComportements)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            paletteIcons.Add(new GUIContent(texture, prefab.name));
        }
        
        // EditorGUILayout.BeginVertical();
        paletteScrollPos = EditorGUILayout.BeginScrollView(paletteScrollPos, GUILayout.Width(400), GUILayout.Height(200));//GUILayout.ExpandWidth(true)
        
        int newPaletteIndex = GUILayout.SelectionGrid(paletteIndex, paletteIcons.ToArray(), 4, GUILayout.Width(400), GUILayout.Height(300));
        
        if (newPaletteIndex != paletteIndex)
        {
            paletteIndex = newPaletteIndex;
            selectedPrefab = _prefabsComportements[paletteIndex]; // selection de la prefab
            Debug.Log($"Prefab selected: {selectedPrefab.name}");
        }
        
        EditorGUILayout.EndScrollView(); 
        
        // EditorGUILayout.EndVertical();

        
    }
    
    private void OnSceneGUI(SceneView SceneView)
    {
        
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        #region Visuel de la range

        if (e.type == EventType.MouseMove || e.type == EventType.Repaint) //
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(hit.point, hit.normal, radius);
                
                Handles.color = Color.blue;
                Handles.DrawLine(hit.point, hit.point + hit.normal * 2);
                SceneView.RepaintAll();
            }
        }

        #endregion


        if (e.type == EventType.MouseMove || e.type == EventType.Repaint)
        {

            // Afficher un rayon à partir de la caméra vers la souris dans la scène
            Handles.color = Color.red; // Choisir la couleur du rayon
            Handles.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Infinity); // Dessine une ligne de longueur 10 à partir du rayon
        }
        
        
        // Vérification si la touche Control est enfoncée
        if (e.type == EventType.MouseMove && e.control)
        {
            
        }
        
        //EventType.MouseDown
        
        if (selectedPrefab == null) return; // Aucun prefab sélectionné
        
        // verif si clic dans scène
        if (e.type == EventType.MouseDrag && e.button == 0 && e.control && !e.alt) //control modificateur, pas A
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;
                
                //verif si on peut placer la prefabs
                if (!lastPlacedPosition.HasValue || Vector3.Distance(lastPlacedPosition.Value, hitPoint) >= placementOffset)
                {
                    // Set une position aleatoire autour de hitPoint
                    Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;
                    Vector3 randomPosition = hitPoint + new Vector3(randomOffset.x, 0, randomOffset.y);
                    
                   // création instance du prefab à l'emplacement du clic
                   GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                   // InstantiatePrefab permet de concerver le lien prefab et pas une version indépendante (clone) comme le Instantiate de base
                   instance.transform.up = hit.normal;
                   instance.transform.position = randomPosition; // Place à la position aléatoire
                   
                   // set le parent
                   if (parentObject)
                       instance.transform.SetParent(parentObject.transform, true);
   
                   Undo.RegisterCreatedObjectUndo(instance, "Place Prefab"); // ctrl Z
                   Debug.Log($"Prefab placed at: {hitPoint}");
                   
                   // maj dernière position placée
                   lastPlacedPosition = hitPoint;
                }
                
                
            }
        
            e.Use(); // on indique que l'event est utilisé
        }
        else if (e.type == EventType.MouseUp && e.button == 0)
        {
            // reset dernière position quand on relache la souris
            lastPlacedPosition = null;
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
        paletteIndex = -1;
    }


}
