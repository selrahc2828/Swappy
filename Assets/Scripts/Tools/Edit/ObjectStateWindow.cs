using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectStateWindow : EditorWindow
{
    private string objectName = "Aucun objet sélectionné";
    private ComportementsStateMachine stateMachineScript;

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
            GUILayout.Label(stateMachineScript?.currentState?.ToString(), EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Current State", GUILayout.Width(120)))
            {
                
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset To Initial State", GUILayout.Width(120)))
            {
                
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Change State", EditorStyles.boldLabel);
            if (GUILayout.Button("Changer", GUILayout.Width(120)))
            {
                
            }
            EditorGUILayout.EndHorizontal();
        }
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
            // Tente de récupérer l'objet sous le clic de la souris
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                objectName = hit.collider.gameObject.name;
                stateMachineScript = hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
                Repaint(); // Met à jour la fenêtre pour afficher le nouveau nom
            }
            else
            {
                objectName = "Aucun objet détecté";
                Repaint();
            }

            //e.Use(); // Empêche d'autres manipulations de l'événement
        }
    }
}
