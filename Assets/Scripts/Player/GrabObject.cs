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

    private GameObject grabUI;
    
    [Header("Variation")]
    public float launchForce;
    public float grabForce;
    [Tooltip("différence accepté entre position obj grab et où il doit être en main (lache sinon)")]
    public float toleranceRange;// différence accepté entre position obj grab et où il doit être en main (lache sinon)
    
    void Start()
    {
        
        controls = GameManager.controls;
        // controls.Player.GrabAction.performed += GrabAction;
        controls.Player.LaunchAction.performed += ActionLancer;//clic gauche
        controls.Player.DropAction.performed += ActionLacher;//clic droit
        
        handlerPosition = GameObject.FindGameObjectWithTag("HandlerPosition").transform;
        playerCollider = GetComponentsInChildren<Collider>();
        
        grabUI = GameObject.FindGameObjectWithTag("GrabUI"); 
        grabUI?.SetActive(false);
        
    }

    private void OnDisable()
    {
        // controls.Player.GrabAction.performed -= GrabAction;
        controls.Player.LaunchAction.performed -= ActionLancer;
        controls.Player.DropAction.performed -= ActionLacher;
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
                Release(false);//on le lache si bloque
            }
            else
            {
                MoveObject();
            }
        }
    }
    
    // si fait interaction autre que grab, a deplacer dans BoxInteraction
    private void GrabAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isCarrying)
            {
                Carry();
            }
        }
    }

    void ActionLancer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isCarrying)
            {
                Release(true);
            }
        }
    }
    void ActionLacher(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isCarrying)
            {
                Release(false);
            }
        }
    }

    public void MoveObject()
    {
        Vector3 dir = handlerPosition.position - carriedObject.transform.position;
        carriedObject.GetComponent<Rigidbody>().AddForce(dir * grabForce, ForceMode.Acceleration);
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
            
            grabUI?.SetActive(true);
            isCarrying = true;
            _objToCarry = null;//on ne detecte plus les obj proche car on en porte déjà un
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Release(bool isLaunched = true)//à voir pour modif dans FSM repulse
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
                //si action lance sinon on lache a ses pieds
                if (isLaunched)
                {
                    if (!FSM_ObjectState.isGrabbed && !FSM_ObjectState.isKinematic)
                    {
                        FSM_ObjectState.GetThrown(handlerPosition.forward * launchForce);
                        //carriedObject.GetComponent<Rigidbody>().AddForce(handlerPosition.forward * launchForce, ForceMode.Impulse);
                    }
                }
            }
            //reset
            grabUI?.SetActive(false);
            carriedObject = null;
            isCarrying = false;
        }
    }
    
    public void SetCarringPosition(GameObject objetGrab, Boolean inHand)
    {
        Rigidbody rb = objetGrab.GetComponent<Rigidbody>();
        if (inHand)
        {
            objetGrab.transform.SetParent(handlerPosition);

            rb.drag = 10f;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            //marche moyen
            objetGrab.transform.rotation = Quaternion.Euler(0, 0, 0);
            objetGrab.transform.localRotation = Quaternion.Euler(0, 0, 0);
            
        }
        else
        {
            objetGrab.transform.SetParent(_originParent);

            rb.drag = 0f;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
