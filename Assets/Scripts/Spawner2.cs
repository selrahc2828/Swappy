using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public GameObject objectSpawned;
    public Transform spawnPoint;
    public float speed;
    private float spawnTimer;
    public float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnTime)
        {
            GameObject bombSpawned = Instantiate(objectSpawned);
            bombSpawned.transform.position = spawnPoint.position;
            bombSpawned.GetComponent<Rigidbody>().AddForce(spawnPoint.up * speed, ForceMode.Impulse);
            spawnTimer = 0f;
        }
    }
}