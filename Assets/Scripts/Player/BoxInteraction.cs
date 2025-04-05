
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxInteraction : MonoBehaviour
{
    public Camera mainCam;
    public Vector3 detectionSize = new Vector3(1f,1f,2.8f);
    public Transform pivotParent;
    public Transform interactorZonePos;//centre zone de detection (qu'on recalcule plus tard)
    
    [HideInInspector] public Transform interactor;
    [HideInInspector] public TextMeshProUGUI interactText;

    private Vector3 _boxCenter;
    private Quaternion _boxRotation;
    public LayerMask hitLayer;
    
    [SerializeField] private GameObject _closestObj;
    public GameObject closestObj
    {
        get { return closestObj; }
        set { closestObj = value; }
    }

    [SerializeField] private GrabObject _grabScript;
    
    [Header("Texte")]
    public InputActionReference actionReference; // Référence à l'action d'interaction
    public string textInteraction = "Press";
    
    private void Start()
    {
        mainCam = GameManager.Instance.mainCamera;
        interactor = GameObject.FindGameObjectWithTag("Interactor").transform;

        interactorZonePos = GameObject.FindGameObjectWithTag("InterractorZone").transform;
        // handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        
        interactText = GameObject.FindGameObjectWithTag("TextInteract").GetComponent<TextMeshProUGUI>(); 
        interactText?.gameObject.SetActive(false);

        _grabScript = GameManager.Instance.grabScript;
    }

    private void Update()
    {
        CreateBox();
        NearObject();
        // SetTextInteraction();
    }


    private void OnEnable()
    {
        GameManager.controls.Player.Interaction.performed += InteractAction;
    }

    public void OnDisable()
    {
        GameManager.controls.Player.Interaction.performed -= InteractAction;
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

    void NearObject() // recupère l'object avec lequel on peut interagir le plus proche
    {
        // on récupère les objets qui ont un script d'inreaction
        
        Collider[] hitColliders = Physics.OverlapBox(_boxCenter, detectionSize / 2, _boxRotation, hitLayer)
            .Where(collider => collider.GetComponent<InteractionSystem>() is not null)
            .ToArray();
        
            // faire par layer ? 

        if (_grabScript.isCarrying)// on ne peut pas interagir si on porte un objet
        {
            _closestObj = null;
            SetTextInteraction();
            return;
        }

        if (hitColliders.Length > 0) // && !isCarrying
        {
            // on récupère l'objet le plus proche 

            float closestDist = Mathf.Infinity;

            foreach (Collider item in hitColliders)
            {
                Vector3 playerToObject = item.transform.position - transform.position;//objet - player
                //Debug.DrawRay(transform.position, playerToObject, Color.red);

                #region vérifie obstacle entre player et objet

                RaycastHit _obstacleHit;
               
                if (Physics.Raycast(transform.position, playerToObject, out _obstacleHit, playerToObject.magnitude))
                {
                    // Si l'objet touché n'est pas le même que l'objet que l'on test, c'est qu'il y a un obstacle
                    if (_obstacleHit.collider.gameObject != item.gameObject)
                    {
                        continue; // On passe à l'objet suivant
                    }
                }

                #endregion 
                
                // faire un trie de priorité ici, prendre en compte le centre de l'écran aussi
                
                //set l'objet le plus proche
                if (playerToObject.magnitude < closestDist) // && item.CompareTag("Movable")
                {
                    closestDist = playerToObject.magnitude;
                    _closestObj = item.gameObject;
                }
            }
            
            SetTextInteraction(_closestObj?.GetComponent<InteractionSystem>());
        }
        else
        { 
            _closestObj = null;
            SetTextInteraction();
        }
    }

    public void InteractAction(InputAction.CallbackContext callbackContext)
    {
        if (_closestObj == null)
        {
            return;
        }
        
        InteractionSystem interactable = _closestObj?.GetComponent<InteractionSystem>();
        if (interactable != null)
        {
            interactable.Interact();
        }
        else
        {
            Debug.Log($"L'objet {_closestObj.name} n'a pas  de InteractableObject");
        }
    }
    
    void SetTextInteraction(InteractionSystem interaction = null)
    {
        if (interaction == null)
        {
            interactText?.gameObject.SetActive(false);
            return;
        }
        
        //InteractableObject interactable = _closestObj.GetComponent<InteractableObject>();
        if (interaction != null)
        {
            interactText?.gameObject.SetActive(true);
            
            string key = actionReference.action.GetBindingDisplayString();
            interactText.text = $"{textInteraction} {key} to {interaction.interactionText}";
        }
        else
        {
            interactText?.gameObject.SetActive(false);
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
