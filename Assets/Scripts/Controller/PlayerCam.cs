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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensX; //deltaTime
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensY;
        //unscaledDeltaTime = temps réel, non affecté par le timescale
        
        // _moveMouseVector = GameManager.controls.Player.Look.ReadValue<Vector2>();
        // possible vector look de input system mais pas du tout la meme sensi
        
        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam, orientation and Avatar fbx
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        GameManager.Instance.orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        //GameManager.Instance.playerFBX.transform.rotation = Quaternion.Euler(0, yRotation, 0);

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
