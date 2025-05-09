using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateZ : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
