using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;
    public Camera mainCamera;
    
    void Start()
    {
       rb = GetComponent<Rigidbody>();
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
        // Get the mouse position on the screen
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a ray from the main camera
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);

        // Cast the ray from the position of this object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraRay.direction, out hit, 100.0f))
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            // Do something with the hit object here, e.g., apply a force, change color, etc.
        }
        else
        {
            Debug.Log("No hit detected");
        }
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