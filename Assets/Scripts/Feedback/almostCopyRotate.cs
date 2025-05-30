using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class almostCopyRotate : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        float targetZ = target.transform.eulerAngles.z;

        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, targetZ);
    }
}
