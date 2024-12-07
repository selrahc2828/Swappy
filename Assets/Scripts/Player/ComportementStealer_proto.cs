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
    public ComportementsStateMachine originSlot1;
    public int slot2 = 0;
    public ComportementsStateMachine originSlot2;

    [Header("Properties")]
    public ComportementsStateMachine stateStolen;
    public ComportementState state;

    // Start is called before the first frame update
    void Start()
    {
        controls = GameManager.controls;

        controls.Player.ActionSlot1.performed += ActionSlot1;
        controls.Player.ActionSlot2.performed += ActionSlot2;
        controls.Player.SwitchSlotsValue.performed += SwitchSlotsValue;
        controls.Player.ApplicationDeComportementSurPlayer.performed += ApplicationDeComportementSurPlayer;
        controls.Player.ViderSlots.performed += ViderSlots;
        slot1 = 0;
        slot2 = 0;
        originSlot1 = null;
        originSlot2 = null;
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
                    //On verifie si slot1 est superieur à 0, s'il l'est, on cherche alors à donner un comportement à l'objet visé, sinon on cherche à prélever un comportement à l'objet visé
                    if (slot1 == 0)
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.stateValue);
                            //On vérifie que la stateValue de l'objet visé est superieur à 0, on ne peut prélever un comportement que si c'est le cas.
                            if(currentObjectState.stateValue != 0)
                            {
                                //stateValue est supperieur à 0, leftValue est donc obligatoirement remplie, etant donné qu'il s'agit du clique gauche, on ne cherche que la leftValue
                                if (currentObjectState.leftValue != 0)
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.leftValue + " à " + currentObjectState.stateValue);
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot1 = currentObjectState.leftValue;
                                    originSlot1 = stateStolen;
                                }
                                else
                                {
                                    Debug.LogWarning("Normalement c'est litteralement impossible de faire ça");
                                }
                            }
                            else
                            {
                                Debug.Log("L'objet ne contiens aucun comportement à prélever");
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
                            //On verifie si l'objet visé est vide, si c'est le cas on lui donne directement un compôrtement avec leftValue, sinon on va vérifier si sa rightValue est vide
                            if(currentObjectState.stateValue != 0)
                            {
                                //On vérifie si le rightValue de l'objet Visé est vide, si c'est le cas, on lui ajoute le comportement stoqué dans slot1.
                                if (currentObjectState.rightValue == 0)
                                {
                                    Debug.Log("Addition de " + slot1 + " à " + currentObjectState.stateValue);
                                    int futurState = currentObjectState.stateValue + slot1;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot1 = 0;
                                    originSlot1 = null;
                                }
                                else
                                {
                                    Debug.Log("L'objet contiens déjà 2 comportements");
                                }
                            }
                            else
                            {
                                Debug.Log("Addition de " + slot1 + " à " + currentObjectState.stateValue);
                                int futurState = currentObjectState.stateValue + slot1;
                                currentObjectState.CalculateNewtState(futurState);
                                slot1 = 0;
                                originSlot1 = null;
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
                    //On verifie si slot2 est superieur à 0, s'il l'est, on cherche alors à donner un comportement à l'objet visé, sinon on cherche à prélever un comportement à l'objet visé
                    if (slot2 == 0)
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            //On verifie si l'objet visé contiens une stateValue, si la stateValue est superieur a 0, l'objet a un comportement
                            if (currentObjectState.stateValue != 0)
                            {
                                //L'objet visé a une stateValue superieur à 0 donc sa leftValue est forcément remplis, on test dans un premier temps la rightValue etant donné que c'ets le click droit.
                                //Si la rightValue est superieur a 0, on la stoque, sinon on stoque la leftValue.
                                if (currentObjectState.rightValue != 0)
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.rightValue + " à " + currentObjectState.stateValue);
                                    int futurState = currentObjectState.stateValue - currentObjectState.rightValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.rightValue;
                                    originSlot2 = stateStolen;
                                }
                                else
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.leftValue + " à " + currentObjectState.stateValue);
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.leftValue;
                                    originSlot2 = stateStolen;
                                }
                            }
                            else
                            {
                                Debug.Log("L'objet ne contiens aucun comportement à prélever");
                            }
                        }
                    }
                    else
                    {
                        stateStolen = stateMachine; // Stocker la référence
                        if (stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)stateStolen.currentState;
                            //On test si l'objet visé est vide ou non, s'il est vide, on lui ajoute directement le comportement, sinon on verifie s'il a une place libre
                            if(currentObjectState.stateValue != 0)
                            {
                                //L'objet visé à une stateValue superieur a 0 donc sa leftValue est forcément remplis, on ne test que la rightValue, si elle a une valeur de 0 on lui ajoute le comportement stoqué
                                if (currentObjectState.rightValue == 0)
                                {
                                    Debug.Log("Soustraction de " + slot2 + " à " + currentObjectState.stateValue);
                                    int futurState = currentObjectState.stateValue + slot2;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
                                    originSlot2 = null;
                                }
                                else
                                {
                                    Debug.Log("L'objet contiens déjà 2 comportements");
                                }
                            }
                            else
                            {
                                Debug.Log("Soustraction de " + slot2 + " à " + currentObjectState.stateValue);
                                int futurState = currentObjectState.stateValue + slot2;
                                currentObjectState.CalculateNewtState(futurState);
                                slot2 = 0;
                                originSlot2 = null;
                            }
                        }
                    }
                }
            }
        }
    }

    void SwitchSlotsValue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            slot1 = slot1 + slot2;
            slot2 = slot1 - slot2;
            slot1 = slot1 - slot2;
        }
    }

    void ApplicationDeComportementSurPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    void ViderSlots(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(slot1 != 0)
            {
                if (originSlot1.currentState is ComportementState)
                {
                    ComportementState originObjectState1 = (ComportementState)originSlot1.currentState;
                    if (originObjectState1.stateValue != 0)
                    {
                        if (originObjectState1.rightValue == 0)
                        {
                            Debug.Log("rétribution du comportement : Addition de " + slot1 + " à " + originObjectState1.stateValue);
                            int futurState = originObjectState1.stateValue + slot1;
                            originObjectState1.CalculateNewtState(futurState);
                            slot1 = 0;
                            originSlot1 = null;
                        }
                        else
                        {
                            slot1 = 0;
                            originSlot1 = null;
                            Debug.Log("L'objet d'origine possède déja 2 comportements, Le comportement stoqué dans la main gauche à été supprimé");
                        }
                    }
                    else
                    {
                        Debug.Log("rétribution du comportement : Addition de " + slot1 + " à " + originObjectState1.stateValue);
                        int futurState = originObjectState1.stateValue + slot1;
                        originObjectState1.CalculateNewtState(futurState);
                        slot1 = 0;
                        originSlot1 = null;
                    }
                }
            }

            if (slot2 != 0)
            {
                if (originSlot2.currentState is ComportementState)
                {
                    ComportementState originObjectState2 = (ComportementState)originSlot2.currentState;
                    if (originObjectState2.stateValue != 0)
                    {
                        if (originObjectState2.rightValue == 0)
                        {
                            Debug.Log("rétribution du comportement : Addition de " + slot2 + " à " + originObjectState2.stateValue);
                            int futurState = originObjectState2.stateValue + slot2;
                            originObjectState2.CalculateNewtState(futurState);
                            slot2 = 0;
                            originSlot2 = null;
                        }
                        else
                        {
                            slot2 = 0;
                            originSlot2 = null;
                            Debug.Log("L'objet d'origine possède déja 2 comportements, Le comportement stoqué dans la main gauche à été supprimé");
                        }
                    }
                    else
                    {
                        Debug.Log("rétribution du comportement : Addition de " + slot2 + " à " + originObjectState2.stateValue);
                        int futurState = originObjectState2.stateValue + slot2;
                        originObjectState2.CalculateNewtState(futurState);
                        slot2 = 0;
                        originSlot2 = null;
                    }
                }
            }
        }
    }
}
