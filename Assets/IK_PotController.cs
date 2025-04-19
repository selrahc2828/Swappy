using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IK_PotController : MonoBehaviour
{
    
    public Transform[] legsTargets;
    public Transform[] idealsPosition;
    public float areaOffset = 1.5f; // zone dans laquelle la jambe doit rester
                                    // 
    public float heightOffset = .5f; // hauteur du pas
    public float localPlacement = .5f; // ecrat entre corps et jambe

    
    void Start()
    {
        for (int i = 0; i < idealsPosition.Length; i++)
        {
            legsTargets[i].position = idealsPosition[i].position;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < idealsPosition.Length; i++)
        {
            float distance = Vector3.Distance(legsTargets[i].position, idealsPosition[i].position);
            
            // si jambe est trop loin de sa position ideal, on la déplace
            if (distance > areaOffset)
            {
                legsTargets[i].position = idealsPosition[i].position;
            }
        }
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
