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
                // SimUni(true);
                // SimSlotAllChange(true);
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
                // SimUni(false);
                // SimSlotAllChange(false);
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

    void SimUni(bool isLeft)
    {
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>();

        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState; // etat a l'instant T
            int playerSlotLeft = playerObjectState.leftValue;
            int playerSlotRight = playerObjectState.rightValue;

            if (playerObjectState.stateValue == 0) // aucun comp
            {
                if (isLeft)
                {
                    _stateStolen.inversion = false;
                    AjouterComp(slot1);
                }
                else
                {
                    _stateStolen.inversion = false;
                    AjouterComp(slot2);
                }
            }
            else if (playerObjectState.leftValue != 0 && playerObjectState.rightValue == 0) // 1 seul comp
            {
                if (_stateStolen.inversion)
                {
                    if (isLeft)
                    {
                        _stateStolen.inversion = true;
                        AjouterComp(slot1);
                    }
                    else
                    {
                        _stateStolen.inversion = true;
                        EchangerComp(slot2);
                    }
                }
                else
                {
                    if (isLeft)
                    {
                        _stateStolen.inversion = false;
                        EchangerComp(slot1);
                    }
                    else
                    {
                        _stateStolen.inversion = false;
                        AjouterComp(slot2);
                    }
                }
            }
            else // 2 comp
            {
                if (_stateStolen.inversion)
                {
                    if (playerObjectState.leftValue > playerObjectState.rightValue)
                    {
                        
                    }
                }
 
            }
            
            
        }
    }

    void AjouterComp(int slot)
    {
        
    }

    void SoustraireComp(int slot)
    {
        
    }

    void EchangerComp(int slot)
    {
        
    }

    void SimSlotAllChange(bool isLeft)
    {
        int playerSlotLeft = 0;
        int playerSlotRight = 0;
        
        // clamp pour pas avoir de negatif au cas où ? 
        
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference

        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            playerSlotLeft = playerObjectState.leftValue;
            playerSlotRight = playerObjectState.rightValue;
            Debug.Log( $"SimSlotAllChange1 playerObjectState left: {playerObjectState.leftValue} / right: {playerObjectState.rightValue}");
            
            
            Debug.Log($"SimSlotAllChange1 - Échange des valeurs : slot1 ({slot1}) ⇄ player left({playerSlotLeft})");
            Debug.Log($"SimSlotAllChange1 - Échange des valeurs : slot2 ({slot2}) ⇄ player right({playerSlotRight})");

            if (!_stateStolen.inversion)
            {
                (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
                (slot2, playerObjectState.rightValue) = (playerObjectState.rightValue, slot2);
            }
            else
            {
                (slot1, playerObjectState.rightValue) = (playerObjectState.rightValue, slot1);
                (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);
            }

            if (playerObjectState.rightValue < playerObjectState.leftValue)
            {
                _stateStolen.inversion = true;
            }
            else
            {
                _stateStolen.inversion = false;
            }
            
            // (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
            slot1Text.text = ((FirstState)slot1).ToString();
            ComportementState newPlayerObjectState= (ComportementState)_stateStolen.currentState; //reset du state actuel
            
            slot1 = slot1;
            slot2 = slot2;
            originSlot1 = _stateStolen;
            originSlot2 = _stateStolen;
            slot1Text.text = ((FirstState)slot1).ToString();
            
            SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
            if (isLeft)
            {
                playeranim.Left_Aspiration();
                LeftArm.Feedback_Slot_Changed();  
            }
            else
            {
                playeranim.Right_Aspiration();
                RightArm.Feedback_Slot_Changed();
            }

        }
    }
    
    void SimSlot1() // left
    {

        // verif quand passe de player à main, car le comp de droite passe à gauche 
        
        int playerSlotLeft = 0;
        int playerSlotRight = 0;
        
        // clamp pour pas avoir de negatif au cas où ? 
        
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference
        
        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            ComportementState newPlayerObjectState;
            playerSlotLeft = playerObjectState.leftValue;
            playerSlotRight = playerObjectState.rightValue;
            Debug.Log($"SimSlot1 playerObjectState left: {playerObjectState.leftValue} / right: {playerObjectState.rightValue}");
#region ancien
            // if (playerSlotLeft == 0)//aucun comportement*
            // {
            //     _stateStolen.inversion = false;
            //
            //
            //     if (slot1 == 0)
            //     {
            //         return;
            //     }
            //     
            //     //ajout comportement
            //     Debug.Log("Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
            //     
            //     int futurState = playerObjectState.stateValue + slot1;
            //     
            //     playerObjectState.CalculateNewtState(futurState);
            //     slot1 = 0;
            //     originSlot1 = null;
            //     slot1Text.text = "";
            //     SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //     playeranim.Left_Attribution();
            //     LeftArm.Feedback_Slot_Changed(null,null, true);
            // }
            // else if (playerSlotRight == 0) //1 comportement a gauche et rien a droite
            // {
            //     if (_stateStolen.inversion)
            //     {
            //         
            //         if (slot1 == 0 ) // soustraction
            //         {
            //             _stateStolen.inversion = true;
            //
            //             Debug.Log("Soustraction de " + playerSlotLeft + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
            //             int futurState = playerObjectState.stateValue - playerSlotLeft;
            //     
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot1 = playerSlotLeft;
            //             originSlot1 = _stateStolen;
            //             slot1Text.text = ((FirstState)slot1).ToString();
            //             SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
            //             playeranim.Left_Aspiration();
            //             LeftArm.Feedback_Slot_Changed();
            //         }
            //         else //addition
            //         {
            //             if (slot1 > playerSlotLeft) // si valeur de ma main > à mon bras on inverse/ verif si inférieur pour bras droit
            //             {
            //                 _stateStolen.inversion = true;
            //             }
            //             else
            //             {
            //                 _stateStolen.inversion = false; //_stateStolen.inversion = !_stateStolen.inversion;
            //             }
            //             
            //             //ajout comportement
            //             Debug.Log("Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
            //     
            //             int futurState = playerObjectState.stateValue + slot1;
            //     
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot1 = 0;
            //             originSlot1 = null;
            //             slot1Text.text = "";
            //             SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //             playeranim.Left_Attribution();
            //             LeftArm.Feedback_Slot_Changed(null,null, true); 
            //         }
            //         
            //
            //     }
            //     else // pas inverse (donc pas a mettre a droite) et j'ai déjà un comportement à gauche
            //     {
            //         _stateStolen.inversion = false;
            //         //echange comportement
            //         
            //         
            //         if (slot1 == 0 ) // soustraction
            //         {
            //             Debug.Log("Soustraction de " + playerSlotLeft + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
            //             int futurState = playerObjectState.stateValue - playerSlotLeft;
            //     
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot1 = playerSlotLeft;
            //             originSlot1 = _stateStolen;
            //             slot1Text.text = ((FirstState)slot1).ToString();
            //             SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
            //             playeranim.Left_Aspiration();
            //             LeftArm.Feedback_Slot_Changed();
            //         }
            //         else
            //         {
            //             Debug.Log($"Échange des valeurs : slot1 ({slot1}) ⇄ player left({playerSlotLeft})");
            //
            //             int futurState = playerObjectState.stateValue - playerSlotLeft + slot1;
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot1 = playerSlotLeft;
            //             originSlot1 = _stateStolen;
            //             
            //             //(slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            //
            //             // Mettre à jour le texte du slot
            //             slot1Text.text = ((FirstState)slot1).ToString();
            //
            //             SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //             playeranim.Left_Attribution();
            //             LeftArm.Feedback_Slot_Changed();                        
            //         }
            //     }
            // }
            // else //1 comportement a gauche ET a droite
            // {
            //     // if (_stateStolen.inversion)
            //     // {
            //         // CAS BRAS DROIT
            //         //bounce3 G  magnet27 D ||| main = impulse1
            //         //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"
            //         
            //         // CAS BRAS GAUCHE
            //         // impulse1 immuable9 ||| main = magnet27
            //         //remplacer impulse1 par magnet27 ==> immuable9 magnet27 or on veut magnet27 immuable9
            //
            //         //par defaut, plus petit toujours a gauche, on doit verifier sa valeur de celui que l'on remplace pour echanger en concequence
            //         // on inverse l'inversion  _stateStolen.inversion = !_stateStolen.inversion;
            //         bool mainValuePlusGrand = false;
            //         
            //         if (slot1 > playerSlotLeft) // si valeur de ma main > à mon bras on inverse/ verif si inférieur pour bras droit
            //         {
            //             mainValuePlusGrand = true;
            //             _stateStolen.inversion = true;
            //         }
            //         else
            //         {
            //             mainValuePlusGrand = false;
            //             _stateStolen.inversion = false; //_stateStolen.inversion = !_stateStolen.inversion;
            //         }
            //
            //         //echange
            //         Debug.Log($"Échange des valeurs : slot1 ({slot1}) ⇄ player left({playerSlotLeft})");
            //
            //         (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            //
            //         // Mettre à jour le texte du slot
            //         slot1Text.text = ((FirstState)slot1).ToString();
            //
            //         SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //         playeranim.Left_Attribution();
            //         LeftArm.Feedback_Slot_Changed();
            //     // }
            // }
            #endregion
            #region ancien2
            
            // // Si aucun comportement n'est actuellement stocké dans slot1 / le bras gauche
            // if (slot1 == 0)
            // {
            //     if (playerObjectState.stateValue != 0 && playerObjectState.leftValue != 0) // sinon on stock le comp alors qu'il est à droite
            //     {
            //         if (playerObjectState.rightValue == 0 && _stateStolen.inversion)
            //         {
            //             return;
            //         }
            //         
            //         int valueToSteal;
            //
            //         if (_stateStolen.inversion)//pour que le "vole" corresponde à ce que je vois 
            //         {
            //             valueToSteal = playerObjectState.rightValue;
            //         }
            //         else
            //         {
            //             valueToSteal = playerObjectState.leftValue;
            //         }
            //         
            //         Debug.Log("SIM - Soustraction de " + valueToSteal + " a " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
            //         
            //         // int valueToSteal = playerObjectState.leftValue;
            //         int futurState = playerObjectState.stateValue - valueToSteal;
            //         playerObjectState.CalculateNewtState(futurState);
            //         slot1 = valueToSteal;
            //         originSlot1 = _stateStolen;
            //         slot1Text.text = ((FirstState)slot1).ToString();
            //
            //         playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
            //
            //         
            //         //verif valeur < pour inverser ou non 
            //         
            //         //ici on retire à gauche, donc si on a encore un comp ça veut dire qu'on en a 1 sur player droite
            //         _stateStolen.inversion = playerObjectState.leftValue != 0; 
            //         // _stateStolen.inversion = (playerObjectState.leftValue < valueToSteal);
            //
            //         // Vérification pour inversion : on compare la rightValue avec la valeur volée
            //         //_stateStolen.inversion = (playerObjectState.rightValue < valueToSteal);
            //
            //         SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
            //         playeranim.Left_Aspiration();
            //         LeftArm.Feedback_Slot_Changed();
            //     }
            //     else
            //     {
            //         Debug.Log("Aucun comportement valable à voler (stateValue ou leftValue == 0)");
            //     }
            // }
            // // Sinon, on ajoute le comportement stocké (dans le bras) sur le player
            // else
            // {
            //     Debug.Log($"leftValue : {playerObjectState.leftValue} / right: {playerObjectState.rightValue} / inversion : {_stateStolen.inversion}");
            //     // Si player possède déjà un comportement à gauche (leftValue non nul)
            //     // et que slot1 est rempli, on échange les valeurs
            //
            //     if (playerObjectState.stateValue !=0 )//left forcément remplis
            //     {
            //         
            //         //addition mais doit verif _stateStolen.inversion
            //         
            //         if (_stateStolen.inversion)
            //         {
            //             if (playerObjectState.rightValue == 0) 
            //             {
            //                 // addition
            //             }
            //             else
            //             {
            //                 // Echange
            //             }
            //         }
            //         else // si j'ai 1 comp sur bras gauche, et que je ne suis pas inversé, j'ehange le comp bras gauche et le slot1
            //         {
            //             //Echange
            //         }
            //         
            //         
            //     }
            //     else
            //     {
            //         //addition (verif _stateStolen.inversion ?)
            //         
            //         Debug.Log("SIM GAUCHE - Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
            //         
            //         int valueToAdd = slot1;
            //         int futurState = playerObjectState.stateValue + valueToAdd;
            //
            //         playerObjectState.CalculateNewtState(futurState);
            //         playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
            //         
            //         // Vérification : inversion est true si rightValue est inférieure à la valeur ajoutée
            //         // CAS BRAS GAUCHE
            //         // impulse1 immuable9 ||| main = magnet27
            //         //remplacer impulse1 par magnet27 ==> immuable9 magnet27 or on veut magnet27 immuable9
            //         
            //         // Debug.Log($"after left value: {playerObjectState.leftValue} / valueToAdd : {valueToAdd} / currentState : {_stateStolen.currentState}");
            //         _stateStolen.inversion = (playerObjectState.leftValue < valueToAdd);
            //         // Debug.Log($"_stateStolen.inversion addition left: {_stateStolen.inversion}");
            //
            //
            //         slot1 = 0;
            //         originSlot1 = null;
            //         slot1Text.text = "";
            //         SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
            //         playeranim.Left_Attribution();
            //         LeftArm.Feedback_Slot_Changed();
            //     }
            //     
            //     
            //     
            //     if (playerObjectState.leftValue != 0 || (playerObjectState.rightValue != 0 && _stateStolen.inversion))// !_stateStolen.inversion     /   || (playerObjectState.rightValue != 0 && _stateStolen.inversion)
            //     {
            //         
            //         // playerObjectState.rightValue != 0 && _stateStolen.inversion SERT A RIEN car playerObjectState.leftValue != 0 toujours bon
            //         // je dois faire en sorte d'aller en addition avec
            //         
            //         // if(playerObjectState.stateValue != 0)
            //         // {
            //         //     //On v�rifie si le rightValue de l'objet Vis� est vide, si c'est le cas, on lui ajoute le comportement stoqu� dans slot1.
            //         //     if (playerObjectState.rightValue == 0)
            //         //
            //         
            //         // CAS SI RIGHT > LEFT ET verif !_stateStolen.inversion, on va en addition avec une valeur de state qui existe pas et ça ecrase tout
            //         
            //         // si je suis en inversion, addition car on a que 1 comportement
            //         int valueToApply = slot1;
            //         int valueToChange;
            //         Debug.LogError($"before slot1: {slot1} / leftValue: {playerObjectState.leftValue} / rightValue: {playerObjectState.rightValue}");
            //         Debug.Log($"inversion: {_stateStolen.inversion}");
            //         // if (_stateStolen.inversion) // pb si inverse et pas de right ? 
            //         // {
            //         //     valueToChange = playerObjectState.rightValue;
            //         //     (slot1, playerObjectState.rightValue) = (playerObjectState.rightValue, slot1);
            //         // }
            //         // else
            //         // {
            //         //     valueToChange = playerObjectState.leftValue;
            //         //     (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            //         //
            //         // }
            //         valueToChange = playerObjectState.leftValue;
            //         (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            //         
            //         Debug.Log($"after slot1: {slot1} / valueToChange : {valueToChange} / leftValue: {playerObjectState.leftValue} / rightValue: {playerObjectState.rightValue}");
            //         // cas slot 1 0 et valueToChange 0
            //         Debug.Log($"SIM - Échange des valeurs : slot1 ({slot1}) ⇄ player left({valueToChange})");
            //         
            //         // (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            //         playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
            //         slot1Text.text = ((FirstState)slot1).ToString();
            //         playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
            //
            //         
            //         // Vérification après échange : si rightValue < nouvelle valeur stockée, inversion devient true
            //         if (playerSlotRight != 0)
            //         {
            //             // CAS BRAS GAUCHE
            //             // impulse1 immuable9 ||| main = magnet27
            //             //remplacer impulse1 par magnet27 ==> immuable9 magnet27 or on veut magnet27 immuable9
            //             if (!_stateStolen.inversion) // si pas inversé, verif normale
            //             {
            //                 if (playerObjectState.leftValue < valueToApply) 
            //                 {
            //                     _stateStolen.inversion = true;
            //
            //                 }
            //                 else
            //                 {
            //                     _stateStolen.inversion = false;
            //                 }
            //             }
            //             else // sinon verif avec l'autre coté
            //             {
            //                 // if (playerObjectState.leftValue < valueToApply)
            //                 // {
            //                 //     _stateStolen.inversion = true;
            //                 // }
            //             }
            //
            //             
            //             // _stateStolen.inversion = (playerObjectState.leftValue < valueToApply);
            //         }
            //         else
            //         {
            //             _stateStolen.inversion = false;
            //         }
            //
            //         SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
            //         playeranim.Left_Attribution();
            //         LeftArm.Feedback_Slot_Changed();
            //     }
            //     // si on a un comportement dans le bras mais pas sur le player
            //     else
            //     {
            //         
            //         // Debug.Log("SIM - Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
            //         //
            //         // int valueToAdd = slot1;
            //         // int futurState = playerObjectState.stateValue + valueToAdd;
            //         //
            //         // playerObjectState.CalculateNewtState(futurState);
            //         // playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
            //         //
            //         // // Vérification : inversion est true si rightValue est inférieure à la valeur ajoutée
            //         // // CAS BRAS GAUCHE
            //         // // impulse1 immuable9 ||| main = magnet27
            //         // //remplacer impulse1 par magnet27 ==> immuable9 magnet27 or on veut magnet27 immuable9
            //         //
            //         // // Debug.Log($"after left value: {playerObjectState.leftValue} / valueToAdd : {valueToAdd} / currentState : {_stateStolen.currentState}");
            //         // _stateStolen.inversion = (playerObjectState.leftValue < valueToAdd);
            //         // // Debug.Log($"_stateStolen.inversion addition left: {_stateStolen.inversion}");
            //         //
            //         //
            //         // slot1 = 0;
            //         // originSlot1 = null;
            //         // slot1Text.text = "";
            //         // SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
            //         // playeranim.Left_Attribution();
            //         // LeftArm.Feedback_Slot_Changed();
            //     }
            // }
            #endregion

            if (slot1 == 0)
            {
                if (playerObjectState.stateValue !=0)
                {
                    int aRecuperer = playerObjectState.leftValue;
                    
                    if (_stateStolen.inversion)
                    {
                        if (playerObjectState.rightValue == 0)// rien sur bras gauche (car il est visuellement a droite)
                        {
                            return;
                        }
                        else // visuellement sur le bras gauche on a le comportement de droite, donc c'est lui qu'on soustrait
                        {
                            aRecuperer = playerObjectState.rightValue;
                        }
                    }
                    
                    //soustraction
                    
                    Debug.Log("SIM GAUCHE - Soustraction de " + aRecuperer + " a " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
                    
                    // int valueToSteal = playerObjectState.leftValue;
                    int futurState = playerObjectState.stateValue - aRecuperer;
                    playerObjectState.CalculateNewtState(futurState);
                    
                    slot1 = aRecuperer;
                    originSlot1 = _stateStolen;
                    slot1Text.text = ((FirstState)slot1).ToString();
                    
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    
                    // verif si je dois inverser
                    if (!_stateStolen.inversion)
                    {
                        if (newPlayerObjectState.leftValue !=0) // j'ai retirer comp de gauche mais il y en a encore 1, donc il était à droite alors j'inverse
                        {
                            _stateStolen.inversion = true;
                        }
                        
                        // controle slot 1 et leftValue 
                    }
                    
                    //verif valeur < pour inverser ou non 
                    
                    //ici on retire à gauche, donc si on a encore un comp ça veut dire qu'on en a 1 sur player droite
                    //_stateStolen.inversion = playerObjectState.leftValue != 0; 
                    //_stateStolen.inversion = (playerObjectState.leftValue < valueToSteal);
                    
                    // Vérification pour inversion : on compare la rightValue avec la valeur volée
                    //_stateStolen.inversion = (playerObjectState.rightValue < valueToSteal);
                    
                    
                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                    playeranim.Left_Aspiration();
                    LeftArm.Feedback_Slot_Changed();
                    
                }
                else // rien dans le slot1 / main gauche et rien dans le bras gauche
                {
                    Debug.Log("SIM GAUCHE - rien a recuperer, voler ou echanger");
                }
            }
            else
            {
                //Debug.Log($"SIM GAUCHE leftValue : {playerObjectState.leftValue} / right: {playerObjectState.rightValue} / inversion : {_stateStolen.inversion}");
                
                // Si player possède déjà un comportement à gauche (leftValue non nul)
                // et que slot1 est rempli, on échange les valeurs

                if (playerObjectState.stateValue !=0 )//left forcément remplis
                {
                    
                    //addition mais doit verif _stateStolen.inversion
                    
                    if (_stateStolen.inversion)
                    {
                        if (playerObjectState.rightValue == 0) // 1 comportement sur player droit et rien sur sa gauche, j'additionne et check inversion
                        {
                            // addition
                            Debug.Log("SIM GAUCHE - Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
                    
                            int valueToAdd = slot1;
                            int futurState = playerObjectState.stateValue + valueToAdd;

                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            // controle _stateStolen.inversion ?
                            _stateStolen.inversion = (newPlayerObjectState.leftValue < slot1);
                            
                            
                            slot1 = 0;
                            originSlot1 = null;
                            slot1Text.text = "";
                            SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                            playeranim.Left_Attribution();
                            LeftArm.Feedback_Slot_Changed();
                        }
                        else // rightValue est le comportement sur la gauche du player, je dois echanger avec slot 1
                        {
                            // Echange slot1 et rightValue
                            
                            Debug.Log($"SIM GAUCHE - Echange(Inversion) des valeurs : slot1 ({slot1}) ⇄ player rightValue ({playerObjectState.rightValue})");
                            // CAS BRAS DROIT
                            //bounce3 G  magnet27 D ||| main = impulse1
                            //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"
                            
                            (slot1, playerObjectState.rightValue) = (playerObjectState.rightValue, slot1);

                            playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            //slot1 = 0;
                            originSlot1 = _stateStolen;
                            slot1Text.text = ((FirstState)slot1).ToString();
                            SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                            playeranim.Left_Attribution();
                            LeftArm.Feedback_Slot_Changed();
                        }
                    }
                    else // si j'ai 1 comp sur bras gauche, et que je ne suis pas inversé, j'echange le comp bras gauche et le slot1
                    {
                        //Echange slot 1 leftValue
                        Debug.Log($"SIM GAUCHE - Echange des valeurs : slot1 ({slot1}) ⇄ player leftValue ({playerObjectState.leftValue})");
                        
                        (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
                        slot1Text.text = ((FirstState)slot1).ToString();
                        
                        playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
                        newPlayerObjectState = (ComportementState)_stateStolen.currentState;

                        
                        //slot1 = 0;
                        originSlot1 = _stateStolen;
                        slot1Text.text = ((FirstState)slot1).ToString();
                        SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                        playeranim.Left_Attribution();
                        LeftArm.Feedback_Slot_Changed();
                        
                    }
                    
                }
                else // j'ai un comp en main gauche et rien sur player donc j'additionne
                {
                    //addition (verif _stateStolen.inversion ?)
                    
                    Debug.Log("SIM GAUCHE - Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot1.gameObject.name);
                    
                    int valueToAdd = slot1;
                    int futurState = playerObjectState.stateValue + valueToAdd;

                    playerObjectState.CalculateNewtState(futurState);
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    // Vérification : inversion est true si rightValue est inférieure à la valeur ajoutée
                    // CAS BRAS GAUCHE
                    // impulse1 immuable9 ||| main = magnet27
                    //remplacer impulse1 par magnet27 ==> immuable9 magnet27 or on veut magnet27 immuable9
                    
                    // Debug.Log($"after left value: {playerObjectState.leftValue} / valueToAdd : {valueToAdd} / currentState : {_stateStolen.currentState}");
                    _stateStolen.inversion = (newPlayerObjectState.leftValue < valueToAdd);
                    // Debug.Log($"_stateStolen.inversion addition left: {_stateStolen.inversion}");


                    slot1 = 0;
                    originSlot1 = null;
                    slot1Text.text = "";
                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                    playeranim.Left_Attribution();
                    LeftArm.Feedback_Slot_Changed();
                } 
            }
            
            Debug.Log($"_stateStolen.inversion left : {_stateStolen.inversion}");
        }
        
    }

    void SimSlot2()//right
    {
       
        int playerSlotRight = 0;
        int playerSlotLeft = 0;
        
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference
        
        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            playerSlotRight = playerObjectState.rightValue;
            playerSlotLeft = playerObjectState.leftValue;

            Debug.Log($"SimSlot2 playerObjectState right: {playerObjectState.rightValue} / left: {playerObjectState.leftValue}");

            #region MyRegion
            //
            //
            //
            // if (playerSlotLeft == 0)//aucun comportement
            // {
            //     _stateStolen.inversion = true;
            //
            //     if (slot2 == 0)
            //     {
            //         return;
            //     }
            //     
            //     //ajout comportement
            //     Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
            //     
            //     int futurState = playerObjectState.stateValue + slot2;
            //     
            //     playerObjectState.CalculateNewtState(futurState);
            //     slot2 = 0;
            //     originSlot2 = null;
            //     slot2Text.text = "";
            //     SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //     playeranim.Right_Attribution();
            //     RightArm.Feedback_Slot_Changed(null,null, true);
            // }
            // else if (playerSlotRight == 0) //1 comportement a gauche et rien a droite
            // {
            //     
            //         
            //     if (slot2 < playerSlotLeft) // si valeur de ma main < à mon bras on inverse
            //     {
            //         _stateStolen.inversion = true;
            //     }
            //     else
            //     {
            //         _stateStolen.inversion = false; //_stateStolen.inversion = !_stateStolen.inversion;
            //     }
            //     
            //     if (_stateStolen.inversion)
            //     {
            //         _stateStolen.inversion = true;
            //         
            //         if (slot2 == 0 ) // soustraction, visu à droite MAIS on a que une leftValue
            //         {
            //             Debug.Log("Soustraction de " + playerSlotLeft + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
            //             int futurState = playerObjectState.stateValue - playerSlotLeft;
            //     
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot2 = playerSlotLeft;
            //             originSlot2 = _stateStolen;
            //             slot2Text.text = ((FirstState)slot2).ToString();
            //             SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
            //             playeranim.Right_Attribution();
            //             RightArm.Feedback_Slot_Changed();
            //         }
            //         else //addition
            //         {
            //             //ajout comportement
            //             Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
            //     
            //             int futurState = playerObjectState.stateValue + slot2;
            //     
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot2 = 0;
            //             originSlot2 = null;
            //             slot2Text.text = "";
            //             SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //             playeranim.Right_Attribution();
            //             RightArm.Feedback_Slot_Changed(null,null, true); 
            //         }
            //         
            //
            //     }
            //     else // pas inverse (donc pas a mettre a droite) et j'ai déjà un comportement à gauche
            //     {
            //         _stateStolen.inversion = false;
            //         //echange comportement
            //         
            //         
            //         if (slot2 == 0 ) // soustraction
            //         {
            //             Debug.Log("Soustraction de " + playerSlotRight + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
            //             int futurState = playerObjectState.stateValue - playerSlotRight;
            //     
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot2 = playerSlotRight;
            //             originSlot2 = _stateStolen;
            //             slot2Text.text = ((FirstState)slot2).ToString();
            //             SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
            //             playeranim.Right_Aspiration();
            //             RightArm.Feedback_Slot_Changed();
            //         }
            //         else
            //         {
            //             Debug.Log($"Échange des valeurs : slot2 ({slot2}) ⇄ player right({playerSlotRight})");
            //
            //             int futurState = playerObjectState.stateValue - playerSlotRight + slot2;
            //             playerObjectState.CalculateNewtState(futurState);
            //             slot2 = playerSlotRight;
            //             originSlot2 = _stateStolen;
            //             
            //             //(slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
            //
            //             // Mettre à jour le texte du slot
            //             slot2Text.text = ((FirstState)slot2).ToString();
            //
            //             SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //             playeranim.Left_Attribution();
            //             LeftArm.Feedback_Slot_Changed();                        
            //         }
            //     }
            // }
            // else //1 comportement a gauche ET a droite
            // {
            //     // if (_stateStolen.inversion)
            //     // {
            //         // CAS BRAS DROIT
            //         //bounce3 G  magnet27 D ||| main = impulse1
            //         //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"
            //         
            //         // CAS BRAS GAUCHE
            //         // impulse1 immuable9 ||| main = magnet27
            //         //remplacer impulse1 par magnet27 ==> immuable9 magnet27 or on veut magnet27 immuable9
            //
            //         //par defaut, plus petit toujours a gauche, on doit verifier sa valeur de celui que l'on remplace pour echanger en concequence
            //         // on inverse l'inversion  _stateStolen.inversion = !_stateStolen.inversion;
            //         bool mainValuePlusGrand = false;
            //         
            //         if (slot2 < playerSlotRight) // si valeur de ma main < à mon bras on inverse
            //         {
            //             mainValuePlusGrand = true;
            //             _stateStolen.inversion = true;
            //         }
            //         else
            //         {
            //             mainValuePlusGrand = false;
            //             _stateStolen.inversion = false; //_stateStolen.inversion = !_stateStolen.inversion;
            //         }
            //
            //         //echange
            //         Debug.Log($"Échange des valeurs : slot2 ({slot2}) ⇄ player left({playerSlotLeft})");
            //
            //         (slot2, playerObjectState.rightValue) = (playerObjectState.rightValue, slot2);
            //
            //         // Mettre à jour le texte du slot
            //         slot2Text.text = ((FirstState)slot2).ToString();
            //
            //         SoundManager.Instance.PlaySoundComponentPlace(gameObject);
            //         playeranim.Right_Attribution();
            //         RightArm.Feedback_Slot_Changed();
            //     // }
            // }
            #endregion
            // Si aucun comportement n'est actuellement stocké dans slot2 / bras droit = on le stock
            if (slot2 == 0)
            {
                if (playerObjectState.stateValue != 0)
                { // on le stock
                    
                    if (playerObjectState.rightValue == 0 && !_stateStolen.inversion)
                    {
                        return;
                    } 
                    
                    // int valueToSteal = (playerObjectState.rightValue != 0) ? playerObjectState.rightValue : playerObjectState.leftValue;
                    int valueToSteal;

                    if (playerObjectState.rightValue != 0)
                    {
                        valueToSteal = playerObjectState.rightValue;
                    }
                    else
                    {
                        valueToSteal = playerObjectState.leftValue;
                    }
                    
                    Debug.Log("SIM  -  Soustraction de " + valueToSteal + " a " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);

                    
                    int futurState = playerObjectState.stateValue - valueToSteal;
                    playerObjectState.CalculateNewtState(futurState);
                    playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    slot2 = valueToSteal;
                    originSlot2 = _stateStolen;
                    slot2Text.text = ((FirstState)slot2).ToString();

                    // si bras gauche vide / leftValue est à 0, inversion devient true
                    if (playerObjectState.rightValue == 0)
                    {
                        _stateStolen.inversion = true;
                    }
                    

                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                    playeranim.Right_Aspiration();
                    RightArm.Feedback_Slot_Changed();
                }
                else
                {
                    Debug.Log("L'objet ne contient aucun comportement à voler");
                }
            }
            // Sinon, on applique le comportement stocké
            else
            {
                // Si bras player possède déjà un comportement à droite (rightValue non nul)
                // et que slot2 est rempli, on échange les valeurs
                if (playerObjectState.rightValue != 0 || (playerObjectState.leftValue != 0 && _stateStolen.inversion))
                {
                    int valueToChange;
                    if (playerObjectState.rightValue == 0)
                    {
                        valueToChange = playerObjectState.leftValue;
                        (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);

                    }
                    else if (_stateStolen.inversion)
                    {
                        valueToChange = playerObjectState.leftValue;
                        (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);
                    }else
                    {
                        valueToChange = playerObjectState.rightValue;
                        (slot2, playerObjectState.rightValue) = (playerObjectState.rightValue, slot2);

                    }
                    Debug.Log($"SIM - Échange des valeurs : slot2 ({slot2}) ⇄ player right({valueToChange})");
                    
                    playerObjectState.CalculateNewtState(playerObjectState.rightValue + playerObjectState.leftValue);
                    playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    slot2Text.text = ((FirstState)slot2).ToString();

                    
                    // CAS BRAS DROIT
                    //bounce3 G  magnet27 D ||| main = impulse1
                    //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"

                    
                    // Vérification : inversion est true si leftValue est inférieure à la valeur ajoutée
                    // si la valeur de droite est < à la valeur que je voulais ajouter, qui doit etre a droite, on inverse
                    if (playerObjectState.rightValue == 0)
                    {
                        _stateStolen.inversion = true;

                    }
                    else
                    {
                        if (playerObjectState.rightValue == slot2)
                        {
                            _stateStolen.inversion = (playerObjectState.leftValue < slot2);
                        }
                        else
                        {
                            _stateStolen.inversion = (playerObjectState.rightValue < slot2);
                        }

                    }
                    // _stateStolen.inversion = (playerObjectState.rightValue > slot2);
                    Debug.Log($"inversion : {_stateStolen.inversion} / rightValue {playerObjectState.rightValue} / slot2 {slot2}");
                    // Vérification après échange : pour SimSlot2, si leftValue < nouvelle valeur ajoutée, inversion devient true
                    // _stateStolen.inversion = (playerObjectState.leftValue < slot2);

                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                    playeranim.Right_Attribution();
                    RightArm.Feedback_Slot_Changed();
                }
                else
                {
                    
                    Debug.Log("SIM  - Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
                    int valueToAdd = slot2;
                    int futurState = playerObjectState.stateValue + valueToAdd;
                    playerObjectState.CalculateNewtState(futurState);
                    playerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel

                    // CAS BRAS DROIT
                    //bounce3 G  magnet27 D ||| main = impulse1
                    //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"

                    
                    // Vérification : inversion est true si leftValue est inférieure à la valeur ajoutée
                    // si la valeur de droite est < à la valeur que je voulais ajouter, qui doit etre a droite, on inverse
                    _stateStolen.inversion = (playerObjectState.rightValue < valueToAdd);

                    // if (!_stateStolen.inversion)
                    // {
                    //     // _stateStolen.inversion = (playerObjectState.rightValue < valueToAdd);
                    // }
                    // else
                    // {
                    //     _stateStolen.inversion = true;
                    // }
                    Debug.Log($"after check left : {playerObjectState.leftValue} / right: {playerObjectState.rightValue} \n valueToAdd : {valueToAdd}");

                    slot2 = 0;
                    originSlot2 = null;
                    slot2Text.text = "";
                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                    playeranim.Right_Attribution();
                    RightArm.Feedback_Slot_Changed();
                }
            }
            
            /*
            // s'il y a un comportement en main droite et rien en slot droit du joueur
            if (slot2 != 0 && playerSlotRight == 0)
            {
                Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name + " - Objet d'origine " + originSlot2.gameObject.name);
                
                // si 1 dans droit faire inverse
                int futurState = playerObjectState.stateValue + slot2;

                // ici j'ajoute forcément un comp à droite, donc la valeur de mon statue est forcément la valeur que j'ai à gauche
                int leftValue = playerObjectState.stateValue; 
                Debug.Log($"rightValue clic gauche: {playerObjectState.stateValue}");
                // if (leftValue != 0)
                // {
                //     _stateStolen.updateRight = true; // on inverse les valeurs left et right pour placer le comportement à "droite" du joueur
                // }
                _stateStolen.updateRight = true; // on inverse les valeurs left et right pour placer le comportement à "droite" du joueur

                //PB DU CLIC DROIT PAS DU GAUCHE
                
                playerObjectState.CalculateNewtState(futurState);
                slot2 = 0;
                originSlot2 = null;
                slot2Text.text = "";
                SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                playeranim.Right_Attribution();
                RightArm.Feedback_Slot_Changed(null,null, true);
                
                
                _stateStolen.updateRight = false; // reset pour les autre modif

            }
            // si aucun comportement en main droite et un comportement dans slot doit du joueur
            else if (slot2 == 0 && playerSlotRight != 0)
            {
                // transfère player vers slot2
                // origin == player
                
                Debug.Log("Soustraction de " + playerSlotRight + " � " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
                int futurState = playerObjectState.stateValue - playerSlotRight;
                
                // verif si playerSlotLeft != 0 ??
                
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
            
            */
            Debug.Log($"_stateStolen.inversion right : {_stateStolen.inversion}");

        }
        
    }
    
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
