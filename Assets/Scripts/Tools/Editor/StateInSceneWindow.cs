using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StateInSceneWindow : EditorWindow
{
    private ComportementsStateMachine stateMachineScript; 
    private int selectedStateIndex = 0;
    private string[] stateNames = Array.Empty<string>(); // Noms des états disponibles
    private int[] stateValues = Array.Empty<int>(); // Valeurs des états
    private ComportementsStateMachine[] listeObjectsInThisState = Array.Empty<ComportementsStateMachine>();
    private Dictionary<ComportementsStateMachine, int> objectStateSelections = new Dictionary<ComportementsStateMachine, int>();

    [MenuItem("Tools/State In Scene Window")]
    public static void ShowWindow()
    {
        GetWindow<StateInSceneWindow>("State In Scene Window");
        
    }

    private void OnEnable()
    {
        UpdateStateList();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    
    private void UpdateStateList()
    {
        // Récupérer les noms et valeurs de l'énumération FirstState
        var enumValues = Enum.GetValues(typeof(FirstState)).Cast<FirstState>().ToArray();
        stateNames = enumValues.Select(v => AddSpacesBeforeUpperCase(v.ToString())).ToArray();
        stateValues = enumValues.Select(v => (int)v).ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("Sélectionnez un état :", EditorStyles.boldLabel);

        // Affichage de la liste déroulante
        if (stateNames != null && stateNames.Length > 0)
        {
            selectedStateIndex = EditorGUILayout.Popup("État :", selectedStateIndex, stateNames);

            if (GUILayout.Button("Rechercher", GUILayout.Height(30)))
            {
                listeObjectsInThisState = SearchEveryObjectWithThisState(stateValues[selectedStateIndex], stateNames[selectedStateIndex]);
                listeObjectsInThisState = listeObjectsInThisState.Where(obj => obj != null).ToArray();

                // Initialiser les sélections d'état pour chaque objet trouvé
                objectStateSelections = listeObjectsInThisState.ToDictionary(obj => obj, obj => Array.IndexOf(stateValues, (int)obj.initialState));
            }

            // Affichage des résultats
            if (listeObjectsInThisState != null && listeObjectsInThisState.Length > 0)
            {
                GUILayout.Label("Objets trouvés :", EditorStyles.boldLabel);

                foreach (var obj in listeObjectsInThisState)
                {
                    if (obj == null)
                    {
                        GUILayout.Label("Objet manquant ou détruit.", EditorStyles.helpBox);
                        continue; // Passe à l'objet suivant
                    }

                    // Début du cadre
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    GUILayout.Label($"Nom : {obj.name}", EditorStyles.boldLabel);

                    // Menu déroulant pour changer l'état de l'objet
                    if (!objectStateSelections.ContainsKey(obj))
                    {
                        objectStateSelections[obj] = Array.IndexOf(stateValues, (int)obj.initialState);
                    }

                    EditorGUILayout.BeginHorizontal();

                    objectStateSelections[obj] = EditorGUILayout.Popup(objectStateSelections[obj], stateNames);

                    if (GUILayout.Button("Appliquer", GUILayout.Width(100)))
                    {
                        int newStateValue = stateValues[objectStateSelections[obj]];

                        if (EditorApplication.isPlaying)
                        {
                            // En mode Play, appeler CalculateNewState
                            if (obj.currentState is ComportementState comportementState)
                            {
                                comportementState.CalculateNewtState(newStateValue);
                                Debug.Log($"{obj.name} : Current State changé à {stateNames[objectStateSelections[obj]]} en mode Play.");
                            }
                            else
                            {
                                Debug.LogWarning($"{obj.name} : Impossible de changer le Current State, l'état actuel n'est pas un ComportementState.");
                            }
                        }
                        else
                        {
                            // Hors mode Play, changer l'état initial
                            obj.initialState = (FirstState)newStateValue;
                            Debug.Log($"{obj.name} : Initial State changé à {stateNames[objectStateSelections[obj]]} hors mode Play.");
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.BeginHorizontal();

                    // Position modifiable
                    GUILayout.Label("Position :", EditorStyles.boldLabel);
                    if (GUILayout.Button("Focus", GUILayout.Width(100)))
                    {
                        // Focus sur l'objet
                        EditorGUIUtility.PingObject(obj.gameObject);
                        SceneView.lastActiveSceneView.Frame(obj.gameObject.GetComponent<Renderer>().bounds, false);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    
                    Vector3 newPosition = EditorGUILayout.Vector3Field("", obj.transform.position);
                    if (newPosition != obj.transform.position)
                    {
                        Undo.RecordObject(obj.transform, "Change Position"); // Permet d'annuler le changement dans l'historique Unity
                        obj.transform.position = newPosition;
                    }

                    // Fin du cadre
                    EditorGUILayout.EndVertical();

                    GUILayout.Space(10); // Espacement entre les cadres
                }
            }
            else
            {
                GUILayout.Label("Aucun objet trouvé pour cet état.", EditorStyles.helpBox);
            }
        }
        else
        {
            GUILayout.Label("Aucun état disponible.", EditorStyles.helpBox);
        }
    }
    
    private string AddSpacesBeforeUpperCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        return System.Text.RegularExpressions.Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }
    private ComportementsStateMachine[] SearchEveryObjectWithThisState(int stateValue, string stateName)
    {
        // Vérifie si Unity est en mode Play
        if (EditorApplication.isPlaying)
        {
            // Recherche les objets ayant cet état actuel
            Debug.Log($"Recherche des objets avec l'état actuel : {stateName} ({stateValue}) en mode Play.");
            return FindObjectsOfType<ComportementsStateMachine>()
                .Where(obj => obj.currentState != null && GetStateValue(obj.currentState) == stateValue)
                .ToArray();
        }
        else
        {
            // Recherche les objets ayant cet état initial
            Debug.Log($"Recherche des objets avec l'état initial : {stateName} ({stateValue}) hors mode Play.");
            return FindObjectsOfType<ComportementsStateMachine>()
                .Where(obj => (int)obj.initialState == stateValue)
                .ToArray();
        }
    }
    private int GetStateValue(State state)
    {
        if (state is ComportementState comportementState)
        {
            return comportementState.stateValue;
        }
        return -1; // Valeur par défaut si l'état n'est pas un ComportementState
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        if (listeObjectsInThisState == null || listeObjectsInThisState.Length == 0) return;

        foreach (var obj in listeObjectsInThisState)
        {
            if (obj == null) continue;

            // Dessiner un handle de position pour chaque objet
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(obj.transform.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                // Enregistrer la position pour permettre d'annuler
                Undo.RecordObject(obj.transform, "Move Object");
                obj.transform.position = newPosition;
            }
        }
        
        if (listeObjectsInThisState == null || listeObjectsInThisState.Length == 0) return;

        foreach (var obj in listeObjectsInThisState)
        {
            if (obj == null) continue;

            // Récupère les limites de l'objet pour le surlignage
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Dessiner un cadre autour de l'objet
                Handles.color = Color.red;
                Handles.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
            }
            else
            {
                // Si pas de renderer, dessine un disque autour de l'objet
                Handles.color = Color.red;
                Handles.DrawWireDisc(obj.transform.position, Vector3.up, 1.0f); // Rayon par défaut
            }
        }
    }
}
