using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientatiaon;

    float xRotation;
    float yRotation;

    public LineRenderer line;

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
        // Si le raycast touche un objet
        line.SetPosition(0, transform.position - transform.up/2 + transform.right/2);  // Début de la ligne (caméra)
        line.SetPosition(1, transform.forward * maxDistance);  // Fin de la ligne (point touché par le rayon)
    }
}
