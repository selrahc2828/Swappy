using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GrabObject : MonoBehaviour
{
    // var position / parent ou mettre obj
    // taille max qu'on peut porter + offset moiti� largeur du player
    public Camera mainCam;
    public Transform handlerPosition;
    public Transform interractorZonePos;//centre zone de detection
    // private Collider playerCollider;
    [HideInInspector] public Collider[] playerCollider; // on a 2 colliders
    public TextMeshProUGUI interactText;
    
    [HideInInspector] public bool isCarrying;

    public LayerMask hitLayer;    
    [HideInInspector]
    public GameObject carriedObject;
    private Transform _originParent;
    private GameObject _closestObj;
    [Header("Variation")]
    public bool isLaunchable;
    public float launchForce;
    public Vector3 detectionSize;
    void Start()
    {
        interactText = GameObject.FindGameObjectWithTag("TextInteract").GetComponent<TextMeshProUGUI>();
        if (interactText)
        {
            interactText.gameObject.SetActive(false);
        }
        
        mainCam = GameManager.Instance.mainCamera;
        interractorZonePos = GameObject.FindGameObjectWithTag("InterractorZone").transform;
        handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        playerCollider = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcul de la nouvelle position du centre pour que le bord reste collé au joueur
        // On décale le centre de la moitié de la profondeur (axe Z) de la boite vers l'avant
        // mainCam pour tourner la box vers où on regarde
        Vector3 boxCenter = interractorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);

        Quaternion boxRotation = mainCam.transform.rotation;

        // on part du principe que seul les objets qu'on peut porter auront le tag
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, detectionSize / 2, boxRotation, hitLayer)
            .Where(collider => collider.CompareTag("Movable"))
            .ToArray();
        //on recupère un tableau trié en amont avec juste les objets qu'on peut bouger
        if (hitColliders.Length > 0 && !isCarrying)
        {
            float closestDist = Mathf.Infinity;
            // voir ajouter compare tag NotInteract ?
            foreach (Collider item in hitColliders)
            {
                float distanceToObject = Vector3.Distance(transform.position, item.transform.position);

                if (distanceToObject < closestDist) // && item.CompareTag("Movable")
                {
                    closestDist = distanceToObject;
                    _closestObj = item.gameObject;
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
            {
                interactText.gameObject.SetActive(false);
            }
            _closestObj = null;
        }
    }

    public void Carrying()
    {
        if (_closestObj != null && !isCarrying) 
        {
            carriedObject = _closestObj;
            _originParent = carriedObject.transform.parent;

            // deplace obj
            ResetCarryPos();

            if (carriedObject.GetComponent<Rigidbody>())
            {
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                foreach (Collider collider in playerCollider)
                {
                    Physics.IgnoreCollision(collider, carriedObject.GetComponent<Collider>(), true);
                }
                // Physics.IgnoreCollision(playerCollider, carriedObject.GetComponent<Collider>(), true);
                //dire a l'objet qu'il est grab au niveau FSM
                var FSM_OfObject = carriedObject.GetComponent<ComportementsStateMachine>();
                ComportementState FSM_ObjectState = (ComportementState)FSM_OfObject.currentState;
                FSM_ObjectState.isGrabbed = true;
            }

            isCarrying = true;
            _closestObj = null;
            //Debug.Log("Grab : " + carriedObject.name);

        }
    }

    public void Drop(bool dropRepulse = false)//à voir pour modif dans FSM repulse
    {
        if (isCarrying)
        {
            carriedObject.transform.SetParent(_originParent);
            if (carriedObject.GetComponent<Rigidbody>()) {

                //dire a l'objet qu'il est grab au niveau FSM
                var FSM_OfObject = carriedObject.GetComponent<ComportementsStateMachine>();
                ComportementState FSM_ObjectState = (ComportementState)FSM_OfObject.currentState;
                FSM_ObjectState.isGrabbed = false;

                carriedObject.GetComponent<Rigidbody>().isKinematic = false;
                // Physics.IgnoreCollision(playerCollider, carriedObject.GetComponent<Collider>(), false);

                foreach (Collider collider in playerCollider)
                {
                    Physics.IgnoreCollision(collider, carriedObject.GetComponent<Collider>(), false);
                }
                if (isLaunchable && !dropRepulse)
                {
                    carriedObject.GetComponent<Rigidbody>().AddForce(handlerPosition.forward * launchForce, ForceMode.Impulse);
                }
            }

            //reset
            carriedObject = null;
            isCarrying = false;
        }
    }
    public void ResetCarryPos()
    {
        if (carriedObject != null)
        {
            carriedObject.transform.SetParent(handlerPosition);
            carriedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            carriedObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            carriedObject.transform.localPosition = Vector3.zero;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (mainCam == null) return;

        // affiche box d'interaction
        
        Gizmos.color = Color.blue;
        if (interractorZonePos != null && mainCam!= null)
        {
            // Meme calcul pour le gizmo de la box de detection
            Vector3 boxCenter = interractorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);
            Quaternion boxRotation = mainCam.transform.rotation;

            // Pour dessiner la boite dans la scene avec Gizmos (comme avec Physics.OverlapBox)
            // Matrix4x4.TRS permet de dessiner, position, rotation et echelle, juste DrawWireCube ne suffit pas 
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, detectionSize);            
        }

    }

}
