using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAimGround : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    Transform position;
    
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 100, layerMask))
        {
            position.transform.position = hit.point;
        }
        
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.red);
    }
}
