#if UNITY_EDITOR

using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public static class UniqueIDManagerEditor
{
    private static Dictionary<string, UniqueID> idMap = new Dictionary<string, UniqueID>();
    // cache / reference des IDs
    // ID:l'objet qui la possede
    public static IReadOnlyDictionary<string, UniqueID> GetIDMap() => idMap;

    static UniqueIDManagerEditor()
    {
        EditorSceneManager.sceneOpened += OnOpenedScene;
    }

    private static void OnOpenedScene(Scene scene, OpenSceneMode mode)
    {
        ClearAll();
        FindAllIDs();
    }

    public static void ClearAll()
    {
        idMap.Clear();
    }

    public static void FindAllIDs()
    {
        foreach (var idObj in Object.FindObjectsOfType<UniqueID>())
        {
            idObj.CheckUniqueID();
        }
    }

    public static bool RegisterID(string id, UniqueID component)
    {
        if (string.IsNullOrEmpty(id)) return false; // si champ vide on genere ID

        if (idMap.TryGetValue(id, out var existing))
        {
            if (existing != component)
            {
                return false; // Si l'id est deja dans le tableau, on genere un nouvel id
            }
            return true; // on ne fait rien
        }
        idMap.Add(id, component); // n'existe pas donc on l'ajoute
        return true;
    }
    
    public static string GenerateNewID(UniqueID id)
    {
        string uniqueID;
        const int maxAttempts = 100;
        int attempts = 0;

        do
        {
            uniqueID = Guid.NewGuid().ToString();
            attempts++;
        } while (!RegisterID(uniqueID, id) &&
                 attempts < maxAttempts); // si id genere est deja dans la liste, on le refait

        if (attempts >= maxAttempts)
        {
            Debug.LogError($"[UniqueID] Impossible de générer un ID unique après {maxAttempts} tentatives sur l'objet {id.name}.");
        }
        return uniqueID;
    }
}
#endif