using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabObject : MonoBehaviour
{
    // var position / parent ou mettre obj
    // taille max qu'on peut porter + offset moitié largeur du player
    // isPickUp pour check si on a un objet ou non
    public CameraController cam;
    private Camera mainCam;
    public Transform parentInteractorFPS;
    public Transform interactorPosFPS;

    public Transform parentInteractorTPS;
    public Transform interactorPosTPS;

    public TextMeshProUGUI interactText;
    private Transform interactorPos;
    public float largeurObjMax;
    public bool isCarrying;
    public LayerMask hitLayer;
    private Ray _ray;
    private RaycastHit _hit;
    private GameObject carriedObject;
    private Transform originParent;

    [Header("Variation")]
    public bool isLaunchable;
    public float launchForce;
    public float interactionRange;
    void Start()
    {
        interactText.gameObject.SetActive(false);
        ChangeParent();
    }

    // Update is called once per frame
    void Update()
    {

        mainCam = cam.Brain.OutputCamera;

        //Vector3 newMousePos = new Vector3(Mathf.Abs(Input.mousePosition.x), Mathf.Abs(Input.mousePosition.y), Mathf.Abs(Input.mousePosition.z));

        if (cam.isFPS)
        {
            _ray = mainCam.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            _ray = new Ray(interactorPos.position, interactorPos.forward);
        }



        if (Physics.Raycast(_ray, out _hit, interactionRange, hitLayer))
        {
            Vector3 objectSize = _hit.collider.bounds.size;
            float objectWidth = objectSize.x;
            if (!_hit.collider.CompareTag("NotInteract") && objectWidth <= largeurObjMax)
                interactText.gameObject.SetActive(true);
            else
                interactText.gameObject.SetActive(false);

            Debug.DrawLine(transform.position, _hit.point, Color.black);
        }
    }

    public void Carrying()
    {
        
        if (Physics.Raycast(_ray, out _hit, interactionRange, hitLayer))
        {
            if (!_hit.collider.CompareTag("NotInteract"))
            {
                Vector3 objectSize = _hit.collider.bounds.size;
                float objectWidth = objectSize.x;

                if (objectWidth <= largeurObjMax) // utilisation du scale actuellement, faudra le changer quand les mesh blender seront toutes à 1
                {
                    carriedObject = _hit.collider.gameObject;
                    originParent = carriedObject.transform.parent;

                    // deplace obj
                    ResetCarryPos();

                    if (carriedObject.GetComponent<Rigidbody>())
                    {
                        carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                        carriedObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("Player");
                    }

                    isCarrying = true;
                    Debug.Log("Grab : " + carriedObject.name);

                }
                else
                {
                    Debug.Log(" Pas Grab - Scale/taille: " + objectWidth);

                }

            }

            Debug.DrawLine(transform.position, _hit.point, Color.black);
        }
    }

    public void Drop()
    {
        carriedObject.transform.SetParent(originParent);
        if (carriedObject.GetComponent<Rigidbody>()) {
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject.GetComponent<Rigidbody>().includeLayers = LayerMask.GetMask("Player");

            if (isLaunchable)
            {
                carriedObject.GetComponent<Rigidbody>().AddForce(interactorPos.forward * launchForce, ForceMode.Impulse);
            }
        }

        //reset
        carriedObject = null;
        isCarrying = false;

        Debug.Log("Drop");
    }
    public void ResetCarryPos()
    {
        Debug.Log("Reset obj carry, interactorPos : " + interactorPos);
        if (carriedObject != null)
        {

            carriedObject.transform.SetParent(interactorPos);
            carriedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            carriedObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            carriedObject.transform.localPosition = Vector3.zero;
        }
    }

    public void ChangeParent()
    {
        // appel dans event camcontroller

        // faire juste un empty pivot dans qui tourne avec le rendere pour TPS et qui tourne avec mouvement cam avec FPS

        if (GameManager.Instance.camControllerScript.isFPS)
        {
            interactorPos = interactorPosFPS;
        }
        else
        {
            interactorPos = interactorPosTPS;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
