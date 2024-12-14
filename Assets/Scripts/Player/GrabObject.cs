using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GrabObject : MonoBehaviour
{
    public Camera mainCam;
    public Transform handlerPosition;
    public Transform interractorZonePos;//centre zone de detection (qu'on recalcule plus tard)
    // private Collider playerCollider;
    [HideInInspector] public Collider[] playerCollider; // on a 2 colliders
    [HideInInspector] public TextMeshProUGUI interactText;
    [HideInInspector] public bool isCarrying;
    [HideInInspector] public GameObject carriedObject;
    public LayerMask hitLayer;
    private Transform _originParent;//pour replacer l'objet une fois lâché
    private GameObject _closestObj;
    [Header("Variation")]
    public bool canThrow;
    public float launchForce;
    public Vector3 detectionSize;

    public float grabForce;
    public float toleranceRange;// différence accepté entre position obj grab et où il doit être en main (lache sinon)
    void Start()
    {
        interactText = GameObject.FindGameObjectWithTag("TextInteract").GetComponent<TextMeshProUGUI>(); 
        interactText?.gameObject.SetActive(false);
        
        mainCam = GameManager.Instance.mainCamera;
        interractorZonePos = GameObject.FindGameObjectWithTag("InterractorZone").transform;
        handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        playerCollider = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcul nouvelle position du centre pour que le bord reste collé au joueur
        // On décale le centre de la moitié de la profondeur (axe Z) de la boite vers l'avant
        // mainCam pour tourner la box vers où on regarde
        Vector3 boxCenter = interractorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);

        Quaternion boxRotation = mainCam.transform.rotation;

        // on part du principe que seul les objets qu'on peut porter auront le tag et on trie la liste
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, detectionSize / 2, boxRotation, hitLayer)
            .Where(collider => collider.CompareTag("Movable"))
            .ToArray();
        
        if (hitColliders.Length > 0 && !isCarrying)
        {
            float closestDist = Mathf.Infinity;//distance plus proche par défaut
            // voir ajouter compare tag NotInteract ?
            foreach (Collider item in hitColliders)
            {
                Vector3 playerToObject = item.transform.position - transform.position;//objet - player
                Debug.DrawRay(transform.position, playerToObject, Color.red);

                // vérifie obstacle entre player et objet
                RaycastHit _areObstacle;
                Physics.Raycast(transform.position, playerToObject, out _areObstacle);//range = objectToPlayer.magnitude
                // verif tag touché joueur vers objet, si pas un Movable, un obstacle est devant
                if (!_areObstacle.collider.CompareTag("Movable"))
                {
                    return;//ou continue ? 
                }

                if (playerToObject.magnitude < closestDist) // && item.CompareTag("Movable")
                {
                    closestDist = playerToObject.magnitude;
                    _closestObj = item.gameObject;
                    if (interactText && _closestObj.GetComponent<Rigidbody>())
                    {
                        interactText.gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        { 
            interactText?.gameObject.SetActive(false);
            _closestObj = null;
        }

        // suivi de l'objet dans les bras et lache si bloque trops
        if (carriedObject != null)
        {
            //pose des pb: quand je grab il arrive que je sois trop loin et je drop directement
            if (Vector3.Distance(carriedObject.transform.position,handlerPosition.position) > toleranceRange)
            {
                Drop(false, true);//pas faire le addforce du drop
            }
            else
            {
                MoveObject();
            }
        }
    }

    public void MoveObject()
    {
        Vector3 dir = handlerPosition.position - carriedObject.transform.position;
        carriedObject.GetComponent<Rigidbody>().AddForce(dir * grabForce);
    }
    
    public void Carrying()
    {
        if (_closestObj != null && !isCarrying && _closestObj.GetComponent<Rigidbody>()) 
        {
            carriedObject = _closestObj;
            _originParent = carriedObject.transform.parent;
            // Debug.LogWarning($" dist grab {Vector3.Distance(carriedObject.transform.position,handlerPosition.position)}");

            //dire a l'objet qu'il est grab au niveau FSM
            var FSM_OfObject = carriedObject.GetComponent<ComportementsStateMachine>();
            ComportementState FSM_ObjectState = (ComportementState)FSM_OfObject.currentState;
            FSM_ObjectState.isGrabbed = true;
            
            //desactive la kinematic pour avoir les collisions
            if (FSM_ObjectState.isKinematic)
            {
                carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            
            // deplace obj
            SetCarringPosition(carriedObject, true);
            
            // ignore collision entre player et obj porté
            foreach (Collider collider in playerCollider)
            {
                Physics.IgnoreCollision(collider, carriedObject.GetComponent<Collider>(), true);
            }
            
            isCarrying = true;
            _closestObj = null;//on ne detecte plus les obj proche car on en porte déjà un
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Drop(bool dropRepulse = false, bool stuckInHand = false)//à voir pour modif dans FSM repulse
    {
        if (isCarrying)
        {
            carriedObject.transform.SetParent(_originParent);
            if (carriedObject.GetComponent<Rigidbody>()) {
                
                //dire a l'objet qu'il est grab au niveau FSM
                var FSM_OfObject = carriedObject.GetComponent<ComportementsStateMachine>();
                ComportementState FSM_ObjectState = (ComportementState)FSM_OfObject.currentState;
                FSM_ObjectState.isGrabbed = false;
                
                // réactive le kinematic des immuables
                if (FSM_ObjectState.isKinematic)
                {
                    carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
                
                SetCarringPosition(carriedObject, false);

                
                foreach (Collider collider in playerCollider)
                {
                    Physics.IgnoreCollision(collider, carriedObject.GetComponent<Collider>(), false);
                }
                //lance que si on lancé actif ET rien en main ou si impulse quand porté                
                if ((canThrow && !stuckInHand) || dropRepulse)
                {
                    if (!FSM_ObjectState.isGrabbed && !FSM_ObjectState.isKinematic)
                    {
                        FSM_ObjectState.GetThrown(handlerPosition.forward * launchForce);
                        //carriedObject.GetComponent<Rigidbody>().AddForce(handlerPosition.forward * launchForce, ForceMode.Impulse);
                    }
                }
            }
            //reset
            carriedObject = null;
            isCarrying = false;
        }
    }
    
    public void SetCarringPosition(GameObject objetGrab, Boolean inHand)
    {
        Rigidbody rb = objetGrab.GetComponent<Rigidbody>();
        if (inHand)
        {
            rb.useGravity = false;
            rb.drag = 10f;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            //marche moyen
            objetGrab.transform.rotation = Quaternion.Euler(0, 0, 0);
            objetGrab.transform.localRotation = Quaternion.Euler(0, 0, 0);
            
            objetGrab.transform.SetParent(handlerPosition);
        }
        else
        {
            rb.useGravity = true;
            rb.drag = 0f;
            rb.constraints = RigidbodyConstraints.None;
            
            objetGrab.transform.SetParent(_originParent);

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
