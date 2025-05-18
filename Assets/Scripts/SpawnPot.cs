
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPot : MonoBehaviour
{
    public PotData potData; 
    public bool isBroken;

    public float size;
    private GameObject pot;
    public float rangeDetect;
    [SerializeField]
    private string uniqueID;
    public string UniqueID => uniqueID;
    
    private void Start()
    {
        Spawn();
    }

    private void OnEnable()
    {
        GlobalEventManager.Instance.OnShattered -= HandleShattered;
    }

    void Spawn()
    {
        if (potData is null)
        {
            Debug.Log("Aucun PotData assigné");
            return;
        }
        
        GameObject prefabToSpawn = isBroken ? potData.brokenPrefab : potData.normalPrefab;

        if (prefabToSpawn is null)
        {
            Debug.Log("Le prefab sélectionné est nul");
            return;
        }
        
        Quaternion targetRotation = prefabToSpawn.transform.rotation;
        
        Vector3 position = transform.position;

        //raycast vers centre monde, hitposition => position
        // RaycastHit raycastHit;
        // Physics.Raycast(transform.position, planete, out raycastHit);
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, rangeDetect))
        {
            Vector3 groundNormal = hit.normal;
            position = hit.point;
            
            // Calculer la rotation cible pour que le joueur "colle" à la planète
            Quaternion surfaceAlignRotation = Quaternion.FromToRotation(Vector3.up, groundNormal);
            targetRotation = surfaceAlignRotation * prefabToSpawn.transform.rotation;
            // targetRotation = Quaternion.FromToRotation(Vector3.up, groundNormal);
            
            // // Appliquer un offset si nécessaire
            // targetRotation *= Quaternion.Euler(offsetRotation);
        }
        
        pot = Instantiate(prefabToSpawn, position, targetRotation);

        // on va s'abonner à un event de l'objet qu'on instantie
        BreakableObject breakable = pot.GetComponent<BreakableObject>();
        if (breakable is not null)
        {
            breakable.Spawner = this; // pour savoir quel abonné est à modifier dans HandleShattered
            GlobalEventManager.Instance.OnShattered += HandleShattered;
        }
    }
    
    void HandleShattered(SpawnPot origin)
    {
        if (origin == this)
        {
            isBroken = true;
        }
    }
    
    private void OnDrawGizmos() {

        Gizmos.DrawWireCube(transform.position, Vector3.one * size);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * rangeDetect);
    }
}