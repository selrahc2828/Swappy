using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ListMovement
{
    Null,
    Move,
    Rotate,
    Action
}

public class Movement : Comportment
{
    public ListMovement type;
    public float x;
    public float y;
    public float z;
    public float angle;
    public float force;
    public float interval;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (type == ListMovement.Move)
        {
            StartCoroutine("AutoJump");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == ListMovement.Rotate)
        {
            RotateObj();
        }
    }

    IEnumerator AutoJump()
    {
        while (true)
        {
            rb.AddForce(transform.up * force, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
        }
    }

    void RotateObj()
    {
        transform.Rotate(new Vector3(x, y, z) * angle * Time.deltaTime);
    }
}
