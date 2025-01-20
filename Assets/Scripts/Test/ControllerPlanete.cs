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
    public Controls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = ouiScript.controls;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;
        Debug.Log(moveInputVector);
        //calculate movement direction
        moveDirection = transform.forward * moveInputVector.y + transform.right * moveInputVector.x;
        
        rb.AddForce(moveDirection * 10f, ForceMode.Force);
    }
}
