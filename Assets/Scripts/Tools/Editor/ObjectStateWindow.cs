using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ObjectStateWindow : EditorWindow
{
    private string objectName = "Aucun objet sélectionné";
    private ComportementsStateMachine stateMachineScript; 
    private int selectedStateIndex = 0;
    private string[] stateNames = Array.Empty<string>(); // Noms des états disponibles
    private int[] stateValues = Array.Empty<int>(); // Valeurs des états


    [MenuItem("Tools/Object State Window")]
    public static void ShowWindow()
    {
        GetWindow<ObjectStateWindow>("Object Picker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Nom de l'objet sélectionné :", EditorStyles.boldLabel);
        GUILayout.Label(objectName, EditorStyles.label);
        
        if (Application.isPlaying)
        {
            GUILayout.Label(stateMachineScript?.currentState?.ToString() ?? "Aucun état actuel", EditorStyles.helpBox);

            if (stateMachineScript != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset Current State"))
                {
                    if (stateMachineScript != null && stateMachineScript.currentState is ComportementState comportementState)
                    {
                        comportementState.CalculateNewtState(comportementState.stateValue);
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset To Initial State"))
                {
                    if (stateMachineScript != null && stateMachineScript.currentState is ComportementState comportementState)
                    {
                        comportementState.CalculateNewtState((int)stateMachineScript.initialState);
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                // Menu déroulant pour selectionner un etat
                EditorGUILayout.Space();
                GUILayout.Label("Change state", EditorStyles.boldLabel);
                
                if (stateNames.Length > 0)
                {
                    selectedStateIndex = EditorGUILayout.Popup("State", selectedStateIndex, stateNames.ToArray());
                    if (GUILayout.Button("Change"))
                    {
                        ChangeState(stateValues[selectedStateIndex]);
                    }
                }
                else
                {
                    GUILayout.Label("Aucun état disponible.", EditorStyles.helpBox);
                }
            }
            else
            {
                GUILayout.Label("Aucun script de machine d'état trouvé sur l'objet sélectionné.", EditorStyles.helpBox);
            }
        }
    }
    private void ChangeState(int stateValue)
    {
        if (stateMachineScript != null && stateMachineScript.currentState is ComportementState comportementState)
        {
            // Vérifie si l'état actuel est différent du nouvel état
            if (comportementState.stateValue == stateValue)
            {
                Debug.Log($"L'état sélectionné ({stateNames[selectedStateIndex]} - {stateValue}) est déjà l'état actuel.");
                return; // Sort de la fonction sans effectuer de changement
            }
            comportementState.CalculateNewtState(stateValue);
            Debug.Log($"État changé en : {stateNames[selectedStateIndex]} ({stateValue})");
        }
        else
        {
            Debug.LogWarning("Impossible de changer d'état : ComportementState introuvable ou non valide.");
        }
    }
    
    private void UpdateStateList()
    {
        if (stateMachineScript != null)
        {
            // Récupérer les noms et valeurs de l'énumération FirstState
            var enumValues = Enum.GetValues(typeof(FirstState)).Cast<FirstState>().ToArray();
            //stateNames = enumValues.Select(v => v.ToString()).ToArray();
            stateNames = enumValues.Select(v => AddSpacesBeforeUpperCase(v.ToString())).ToArray();
            stateValues = enumValues.Select(v => (int)v).ToArray();
            
            // Synchroniser l'index avec l'état actuel
            if (stateMachineScript.currentState is ComportementState comportementState)
            {
                var currentStateValue = comportementState.stateValue;
                selectedStateIndex = Array.IndexOf(stateValues, currentStateValue);
            }
            
            if (selectedStateIndex < 0)
            {
                selectedStateIndex = 0; // Si l'état initial n'est pas trouvé, sélectionner le premier
            }
        }
        else
        {
            stateNames = Array.Empty<string>();
            stateValues = Array.Empty<int>();
            selectedStateIndex = 0;
        }
    }
    private string AddSpacesBeforeUpperCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        return System.Text.RegularExpressions.Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0) // Clic gauche
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                objectName = hitObject.name;
                stateMachineScript = hitObject.GetComponent<ComportementsStateMachine>();
                UpdateStateList(); // Met à jour la liste des états
                Repaint();
            }
            else
            {
                objectName = "Aucun objet détecté";
                stateMachineScript = null;
                UpdateStateList();
                Repaint();
            }
        }
    }
}
