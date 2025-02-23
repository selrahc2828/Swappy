using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    
    float xRotation;
    float yRotation;

    public LineRenderer line;
    private Ray _ray;
    public LayerMask hitLayer;
    private Transform orientation;

    void Start()
    {
        orientation = GameManager.Instance.orientation.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensY;
        
        
        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

        LineRenderer();
    }


    private void LineRenderer()
    {

        float maxDistance = 500f;
        line.SetPosition(0, transform.position - transform.up / 5 + transform.right / 5);  // D�but de la ligne (cam�ra)

        RaycastHit _hit;

        _ray = this.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            // Si le raycast touche un objet
            line.SetPosition(1, _hit.point);  // Fin de la ligne (point touch� par le rayon)
        }
        else
        {
            line.SetPosition(1,transform.forward * maxDistance);
        }
    }
}
