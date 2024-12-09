using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComportementStealer_proto : MonoBehaviour
{
    private Controls controls;
    private GameManager gameManager;

    [Header("Raycast")]
    public LayerMask hitLayer;
    private Ray _ray;
    public Camera mainCam;

    [Header("Comportement stoqu�")]
    public int slot1 = 0;
    public ComportementsStateMachine originSlot1;
    public int slot2 = 0;
    public ComportementsStateMachine originSlot2;

    [Header("Properties")]
    private ComportementsStateMachine _stateStolen;
    
    [Header("UI")]
    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
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

    private void OnDisable()
    {
        controls.Player.ActionSlot1.performed -= ActionSlot1;
        controls.Player.ActionSlot2.performed -= ActionSlot2;
        controls.Player.SwitchSlotsValue.performed -= SwitchSlotsValue;
        controls.Player.ApplicationDeComportementSurPlayer.performed -= ApplicationDeComportementSurPlayer;
        controls.Player.ViderSlots.performed -= ViderSlots;
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
                    //On verifie si slot1 est superieur � 0, s'il l'est, on cherche alors � donner un comportement � l'objet vis�, sinon on cherche � pr�lever un comportement � l'objet vis�
                    if (slot1 == 0)
                    {
                        _stateStolen = stateMachine; // Stocker la r�f�rence
                        if (_stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.rightValue);
                            //On v�rifie que la stateValue de l'objet vis� est superieur � 0, on ne peut pr�lever un comportement que si c'est le cas.
                            if(currentObjectState.stateValue != 0)
                            {
                                //stateValue est supperieur � 0, leftValue est donc obligatoirement remplie, etant donn� qu'il s'agit du clique gauche, on ne cherche que la leftValue
                                if (currentObjectState.leftValue != 0)
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.leftValue + " � " + currentObjectState.stateValue + " - Objet d'origine : "+ _hit.collider.gameObject.name);
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot1 = currentObjectState.leftValue;
                                    originSlot1 = _stateStolen;
                                    slot1Text.text = ((FirstState)slot1).ToString();
                                }
                                else
                                {
                                    Debug.LogWarning("Normalement c'est litteralement impossible de faire �a");
                                }
                            }
                            else
                            {
                                Debug.Log("L'objet ne contiens aucun comportement � pr�lever");
                            }
                        }
                    }
                    else
                    {
                        _stateStolen = stateMachine; // Stocker la r�f�rence
                        if (_stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                            Debug.Log("Right Value: " + currentObjectState.stateValue);
                            //On verifie si l'objet vis� est vide, si c'est le cas on lui donne directement un comp�rtement avec leftValue, sinon on va v�rifier si sa rightValue est vide
                            if(currentObjectState.stateValue != 0)
                            {
                                //On v�rifie si le rightValue de l'objet Vis� est vide, si c'est le cas, on lui ajoute le comportement stoqu� dans slot1.
                                if (currentObjectState.rightValue == 0)
                                {
                                    Debug.Log("Addition de " + slot1 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name + " - Objet d'origine "+originSlot1.gameObject.name);
                                    int futurState = currentObjectState.stateValue + slot1;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot1 = 0;
                                    originSlot1 = null;
                                    slot1Text.text = "";
                                }
                                else
                                {
                                    Debug.Log("L'objet contiens d�j� 2 comportements");
                                }
                            }
                            else
                            {
                                Debug.Log("Addition de " + slot1 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name + " - Objet d'origine "+originSlot1.gameObject.name);
                                int futurState = currentObjectState.stateValue + slot1;
                                currentObjectState.CalculateNewtState(futurState);
                                slot1 = 0;
                                originSlot1 = null;
                                slot1Text.text = "";
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
                    //On verifie si slot2 est superieur � 0, s'il l'est, on cherche alors � donner un comportement � l'objet vis�, sinon on cherche � pr�lever un comportement � l'objet vis�
                    if (slot2 == 0)
                    {
                        _stateStolen = stateMachine; // Stocker la r�f�rence
                        if (_stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                            //On verifie si l'objet vis� contiens une stateValue, si la stateValue est superieur a 0, l'objet a un comportement
                            if (currentObjectState.stateValue != 0)
                            {
                                //L'objet vis� a une stateValue superieur � 0 donc sa leftValue est forc�ment remplis, on test dans un premier temps la rightValue etant donn� que c'ets le click droit.
                                //Si la rightValue est superieur a 0, on la stoque, sinon on stoque la leftValue.
                                if (currentObjectState.rightValue != 0)
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.rightValue + " � " + currentObjectState.stateValue + " - Objet d'origine : "+ _hit.collider.gameObject.name);
                                    int futurState = currentObjectState.stateValue - currentObjectState.rightValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.rightValue;
                                    originSlot2 = _stateStolen;
                                }
                                else
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.leftValue + " � " + currentObjectState.stateValue + " - Objet d'origine : "+ _hit.collider.gameObject.name);
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.leftValue;
                                    originSlot2 = _stateStolen;
                                    slot2Text.text = ((FirstState)slot2).ToString();
                                }
                            }
                            else
                            {
                                Debug.Log("L'objet ne contiens aucun comportement � pr�lever");
                            }
                        }
                    }
                    else
                    {
                        _stateStolen = stateMachine; // Stocker la r�f�rence
                        if (_stateStolen.currentState is ComportementState)
                        {
                            ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                            //On test si l'objet vis� est vide ou non, s'il est vide, on lui ajoute directement le comportement, sinon on verifie s'il a une place libre
                            if(currentObjectState.stateValue != 0)
                            {
                                //L'objet vis� � une stateValue superieur a 0 donc sa leftValue est forc�ment remplis, on ne test que la rightValue, si elle a une valeur de 0 on lui ajoute le comportement stoqu�
                                if (currentObjectState.rightValue == 0)
                                {
                                    Debug.Log("Soustraction de " + slot2 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name + " - Objet d'origine "+originSlot2.gameObject.name);
                                    int futurState = currentObjectState.stateValue + slot2;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
                                    originSlot2 = null;
                                    slot2Text.text = "";
                                }
                                else
                                {
                                    Debug.Log("L'objet contiens d�j� 2 comportements");
                                }
                            }
                            else
                            {
                                Debug.Log("Soustraction de " + slot2 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name + " - Objet d'origine "+originSlot2.gameObject.name);
                                int futurState = currentObjectState.stateValue + slot2;
                                currentObjectState.CalculateNewtState(futurState);
                                slot2 = 0;
                                originSlot2 = null;
                                slot2Text.text = "";
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
            slot1Text.text = ((FirstState)slot1).ToString();
            slot2Text.text = ((FirstState)slot2).ToString();
        }
    }

    void ApplicationDeComportementSurPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(slot1 != 0 || slot2 != 0)
            {
                _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); ; // Stocker la r�f�rence
                if (_stateStolen.currentState is ComportementState)
                {
                    ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
                    //On test si l'objet vis� est vide ou non, s'il est vide, on lui ajoute directement le comportement, sinon on verifie s'il a une place libre
                    if (playerObjectState.stateValue != 0)
                    {
                        if(slot2 == 0 || slot1 == 0)
                        {
                            //L'objet vis� � une stateValue superieur a 0 donc sa leftValue est forc�ment remplis, on ne test que la rightValue, si elle a une valeur de 0 on lui ajoute le comportement stoqu�
                            if (playerObjectState.rightValue == 0)
                            {

                                if (slot1 != 0)
                                {
                                    Debug.Log("Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
                                    int futurState = playerObjectState.stateValue + slot1;
                                    playerObjectState.CalculateNewtState(futurState);
                                    slot1 = 0;
                                    originSlot1 = null;
                                    slot1Text.text = "";
                                }
                                else
                                {
                                    Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
                                    int futurState = playerObjectState.stateValue + slot2;
                                    playerObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
                                    originSlot2 = null;
                                    slot2Text.text = "";
                                }
                            }
                            else
                            {
                                Debug.Log("Le Player contiens d�j� 2 comportements");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Celui qui arrive a m'afficher cette erreur je lui dois 10 balles");
                        }
                    }
                    else
                    {
                        Debug.Log("Addition de " + slot2 + " et " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                        int futurState = playerObjectState.stateValue + slot2 +slot1;
                        playerObjectState.CalculateNewtState(futurState);
                        slot1 = 0;
                        slot2 = 0;
                        originSlot1 = null;
                        originSlot2 = null;
                        slot1Text.text = "";
                        slot2Text.text = "";
                    }
                }
            }
            else
            {
                Debug.Log("Vous n'avez aucun comportement stoqué");
            }
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
                            Debug.Log("r�tribution du comportement : Addition de " + slot1 + " � " + originObjectState1.stateValue);
                            int futurState = originObjectState1.stateValue + slot1;
                            originObjectState1.CalculateNewtState(futurState);
                            slot1 = 0;
                            originSlot1 = null;
                            slot1Text.text = "";
                        }
                        else
                        {
                            slot1 = 0;
                            originSlot1 = null;
                            slot1Text.text = "";
                            Debug.Log("L'objet d'origine poss�de d�ja 2 comportements, Le comportement stoqu� dans la main gauche � �t� supprim�");
                        }
                    }
                    else
                    {
                        Debug.Log("r�tribution du comportement : Addition de " + slot1 + " � " + originObjectState1.stateValue);
                        int futurState = originObjectState1.stateValue + slot1;
                        originObjectState1.CalculateNewtState(futurState);
                        slot1 = 0;
                        originSlot1 = null;
                        slot1Text.text = "";

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
                            Debug.Log("r�tribution du comportement : Addition de " + slot2 + " � " + originObjectState2.stateValue);
                            int futurState = originObjectState2.stateValue + slot2;
                            originObjectState2.CalculateNewtState(futurState);
                            slot2 = 0;
                            originSlot2 = null;
                            slot2Text.text = "";
                        }
                        else
                        {
                            slot2 = 0;
                            originSlot2 = null;
                            slot2Text.text = "";
                            Debug.Log("L'objet d'origine poss�de d�ja 2 comportements, Le comportement stoqu� dans la main gauche � �t� supprim�");
                        }
                    }
                    else
                    {
                        Debug.Log("r�tribution du comportement : Addition de " + slot2 + " � " + originObjectState2.stateValue);
                        int futurState = originObjectState2.stateValue + slot2;
                        originObjectState2.CalculateNewtState(futurState);
                        slot2 = 0;
                        originSlot2 = null;
                        slot2Text.text = "";
                    }
                }
            }
        }
    }
}
