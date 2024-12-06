using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SpawnMagnetRange : Comportment
{
    public GameObject magnetRange_Prefab;
    public float magnetRange_size;

    private GameObject magnetZone;

    void Start()
    {
        if (magnetRange_Prefab)
        {
            magnetZone = Instantiate(magnetRange_Prefab, transform.position, Quaternion.identity, transform);
            magnetZone.transform.localScale = transform.localScale * magnetRange_size;
        }
    }

    private void OnDisable()
    {
        Destroy(magnetZone);
    }
}
