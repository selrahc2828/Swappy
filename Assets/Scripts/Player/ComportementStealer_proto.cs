using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComportementStealer_proto : MonoBehaviour
{
    private Controls controls;
    private GameManager gameManager;

    [Header("Raycast")]
    public LayerMask hitLayer;
    private Ray _ray;
    [HideInInspector] public Camera mainCam;

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

    [Header("Animation")] 
    public Anim_manager playeranim;
    
    [Header("FeedBacks")] 
    public Slot_feedback LeftArm;
    public Slot_feedback RightArm;

    public bool simActive;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        controls = GameManager.controls;

        controls.Player.ActionSlot1.performed += ActionSlot1;//clic gauche
        controls.Player.ActionSlot2.performed += ActionSlot2;//clic droit
        controls.Player.SwitchSlotsValue.performed += SwitchSlotsValue;//T
        controls.Player.ApplicationDeComportementSurPlayer.performed += ApplicationDeComportementSurPlayer;//F
        controls.Player.ViderComportementSurPlayer.performed += ViderComportementSurPlayer;//E
        controls.Player.ViderSlots.performed += ViderSlots;//G
        controls.Player.SIMAction.performed += SimAction;//Alt press
        controls.Player.SIMAction.canceled += SimAction;//Alt release
        slot1 = 0;
        slot2 = 0;
        originSlot1 = null;
        originSlot2 = null;

        mainCam = GameManager.Instance.mainCamera;
        slot1Text = GameObject.FindGameObjectWithTag("TextSlot1").GetComponent<TextMeshProUGUI>();
        slot2Text = GameObject.FindGameObjectWithTag("TextSlot2").GetComponent<TextMeshProUGUI>();
        
        LeftArm = GameObject.FindGameObjectWithTag("leftArm").GetComponent<Slot_feedback>();
        RightArm = GameObject.FindGameObjectWithTag("rightArm").GetComponent<Slot_feedback>();

    }

    private void OnDisable()
    {
        controls.Player.ActionSlot1.performed -= ActionSlot1;
        controls.Player.ActionSlot2.performed -= ActionSlot2;
        controls.Player.SwitchSlotsValue.performed -= SwitchSlotsValue;
        controls.Player.ApplicationDeComportementSurPlayer.performed -= ApplicationDeComportementSurPlayer;//F
        controls.Player.ViderComportementSurPlayer.performed -= ViderComportementSurPlayer;//E
        controls.Player.ViderSlots.performed -= ViderSlots;
        controls.Player.SIMAction.performed -= SimAction;//Alt press
        controls.Player.SIMAction.canceled -= SimAction;//Alt release
    }

    private void SimAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            simActive = true;
        }

        if (context.canceled)
        {
            simActive = false;
        }
    }
    
    void ActionSlot1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("ActionSlot1");
            if (GameManager.Instance.grabScript.isCarrying)//on ne peut pas voler/attribuer si on porte un objet
            {
                return;
            }

            if (simActive)
            {
                SimSlot1();
                return;
            }
            
            RaycastHit _hit;

            _ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
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
                            
                            //Debug.Log("Right Value: " + currentObjectState.rightValue);
                            
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
                                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                                    playeranim.Left_Aspiration();
                                    LeftArm.Feedback_Slot_Changed(_hit.transform);
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
                                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                                    playeranim.Left_Attribution();
                                    LeftArm.Feedback_Slot_Changed(null,_hit.transform);
                                }
                                else
                                {
                                    Debug.Log("L'objet contiens d�j� 2 comportements");
                                }
                            }
                            else
                            {
                                Debug.Log("Addition de " + slot1 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name + " - Objet d'origine "+originSlot1?.gameObject.name);
                                int futurState = currentObjectState.stateValue + slot1;
                                currentObjectState.CalculateNewtState(futurState);
                                slot1 = 0;
                                originSlot1 = null;
                                slot1Text.text = "";
                                SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                                playeranim.Left_Attribution();
                                LeftArm.Feedback_Slot_Changed(null,_hit.transform);
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
            
            if (GameManager.Instance.grabScript.isCarrying)//on ne peut pas voler/attribuer si on porte un objet
            {
                return;
            }
            
            if (simActive)
            {
                SimSlot2();
                return;
            }
            
            RaycastHit _hit;

            _ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
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
                                    slot2Text.text = ((FirstState)slot2).ToString();
                                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                                    playeranim.Right_Aspiration();
                                    RightArm.Feedback_Slot_Changed(_hit.transform);
                                }
                                else
                                {
                                    Debug.Log("Soustraction de " + currentObjectState.leftValue + " � " + currentObjectState.stateValue + " - Objet d'origine : "+ _hit.collider.gameObject.name);
                                    int futurState = currentObjectState.stateValue - currentObjectState.leftValue;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = currentObjectState.leftValue;
                                    originSlot2 = _stateStolen;
                                    slot2Text.text = ((FirstState)slot2).ToString();
                                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                                    playeranim.Right_Aspiration();
                                    RightArm.Feedback_Slot_Changed(_hit.transform);
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
                                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                                    playeranim.Right_Attribution();
                                    RightArm.Feedback_Slot_Changed(null,_hit.transform);
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
                                SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                                playeranim.Right_Attribution();
                                RightArm.Feedback_Slot_Changed(null,_hit.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    void SimSlot1() // left
    {

        int playerSlotLeft = 0;
        
        // clamp pour pas avoir de negatif au cas où ? 
        
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference
        
        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            playerSlotLeft = playerObjectState.leftValue;
            Debug.Log($"SimSlot1 playerObjectState right: {playerObjectState.rightValue} / left: {playerObjectState.leftValue}");

            // s'il y a un comportement en main gauche et rien en slot gauche du joueur
            if (slot1 != 0 && playerSlotLeft == 0)
            {
                Debug.Log("Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
                
                int futurState = playerObjectState.stateValue + slot1;
                playerObjectState.CalculateNewtState(futurState);
                slot1 = 0;
                originSlot1 = null;
                slot1Text.text = "";
                SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                playeranim.Left_Attribution();
                LeftArm.Feedback_Slot_Changed(null,null, true);
            }
            // si aucun comportement en main gauche et un comportement dans slot gauche du joueur
            else if (slot1 == 0 && playerSlotLeft != 0)
            {
                // transfère player vers slot1 
                // origin == player
                
                Debug.Log("Soustraction de " + playerSlotLeft + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
                int futurState = playerObjectState.stateValue - playerSlotLeft;
                playerObjectState.CalculateNewtState(futurState);
                slot1 = playerSlotLeft;
                originSlot1 = _stateStolen;
                slot1Text.text = ((FirstState)slot1).ToString();
                SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                playeranim.Left_Aspiration();
                LeftArm.Feedback_Slot_Changed();
            }
            else if (slot1 != 0 && playerSlotLeft != 0)
            {
                Debug.Log($"Échange des valeurs : slot1 ({slot1}) ⇄ player left({playerSlotLeft})");

                (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);

                // Mettre à jour le texte du slot
                slot1Text.text = ((FirstState)slot1).ToString();

                SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                playeranim.Left_Attribution();
                LeftArm.Feedback_Slot_Changed();
            }
        }
        
    }

    void SimSlot2()//right
    {
        
        int playerSlotRight = 0;
        
        // clamp pour pas avoir de negatif au cas où ? 
        
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference
        
        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            playerSlotRight = playerObjectState.rightValue;
            Debug.Log($"SimSlot2 playerObjectState right: {playerObjectState.rightValue} / left: {playerObjectState.leftValue}");

            // s'il y a un comportement en main droite et rien en slot droit du joueur
            if (slot2 != 0 && playerSlotRight == 0)
            {
                Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
                
                int futurState = playerObjectState.stateValue + slot2;
                playerObjectState.CalculateNewtState(futurState);
                slot2 = 0;
                originSlot2 = null;
                slot2Text.text = "";
                SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                playeranim.Right_Attribution();
                RightArm.Feedback_Slot_Changed(null,null, true);
            }
            // si aucun comportement en main gauche et un comportement dans slot gauche du joueur
            else if (slot2 == 0 && playerSlotRight != 0)
            {
                // transfère player vers slot2 
                // origin == player
                
                Debug.Log("Soustraction de " + playerSlotRight + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
                int futurState = playerObjectState.stateValue - playerSlotRight;
                playerObjectState.CalculateNewtState(futurState);
                slot2 = playerSlotRight;
                originSlot2 = _stateStolen;
                slot2Text.text = ((FirstState)slot2).ToString();
                SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                playeranim.Right_Attribution();
                RightArm.Feedback_Slot_Changed();
            }
            else if (slot2 != 0 && playerSlotRight != 0)
            {
                Debug.Log($"Échange des valeurs : slot2 ({slot2}) ⇄ player left({playerSlotRight})");

                (slot2, playerObjectState.rightValue) = (playerObjectState.rightValue, slot2);

                // Mettre à jour le texte du slot
                slot2Text.text = ((FirstState)slot2).ToString();

                SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                playeranim.Right_Attribution();
                RightArm.Feedback_Slot_Changed();
            }
        }    }
    
    void SwitchSlotsValue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            (slot1, slot2) = (slot2, slot1);
            (originSlot1, originSlot2) = (originSlot2, originSlot1);
            slot1Text.text = ((FirstState)slot1).ToString();
            slot2Text.text = ((FirstState)slot2).ToString();
            
            // change Feedback orbe/flare juste un swap color
            RightArm.Feedback_Slot_Changed();
            LeftArm.Feedback_Slot_Changed();
        }
    }

    void ApplicationDeComportementSurPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(slot1 != 0 || slot2 != 0)
            {
                _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la r�f�rence
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
                                    SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                                    playeranim.Left_Attribution();
                                    LeftArm.Feedback_Slot_Changed(null,null, true);//player à gérer
                                }
                                else
                                {
                                    Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
                                    int futurState = playerObjectState.stateValue + slot2;
                                    playerObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
                                    originSlot2 = null;
                                    slot2Text.text = "";
                                    SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                                    playeranim.Right_Attribution();
                                    RightArm.Feedback_Slot_Changed(null, null, true);//player
                                }
                            }
                            else
                            {
                                Debug.Log("Le Player contiens d�j� 2 comportements");
                            }
                        }
                        else
                        {
                            //L'objet vis� � une stateValue superieur a 0 donc sa leftValue est forc�ment remplis, on ne test que la rightValue, si elle a une valeur de 0 on lui ajoute le comportement stoqu�
                            if (playerObjectState.rightValue == 0)
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
                                // playeranim.Left_Attribution();
                                // LeftArm.Feedback_Slot_Changed();
                                // playeranim.Right_Attribution();
                                // RightArm.Feedback_Slot_Changed();
                            }
                            else
                            {
                                Debug.LogWarning("Le Player contiens déjà au moins un comportement et vous essayez de lui en ajouter 2 ");
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Addition de " + slot2 + " et " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                        int futurState = playerObjectState.stateValue + slot2 +slot1;
                        playerObjectState.CalculateNewtState(futurState);
                        
                        if (slot1 != 0)
                        {
                            playeranim.Left_Attribution();
                            LeftArm.Feedback_Slot_Changed(null, null, true);//player 
                        }
                        if (slot2 != 0)
                        {
                            playeranim.Right_Attribution();
                            RightArm.Feedback_Slot_Changed(null, null, true); // player
                        }
                        
                        slot1 = 0;
                        slot2 = 0;
                        originSlot1 = null;
                        originSlot2 = null;
                        slot1Text.text = "";
                        slot2Text.text = "";
                        SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                        // playeranim.Left_Attribution();
                        // LeftArm.Feedback_Slot_Changed(null, null, true);
                        // // playeranim.Right_Attribution();
                        // // RightArm.Feedback_Slot_Changed(null, null, true);
                    }
                }
            }
            else
            {
                Debug.Log("Vous n'avez aucun comportement stoqué");
            }
        }
    }

    void ViderComportementSurPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            #region Ce code est nul et temporaire, il faudra le refaire pour renvoyer les comportement dans les propriétaires
            _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>();
            Debug.Log(_stateStolen);
            Debug.Log(_stateStolen.currentState);
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            Debug.Log(playerObjectState);
            playerObjectState.CalculateNewtState(0);
            #endregion
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
                    //suppression des Feedback orbe/flare comportement sur les bras
                    LeftArm.Feedback_Slot_Changed(null, null, true);
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
                    //suppression des Feedback orbe/flare comportement sur les bras
                    RightArm.Feedback_Slot_Changed(null, null, true);
                }
            }
        }
    }
}
