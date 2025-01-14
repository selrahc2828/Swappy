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
    [MenuItem("Tools/State In Scene Window")]
    public static void ShowWindow()
    {
        GetWindow<StateInSceneWindow>("State In Scene Window");
        
    }

    private void OnEnable()
    {
        UpdateStateList();
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

            // Afficher le bouton sous la liste déroulante
            if (GUILayout.Button("Rechercher", GUILayout.Height(30)))
            {
                SearchEveryObjectWithThisState(stateValues[selectedStateIndex], stateNames[selectedStateIndex]);
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
    private void SearchEveryObjectWithThisState(int stateValue, string stateName)
    {
        // Logique à exécuter lorsque le bouton est cliqué
        Debug.Log($"Action pour l'état : {stateName} ({stateValue})");
        listeObjectsInThisState = FindObjectsOfType<ComportementsStateMachine>();
        // Ajouter ici une action spécifique pour l'état
    }
}
