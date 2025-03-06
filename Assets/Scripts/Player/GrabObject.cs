using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GrabObject : MonoBehaviour
{
    public Controls controls;
    public Transform handlerPosition;

    // private Collider playerCollider;
    [HideInInspector] public Collider[] playerCollider; // on a 2 colliders
    [HideInInspector] public bool isCarrying;
    [HideInInspector] public GameObject carriedObject;
    private Transform _originParent;//pour replacer l'objet une fois lâché
    
    private GameObject _objToCarry;
    public GameObject objToCarry
    {
        get { return _objToCarry; }
        set { _objToCarry = value; }
    }
    
    [Header("Variation")]
    public bool canThrow;
    public float launchForce;
    public float grabForce;
    [Tooltip("différence accepté entre position obj grab et où il doit être en main (lache sinon)")]
    public float toleranceRange;// différence accepté entre position obj grab et où il doit être en main (lache sinon)
    
    void Start()
    {
        
        controls = GameManager.controls;
        
        controls.Player.GrabDrop.performed += GrabAction;

        handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        playerCollider = GetComponentsInChildren<Collider>();
    }

    private void OnDisable()
    {
        controls.Player.GrabDrop.performed -= GrabAction;
    }

    // Update is called once per frame
    void Update()
    {

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
    
    private void GrabAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isCarrying)
            {
                Drop();
            }
            else
            {
                Carry();
            }
        }
    }

    public void MoveObject()
    {
        Vector3 dir = handlerPosition.position - carriedObject.transform.position;
        carriedObject.GetComponent<Rigidbody>().AddForce(dir * grabForce);
    }
    
    public void Carry()
    {
        if (_objToCarry != null && !isCarrying ) // rb déjà controle dans BoxInteraction
        {
            carriedObject = _objToCarry;
            // on save le parent car on va déplacer l'objet en enfant de handlerPosition
            _originParent = carriedObject.transform.parent;

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
            _objToCarry = null;//on ne detecte plus les obj proche car on en porte déjà un
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
                //lance que si lance actif ET rien en main ou si impulse quand porte
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
}
