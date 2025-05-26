using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float moveSpeed;
    public GameObject target;

    private void Start()
    {
        this.transform.position = target.transform.position;
    }

    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }
}
