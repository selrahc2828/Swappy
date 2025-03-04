
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    public Camera mainCam;
    public Vector3 detectionSize = new Vector3(1f,1f,2.8f);
    public Transform pivotParent;
    public Transform interactorZonePos;//centre zone de detection (qu'on recalcule plus tard)
    
    [HideInInspector]public Transform interactor;
    [HideInInspector] public TextMeshProUGUI interactText;

    private Vector3 _boxCenter;
    private Quaternion _boxRotation;
    public LayerMask hitLayer;
    
    [SerializeField] private GameObject _closestObj;

    [SerializeField] private GrabObject _grabScript;
    
    [Header("Textes")]
    public string textGrab = "Press C to grab";
    public string textInteraction = "Press C to interact";
    
    private void Start()
    {
        mainCam = GameManager.Instance.mainCamera;
        interactor = GameObject.FindGameObjectWithTag("Interactor").transform;

        interactorZonePos = GameObject.FindGameObjectWithTag("InterractorZone").transform;
        // handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        
        
        interactText = GameObject.FindGameObjectWithTag("TextInteract").GetComponent<TextMeshProUGUI>(); 
        interactText?.gameObject.SetActive(false);
        
        _grabScript = GetComponent<GrabObject>();
    }

    private void Update()
    {
        CreateBox();
        Detection();
        CheckInteraction();

    }

    void CreateBox()
    {
        // Calcul nouvelle position du centre pour que le bord reste collé au joueur
        // On décale le centre de la moitié de la profondeur (axe Z) de la boite vers l'avant
        // mainCam pour tourner la box vers où on regarde
        _boxCenter = interactorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);

        _boxRotation = mainCam.transform.rotation;
        if (interactor != null)
        {
            // Appliquer rotation camera sur le pivot
            pivotParent.rotation = mainCam.transform.rotation;
            // change rotation zone d'interaction par rapport au pivot
            interactor.rotation = pivotParent.rotation;
        }
    }

    void Detection()
    {
        // on part du principe que seul les objets qu'on peut porter ou interagir auront les tag et on trie la liste
        Collider[] hitColliders = Physics.OverlapBox(_boxCenter, detectionSize / 2, _boxRotation, hitLayer)
            .Where(collider => collider.CompareTag("Movable") || collider.CompareTag("Interact"))
            .ToArray();
        
        if (hitColliders.Length > 0) // && !isCarrying
        {
            // on récupère l'objet le plus proche 

            float closestDist = Mathf.Infinity;//distance plus proche par défaut
            // voir ajouter compare tag NotInteract ?
            foreach (Collider item in hitColliders)
            {
                
                Vector3 playerToObject = item.transform.position - transform.position;//objet - player
                //Debug.DrawRay(transform.position, playerToObject, Color.red);

                // vérifie obstacle entre player et objet
                RaycastHit _obstacleHit;
               
                if (Physics.Raycast(transform.position, playerToObject, out _obstacleHit, playerToObject.magnitude))
                {
                    // Si l'objet touché n'est pas le même que l'objet que l'on test, c'est qu'il y a un obstacle
                    if (_obstacleHit.collider.gameObject != item.gameObject)
                    {
                        continue; // On passe à l'objet suivant
                    }
                }
                
                //set l'objet le plus proche
                if (playerToObject.magnitude < closestDist) // && item.CompareTag("Movable")
                {
                    closestDist = playerToObject.magnitude;
                    _closestObj = item.gameObject;
                }
            }
        }
        else
        { 
            _closestObj = null;
        }
    }

    void CheckInteraction()
    {
        switch (_closestObj?.tag)
        {
            case null:
                interactText?.gameObject.SetActive(false);
                _grabScript.objToCarry = null;
                break;
            
            case "Movable":
                if (!_grabScript.isCarrying)
                {
                    interactText?.gameObject.SetActive(true);
                    interactText.text = textGrab;

                    if (_closestObj.GetComponent<Rigidbody>())
                    {
                        //set obj que l'on peut grab
                        _grabScript.objToCarry = _closestObj;
                    }
                    else
                    {
                        //cache le texte    
                    }                    
                }
                
                break;
            
            case "Interact":
                interactText?.gameObject.SetActive(true);

                interactText.text = textInteraction;
                
                break;
            
            default:
                break;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (mainCam == null) return;

        // affiche box d'interaction
        
        Gizmos.color = Color.green;
        if (interactorZonePos != null && mainCam!= null)
        {
            // Meme calcul pour le gizmo de la box de detection
            Vector3 boxCenter = interactorZonePos.position + mainCam.transform.forward * (detectionSize.z / 2);
            Quaternion boxRotation = mainCam.transform.rotation;

            // Pour dessiner la boite dans la scene avec Gizmos (comme avec Physics.OverlapBox)
            // Matrix4x4.TRS permet de dessiner, position, rotation et echelle, juste DrawWireCube ne suffit pas 
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, detectionSize);            
        }

    }
}
