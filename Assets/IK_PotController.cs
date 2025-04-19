using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IK_PotController : MonoBehaviour
{
    
    public Transform[] legsTargets;
    public Transform[] idealsPosition;
    [FormerlySerializedAs("sizeOffset")] public float areaOffset = 1.5f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < idealsPosition.Length; i++)
        {
            legsTargets[i].position = idealsPosition[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Color color = Color.green;
        foreach (Transform pos in legsTargets)
        {
            Gizmos.DrawWireSphere(pos.position, areaOffset);
        }
    }
}
