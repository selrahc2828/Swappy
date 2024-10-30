using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TemporaryController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientatiaon;

    public float xRotation;
    public float yRotation;
    
    void Start()
    {
       //rb = GetComponent<Rigidbody>();
       Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        LookAround();
        MovePlayer();
        Jump();
    }

    void LookAround()
    {

    }

    void MovePlayer()
    {
        
    }

    void Jump()
    {
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}