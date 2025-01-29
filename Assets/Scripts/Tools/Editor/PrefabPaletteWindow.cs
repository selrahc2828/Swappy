using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

    
    private BrushToolData brushToolData;
    private static Vector3? lastPlacedPosition = null; // Dernière position où une prefab a été placée
    
    private LayerMask hitLayer;
    
    [MenuItem("Tools/Prefab Palette")]
    private static void ShowWindow()
    {
        var window = GetWindow<PrefabPaletteWindow>();
        window.titleContent = new GUIContent("Prefab Palette");
        window.Show();
    }

    private void OnEnable()
    {
        LoadBrushToolData();
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
        brushToolData.placementOffset = EditorGUILayout.FloatField("Offset Placement",brushToolData.placementOffset,GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        brushToolData.radius = EditorGUILayout.FloatField("Radius brush",brushToolData.radius, GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();
        
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Random scale : ", GUILayout.Width(100));
        
        EditorGUILayout.BeginVertical(GUILayout.Width(350));
        brushToolData.minScale = EditorGUILayout.Slider("Min Scale", brushToolData.minScale, 0.1f, brushToolData.maxScale);
        brushToolData.maxScale = EditorGUILayout.Slider("Max Scale", brushToolData.maxScale, brushToolData.minScale, 10f);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        
        
        // EditorGUILayout.BeginHorizontal();
        // hitLayer = EditorGUILayout.LayerField("Hit Mask", hitLayer);
        // EditorGUILayout.EndHorizontal();

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
        
    }
    
    private void OnSceneGUI(SceneView SceneView)
    {
        float radius = brushToolData.radius;
        float placementOffset = brushToolData.placementOffset;
        
        Event e = Event.current;
        
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;
        
        #region Visuel de la range

        if (e.type == EventType.MouseMove || e.type == EventType.Repaint) //
        {
            if (Physics.Raycast(ray, out  hit, Mathf.Infinity)) 
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(hit.point, hit.normal, radius);
                
                Handles.color = Color.blue;
                Handles.DrawLine(hit.point, hit.point + hit.normal * 2);
                SceneView.RepaintAll();
            }
        }

        #endregion
        
        //EventType.MouseDown
        
        if (selectedPrefab == null) return; // Aucun prefab sélectionné
        
        // verif si clic dans scène / clic gauche + ctrl enfoncé
        if (e.type == EventType.MouseDrag && e.button == 0 && e.control && !e.alt) //control modificateur, pas A
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                Vector3 hitPoint = hit.point;
                
                // offset aléatoire en fonction du radius (par rapport à l'écran donc vecteur 2D)
                Vector2 randomOffset = Random.insideUnitCircle * radius;  // Génère un vecteur dans un cercle
                Vector3 offsetPosition = new Vector3(randomOffset.x, 0, randomOffset.y); // Position offset en X et Z
                
                Ray placementRay = new Ray(ray.origin+offsetPosition, ray.direction);  // Créer le rayon avec l'offset

                RaycastHit placementHit;

                if (Physics.Raycast(placementRay, out placementHit, Mathf.Infinity)) // Utiliser le LayerMask pour filtrer les surfaces
                {
                    //verif si on peut placer la prefabs
                    if (!lastPlacedPosition.HasValue || Vector3.Distance(lastPlacedPosition.Value, placementHit.point) >= placementOffset)
                    {
                        #region Placement Prefab
                        // Set une position aleatoire autour de hitPoint

                        // création instance du prefab à l'emplacement du clic
                        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                        
                        // InstantiatePrefab permet de concerver le lien prefab et pas une version indépendante (clone) comme le Instantiate de base
                        instance.transform.up = placementHit.normal;
                        instance.transform.position = placementHit.point;
               
                        // scale aléatoire
                        float randomScale = Random.Range(brushToolData.minScale, brushToolData.maxScale);
                        instance.transform.localScale = Vector3.one * randomScale;
                        
                        // set le parent
                        if (parentObject)
                            instance.transform.SetParent(parentObject.transform, true);

                        Undo.RegisterCreatedObjectUndo(instance, "Place Prefab"); // ctrl Z
                        Debug.Log($"Prefab placed at: {placementHit.point}");
               
                        
                        // maj dernière position placée
                        lastPlacedPosition = placementHit.point;                      
                    
                    
                        #endregion
                    }  
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
    
    private void LoadBrushToolData()
    {
        string path = "Assets/ScriptableDatas/Tool/BrushToolData.asset";//dossier à changer
        
        brushToolData = AssetDatabase.LoadAssetAtPath<BrushToolData>(path);

        if (brushToolData == null)
        {
            // si existe pas, on en créer un nouveau
            brushToolData = CreateInstance<BrushToolData>();
            AssetDatabase.CreateAsset(brushToolData, path);
            AssetDatabase.SaveAssets();
            Debug.Log("BrushToolData créé et sauvegardé à : " + path);
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
