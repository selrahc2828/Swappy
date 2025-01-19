using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class ControllerPlanete : MonoBehaviour
{
    public Rigidbody rb;
    public Vector2 moveInputVector;
    public Vector3 moveDirection;
    protected Controls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
