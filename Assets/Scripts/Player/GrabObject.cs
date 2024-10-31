using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabObject : MonoBehaviour
{
    // var position / parent ou mettre obj
    // taille max qu'on peut porter + offset moitié largeur du player
    // isPickUp pour check si on a un objet ou non
    public Camera mainCam;
    private Collider playerCollider;
    public TextMeshProUGUI interactText;
    public Transform handlerPosistion;
    public Transform handlerCamera;

    public Transform interractorZonePos;//centre zone de detection

    [HideInInspector]
    public bool isCarrying;

    public LayerMask hitLayer;    
    private GameObject carriedObject;
    private Transform originParent;

    private GameObject closestObj;
    [Header("Variation")]
    public bool isLaunchable;
    public float launchForce;
    public Vector3 detectionSize;
    void Start()
    {
        if (interactText)
            interactText.gameObject.SetActive(false);
        // set la position du "vraie" handler qui est dans la camera, à la position qu'on a set dans le player
        handlerCamera.position = handlerPosistion.position;
        //on le met dans la camera
        ChangeParent(handlerCamera);
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcul de la nouvelle position du centre pour que le bord reste collé au joueur
        // On décale le centre de la moitié de la profondeur (axe Z) de la boîte vers l'avant
        // mainCam pour tourner la box vers où on regarde
        Vector3 boxCenter = interractorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);

        Quaternion boxRotation = mainCam.transform.rotation;

        Collider[] hitColliders = Physics.OverlapBox(boxCenter, detectionSize / 2, boxRotation, hitLayer); // transform.rotation pour tourner avec player

        if (hitColliders.Length > 0 && !isCarrying)
        {
            float closestDist = Mathf.Infinity;
            // voir ajouter compare tag NotInteract ?
            foreach (Collider item in hitColliders)
            {
                float distanceToObject = Vector3.Distance(transform.position, item.transform.position);

                // on part du principe que seul les objets qu'on peut porter auront le tag
                if (distanceToObject < closestDist && item.CompareTag("Movable")) 
                {
                    closestDist = distanceToObject;
                    closestObj = item.gameObject;
                    if (interactText)
                    {
                        interactText.gameObject.SetActive(true);
                    }
                    //Debug.Log("closestObj : " + closestObj.name);
                }
            }
        }
        else
        {
            if (interactText)
                interactText.gameObject.SetActive(false);
            closestObj = null;
        }
    }

    public void Carrying()
    {
        if (closestObj != null && !isCarrying) 
        {
            carriedObject = closestObj;
            originParent = carriedObject.transform.parent;

            // deplace obj
            ResetCarryPos();

            if (carriedObject.GetComponent<Rigidbody>())
            {
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                Physics.IgnoreCollision(playerCollider, carriedObject.GetComponent<Collider>(), true);
            }

            isCarrying = true;
            closestObj = null;
            //Debug.Log("Grab : " + carriedObject.name);

        }
    }

    public void Drop()
    {
        if (isCarrying)
        {
            carriedObject.transform.SetParent(originParent);
            if (carriedObject.GetComponent<Rigidbody>()) {
                carriedObject.GetComponent<Rigidbody>().isKinematic = false;
                Physics.IgnoreCollision(playerCollider, carriedObject.GetComponent<Collider>(), false);

                if (isLaunchable)
                {
                    carriedObject.GetComponent<Rigidbody>().AddForce(handlerPosistion.forward * launchForce, ForceMode.Impulse);
                }
            }

            //reset
            carriedObject = null;
            isCarrying = false;

            //Debug.Log("Drop");
        }

    }
    public void ResetCarryPos()
    {
        //Debug.Log("Reset obj carry, interactorPos : " + handlerPos);
        if (carriedObject != null)
        {
            carriedObject.transform.SetParent(handlerPosistion);
            carriedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            carriedObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            carriedObject.transform.localPosition = Vector3.zero;
        }
    }

    public void ChangeParent(Transform pos)
    {
        handlerPosistion = pos;
    }

    private void OnDrawGizmos()
    {
        if (mainCam == null) return;

        Gizmos.color = Color.blue;

        // Même calcul pour le gizmo de la box de detection
        Vector3 boxCenter = interractorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);
        Quaternion boxRotation = mainCam.transform.rotation;

        // Pour dessiner la boîte dans la scène avec Gizmos (comme avec Physics.OverlapBox)
        // Matrix4x4.TRS permetd dessiner, position, rotation et echelle, juste DrawWireCube ne suffit pas 
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectionSize);
    }

}
