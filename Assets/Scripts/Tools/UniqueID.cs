using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[DisallowMultipleComponent] // empeche le componenet d'etre ajoute plusieurs fois a l'objet
public class UniqueID : MonoBehaviour
{
    [FormerlySerializedAs("uniqueID")] [SerializeField]
    private string uniqueId;
    public string UniqueId => uniqueId;

    private static HashSet<string> _existingIds;
    private static Dictionary<string, UniqueID> allIDs = new Dictionary<string, UniqueID>();
    // cache / reference des IDs, id et nombre de fois qu'elle apparait
    // clé => ID, value => l'objet qui le possède

    private void OnValidate()
    {
        RefreshExistingIDs();

        // Si l'ID est vide ou deja assigne, on en fait un nouveau
        if (string.IsNullOrEmpty(uniqueId) || IsDuplicate(uniqueId))
        {
            // uniqueId = GenerateUniqueID();
            Debug.Log($"{gameObject.name}: GenerateNewUniqueID");
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private static void  RefreshExistingIDs()
    {
        // Réinitialiser la liste des IDs
        allIDs.Clear();
        
        foreach (UniqueID objID in FindObjectsOfType<UniqueID>(true))
        {
            if (string.IsNullOrEmpty(objID.uniqueId)) continue;

            if (!allIDs.ContainsKey(objID.uniqueId))
                allIDs.Add(objID.uniqueId, objID);
                // ID : Obj
        }
    }
    
    public bool IsDuplicate(string id)
    {
        if (!allIDs.ContainsKey(id)) return false; // si l'id n'est pas dans la liste, pas de double

        return allIDs[id] != this; // retourne true si l'id est sur l'objet qui test, sinon l'id est sur un autre et c'est donc un doubon
    }
}
