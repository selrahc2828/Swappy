using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Magnet_Range : Comportment
{
    public GameObject magnetRange_Prefab;
    public float magnetRange_size;
    // Start is called before the first frame update
    void Start()
    {
        if (magnetRange_Prefab)
        {
            GameObject magnetism = Instantiate(magnetRange_Prefab, transform.position, Quaternion.identity, transform);
            magnetism.transform.localScale = transform.localScale * magnetRange_size;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}