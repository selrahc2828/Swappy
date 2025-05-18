#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class UniqueIDEditor 
{
    static Dictionary<string, int> tupleID = new();
    
    //ici on veut générer un ID unique sur les spawner de pot cassable, et faire en sorte que cette ID soit différent quand on duplique l'objet
    static UniqueIDEditor()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        EditorSceneManager.sceneOpened += OnOpenedScene;
        
        // Uniquement pour la scene active quand on demare unity
        if (EditorSceneManager.GetActiveScene().isLoaded)
        {
            InitIDTracking();
        }
    }

    private static void OnOpenedScene(Scene scene, OpenSceneMode mode)
    {
        //initialise la liste des ID Spawner
        InitIDTracking();
    }

    private static void InitIDTracking()
    {
        tupleID.Clear();
        
        // on stock les unique ID des spawner dans la scene et leur ID d'instance pour pouvoir ensuite les comparer quand la hierarchie change
        // comme ça on evite de changer le spawner d'origine
        
        SpawnPot[] all = UnityEngine.Object.FindObjectsOfType<SpawnPot>();
        
        foreach (SpawnPot spawner in all)
        {
            if (string.IsNullOrEmpty(spawner.UniqueID))
            {
                SetUniqueID(spawner);
            }

            if (!tupleID.ContainsKey(spawner.UniqueID))
            {
                tupleID[spawner.UniqueID] = spawner.GetInstanceID();
            }
        }
    }

    static void OnHierarchyChanged()
    {
        
        // Récupérer tous les objets SpawnPot dans la scene active
        SpawnPot[] all = UnityEngine.Object.FindObjectsOfType<SpawnPot>();
        
        foreach (SpawnPot spawner in all)
        {
            if (string.IsNullOrEmpty(spawner.UniqueID))
            {
                SetUniqueID(spawner);
                tupleID[spawner.UniqueID] = spawner.GetInstanceID();
                continue;
            }
            
            //on test si on a déjà une instance pour le uniqueID
            if (!tupleID.TryGetValue(spawner.UniqueID, out var knownInstanceID))
            {
                // premiere fois qu'on voit cet ID → on l’enregistre
                tupleID[spawner.UniqueID] = spawner.GetInstanceID();
            }
            else if (knownInstanceID != spawner.GetInstanceID())
            {
                // si on a déjà une instance ET qu'elle est differente de la valeur stocke, alors c'est une duplication
                SetUniqueID(spawner);
                tupleID[spawner.UniqueID] = spawner.GetInstanceID();
            }
        }
    }

    static void SetUniqueID(SpawnPot spawner)
    {
        // UniqueID n'a pas de setter
        spawner.GetType()
            .GetField("uniqueID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(spawner, Guid.NewGuid().ToString());
        // System.Reflection.BindingFlags.NonPublic => cherche les var private et protected
        // BindingFlags.Instance => on veut un chap d'instance, qui est a un objet et non a une classe static
        
        EditorUtility.SetDirty(spawner);
    }
}
#endif
