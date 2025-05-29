
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class SpawnPot : MonoBehaviour
{
    [Header("Info")]
    public PotData potData; 
    public bool isBroken;
    [Header("SpawnerSetUp")]
    public float size;
    public float rangeDetect;
    
    private GameObject pot;
    [SerializeField]
    private string uniqueID;
    public string UniqueID
    {
        get => uniqueID;
        set => uniqueID = value;
    }

    private void Start()
    {
        Spawn();
    }

    // private void OnEnable()
    // {
    //     GlobalEventManager.Instance.OnShattered -= HandleShattered;
    // }

    void Spawn()
    {
        if (potData is null)
        {
            Debug.Log("Aucun PotData assigne");
            return;
        }
        
        GameObject prefabToSpawn = isBroken ? potData.brokenPrefab : potData.normalPrefab;

        if (prefabToSpawn is null)
        {
            Debug.Log("Le prefab selectionne est nul");
            return;
        }
        
        Quaternion targetRotation = prefabToSpawn.transform.rotation;
        
        Vector3 position = transform.position;

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, rangeDetect))
        {
            Vector3 groundNormal = hit.normal;
            position = hit.point;
            
            // Calculer la rotation cible pour que le joueur "colle" à la planète
            Quaternion surfaceAlignRotation = Quaternion.FromToRotation(Vector3.up, groundNormal);
            targetRotation = surfaceAlignRotation * prefabToSpawn.transform.rotation;
        }
        
        pot = Instantiate(prefabToSpawn, position, targetRotation);

        // on va s'abonner à un event de l'objet qu'on instantie
        BreakableObject breakable = pot.GetComponent<BreakableObject>();
        if (breakable is not null)
        {
            breakable.Spawner = this; // pour savoir quel abonné est à modifier dans HandleShattered
            // GlobalEventManager.Instance.OnShattered += HandleShattered;
        }
    }
    
    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one * size);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * rangeDetect);
    }
}