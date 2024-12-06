using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnPoint;
    public float spawnForce;
    private float spawnTimer;
    public float spawnTime;

    void Start()
    {
        spawnTimer = 0f;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnTime)
        {
            GameObject instantiatedObject = Instantiate(this.objectToSpawn);
            instantiatedObject.transform.position = spawnPoint.position;
            instantiatedObject.GetComponent<Rigidbody>().AddForce(spawnPoint.up * spawnForce, ForceMode.Impulse);
            spawnTimer = 0f;
        }
    }
}