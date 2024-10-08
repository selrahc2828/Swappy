using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnRate;
    public Transform spwanPoint;

    public GameObject prefab;

    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Spawn()
    {

        while (true)
        {
            yield return new WaitForSeconds(SpawnRate);

            if (prefab)
            {
                Instantiate(prefab, spwanPoint.position, Quaternion.identity);
            }
        }

    }
}
