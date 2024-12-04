using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientatiaon;

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

    // Update is called once per frame
    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientatiaon.rotation = Quaternion.Euler(0, yRotation, 0);

        LineRenderer();
    }


    private void LineRenderer()
    {

        float maxDistance = 500f;
        line.SetPosition(0, transform.position - transform.up / 5 + transform.right / 5);  // Début de la ligne (caméra)

        RaycastHit _hit;

        _ray = this.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            // Si le raycast touche un objet
            line.SetPosition(1, _hit.point);  // Fin de la ligne (point touché par le rayon)
        }
        else
        {
            line.SetPosition(1,transform.forward * maxDistance);
        }
    }
}
