using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Spawner : Comportment
{
    [SerializeField] private GameObject objectSpawned;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnTime;
    
    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnTime)
        {
            GameObject instantiatedObject = Instantiate(objectSpawned, transform.position, Quaternion.identity);
            spawnTimer = 0f;
        }
    }

    private void CheckNewComponents()
    {
        
    }
}
