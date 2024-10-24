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
    public float useTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (type == ListMovement.Move)
        {
            //StartCoroutine("AutoJump");
        }

        useTime = interval;
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ListMovement.Null:
                break;
            case ListMovement.Move:
                useTime -= Time.deltaTime;
                if (useTime < 0f)
                {
                    AutoJumpBancale();
                    useTime = interval;
                }
                break;
            case ListMovement.Rotate:
                RotateObj();
                break;
            case ListMovement.Action:
                break;
            default:
                break;
        }
    }
    void AutoJumpBancale()
    {
        rb.AddForce(transform.up * force, ForceMode.Impulse);
    }

    void RotateObj()
    {
        transform.Rotate(new Vector3(x, y, z) * angle * Time.deltaTime);
    }
}
