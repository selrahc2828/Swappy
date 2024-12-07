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
    // isPickUp pour check si on a un objet ou non
    public Camera mainCam;
    public Transform handlerPosition;
    public Transform interractorZonePos;//centre zone de detection
    
    public Collider[] playerCollider; // on a 2 colliders
    public TextMeshProUGUI interactText;
    
    public bool isCarrying;

    public LayerMask hitLayer;    
    [SerializeField]
    private GameObject _carriedObject;
    private Transform _originParent;
    [SerializeField]
    private GameObject _closestObj;
    [Header("Variation")]
    public bool isLaunchable;
    public float launchForce;
    public Vector3 detectionSize;
    void Start()
    {
        if (interactText)
        {
            interactText.gameObject.SetActive(false);
        }
        
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        interractorZonePos = GameObject.FindGameObjectWithTag("InterractorZone").transform;
        handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        
        // set la position du "vraie" handler qui est dans la camera, à la position qu'on a set dans le player
        // on le met dans la camera (---OBSOLETE---)
        // ChangeParent(handlerPosition);
        // playerCollider = GetComponent<Collider>();
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
                Debug.LogWarning("item: "+ item.name);
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
            _carriedObject = _closestObj;
            _originParent = _carriedObject.transform.parent;

            // deplace obj
            ResetCarryPos();

            if (_carriedObject.GetComponent<Rigidbody>())
            {
                _carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                foreach (Collider collider in playerCollider)
                {
                    Physics.IgnoreCollision(collider, _carriedObject.GetComponent<Collider>(), true);
                }
            }

            isCarrying = true;
            _closestObj = null;
            //Debug.Log("Grab : " + carriedObject.name);

        }
    }

    public void Drop()
    {
        if (isCarrying)
        {
            _carriedObject.transform.SetParent(_originParent);
            if (_carriedObject.GetComponent<Rigidbody>()) {
                _carriedObject.GetComponent<Rigidbody>().isKinematic = false;
                foreach (Collider collider in playerCollider)
                {
                    Physics.IgnoreCollision(collider, _carriedObject.GetComponent<Collider>(), false);
                }
                if (isLaunchable)
                {
                    _carriedObject.GetComponent<Rigidbody>().AddForce(handlerPosition.forward * launchForce, ForceMode.Impulse);
                }
            }

            //reset
            _carriedObject = null;
            isCarrying = false;
        }
    }
    public void ResetCarryPos()
    {
        if (_carriedObject != null)
        {
            _carriedObject.transform.SetParent(handlerPosition);
            _carriedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            _carriedObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _carriedObject.transform.localPosition = Vector3.zero;
        }
    }

    public void ChangeParent(Transform pos)
    {
        handlerPosition = pos;
    }

    private void OnDrawGizmos()
    {
        if (mainCam == null) return;

        // affiche box d'interaction
        
        Gizmos.color = Color.blue;

        // M�me calcul pour le gizmo de la box de detection
        Vector3 boxCenter = interractorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);
        Quaternion boxRotation = mainCam.transform.rotation;

        // Pour dessiner la bo�te dans la sc�ne avec Gizmos (comme avec Physics.OverlapBox)
        // Matrix4x4.TRS permetd dessiner, position, rotation et echelle, juste DrawWireCube ne suffit pas 
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, detectionSize);
    }

}
