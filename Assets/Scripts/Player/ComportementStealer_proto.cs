using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComportementStealer_proto : MonoBehaviour
{
    private Controls controls;

    [Header("Raycast")]
    public LayerMask hitLayer;
    private Ray _ray;
    public Camera mainCam;

    [Header("Comportement stoqué")]
    public int slot1 = 0;
    public int slot2 = 0;

    [Header("Properties")]
    public GameObject objectStolen;
    public ComportementsStateMachine stateStolen;
    public ComportementState state;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;

        controls.Player.ActionSlot1.performed += ActionSlot1;
        controls.Player.ActionSlot2.performed += ActionSlot2;
        controls.Player.ApplicationDeComportementSurPlayer.performed += ApplicationDeComportementSurPlayer;
        controls.Player.ViderComportementDuPlayer.performed += ViderComportementDuPlayer;
        slot1 = 0;
        slot2 = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ActionSlot1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit _hit;

            _ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
            {
                if (_hit.collider == null || _hit.collider.CompareTag("NotInteract"))
                {
                    return;
                }
                var stateMachine = _hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
                if (stateMachine != null)
                {
                    if (slot1 == 0)
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.stateValue);
                            if(currentObjectState.stateValue != 0)
                            {
                                if (currentObjectState.leftValue != 0)
                                {
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot1 = currentObjectState.leftValue;
                                }
                            }
                        }
                    }
                    else
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.stateValue);
                            if (currentObjectState.rightValue == 0)
                            {
                                int futurState = currentObjectState.stateValue + slot1;
                                currentObjectState.CalculateNewtState(futurState);
                                slot1 = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    void ActionSlot2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit _hit;

            _ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
            {
                if (_hit.collider == null || _hit.collider.CompareTag("NotInteract"))
                {
                    return;
                }
                var stateMachine = _hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
                if (stateMachine != null)
                {
                    if (slot2 == 0)
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.stateValue);
                            if (currentObjectState.stateValue != 0)
                            {
                                if (currentObjectState.rightValue != 0)
                                {
                                    int futurState = currentObjectState.stateValue - currentObjectState.rightValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.rightValue;
                                }
                                else
                                {
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.leftValue;
                                }
                            }
                        }
                    }
                    else
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.stateValue);
                            if(currentObjectState.stateValue != 0)
                            {
                                if (currentObjectState.rightValue == 0)
                                {
                                    int futurState = currentObjectState.stateValue + slot2;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
                                }
                            }
                            else
                            {
                                int futurState = currentObjectState.stateValue + slot2;
                                currentObjectState.CalculateNewtState(futurState);
                                slot2 = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    void ApplicationDeComportementSurPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    void ViderComportementDuPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
}
