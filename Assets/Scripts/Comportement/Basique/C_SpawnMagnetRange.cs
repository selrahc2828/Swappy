using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SpawnMagnetRange : Comportment
{
    public GameObject magnetRange_Prefab;
    public float magnetRange_size;

    void Start()
    {
        if (magnetRange_Prefab)
        {
            GameObject magnetism = Instantiate(magnetRange_Prefab, transform.position, Quaternion.identity, transform);
            magnetism.transform.localScale = transform.localScale * magnetRange_size;
        }
    }
}
