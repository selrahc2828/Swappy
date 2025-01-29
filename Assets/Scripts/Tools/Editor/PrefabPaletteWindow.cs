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

    private float radius = 2f;
    private static Vector3? lastPlacedPosition = null; // Dernière position où une prefab a été placée
    private float placementOffset = 2.0f;      // Distance minimale entre deux placements
    
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
    private void RaycastWithAngle(Event e, float angleOffset)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Quaternion rotation = Quaternion.Euler(angleOffset, angleOffset, angleOffset);
        Vector3 customDirection = rotation * ray.direction;
        Ray customRay = new Ray(ray.origin, customDirection);
        
        Handles.DrawWireCube(customRay.origin, Vector3.one);
        
        if (Physics.Raycast(customRay, out RaycastHit hit, Mathf.Infinity))
        {
            Handles.color = Color.red;
            Handles.DrawWireCube(hit.point, Vector3.one * 5f);
            Handles.color = Color.green;
            Handles.DrawLine(customRay.origin, hit.point); // Affiche la ligne dans la scène
        }
    }

    private Vector3 GetRandomDirection3D(float radius)
    {
        // Générer une direction aléatoire dans une sphère unitaire
        Vector3 randomDirection = Random.onUnitSphere;

        // Multiplier par le rayon pour ajuster la longueur
        return randomDirection * radius;
    }

    private void RaycastWithRandomAngle(Event e, float radius)
    {
        // Tirer un raycast avec la position de la souris
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Le point d'impact où la souris a touché l'objet
            Vector3 hitPoint = hit.point;
            Vector3 normal = hit.normal; // La normale à la surface

            // Dessiner un cercle autour du point d'impact pour visualiser le rayon
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(hitPoint, normal, radius);

            // Tirer des raycasts autour du hitpoint
            for (int i = 0; i < 5; i++)
            {
                // Générer un offset aléatoire dans toutes les directions (sphère de rayon donné)
                Vector3 randomOffset = Random.onUnitSphere * radius;

                // Créer une position modifiée en tenant compte de la normale
                Vector3 randomPosition = hitPoint + randomOffset;

                // Créer un raycast qui part du hitpoint mais à la position décalée
                RaycastHit randomHit;
                if (Physics.Raycast(randomPosition, normal, out randomHit))
                {
                    // Afficher les raycasts supplémentaires (à ajuster selon les besoins)
                    Debug.DrawLine(randomPosition, randomHit.point, Color.red, 1.0f);
                }
            }

            // Forcer la scène à se rafraîchir
            SceneView.RepaintAll();
        }
    }

    
    private void OnSceneGUI(SceneView SceneView)
    {
        
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


        if (e.type == EventType.MouseMove || e.type == EventType.Repaint)
        {
            // Afficher un rayon à partir de la caméra vers la souris dans la scène
            Handles.color = Color.red; // Choisir la couleur du rayon
            Handles.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Infinity); // Dessine une ligne de longueur 10 à partir du rayon
        }
        
        //EventType.MouseDown
        
        if (selectedPrefab == null) return; // Aucun prefab sélectionné
        
        // verif si clic dans scène / clic gauche + ctrl enfoncé
        if (e.type == EventType.MouseDrag && e.button == 0 && e.control && !e.alt) //control modificateur, pas A
        {
            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                Vector3 hitPoint = hit.point;
                Vector3 normal = hit.normal;  // Normale de la surface
                
                // Générer un offset aléatoire basé sur le rayon pour avoir une variation dans le placement
                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * radius;  // Génère un vecteur dans un cercle
                Vector3 randomPosition = hitPoint + new Vector3(randomOffset.x, 0, randomOffset.y);  // Ajoute l'offset à la position
                Vector3 offsetPosition = new Vector3(randomOffset.x, 0, randomOffset.y); // Position offset en X et Z

                // Ray placementRay = new Ray(hitPoint + new Vector3(randomOffset.x, 0, randomOffset.y), normal);  // Créer le rayon avec l'offset
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
