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
    public int slot2 = 0;

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
        playeranim = FindObjectOfType<Anim_manager>();
        
        controls.Player.ActionSlot1.performed += ActionSlot1;//clic gauche
        controls.Player.ActionSlot2.performed += ActionSlot2;//clic droit
        controls.Player.SwitchSlotsValue.performed += SwitchSlotsValue;//T
        controls.Player.ApplicationDeComportementSurPlayer.performed += ApplicationDeComportementSurPlayer;//F
        controls.Player.SIMAction.performed += SimAction;//Alt press
        controls.Player.SIMAction.canceled += SimAction;//Alt release
        
        // playeranim = GameManager.Instance.
        
        slot1 = 0;
        slot2 = 0;

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
                                    Debug.Log("Addition de " + slot1 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name);
                                    int futurState = currentObjectState.stateValue + slot1;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot1 = 0;
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
                                Debug.Log("Addition de " + slot1 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name);
                                int futurState = currentObjectState.stateValue + slot1;
                                currentObjectState.CalculateNewtState(futurState);
                                slot1 = 0;
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
                                    Debug.Log("Soustraction de " + slot2 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name);
                                    int futurState = currentObjectState.stateValue + slot2;
                                    currentObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
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
                                Debug.Log("Soustraction de " + slot2 + " � " + currentObjectState.stateValue + " - Objet visé : "+ _hit.collider.gameObject.name);
                                int futurState = currentObjectState.stateValue + slot2;
                                currentObjectState.CalculateNewtState(futurState);
                                slot2 = 0;
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
            
            int tempValueSlot = slot1;

            Debug.Log($"SimSlot1 playerObjectState left: {playerObjectState.leftValue} / right: {playerObjectState.rightValue}");

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
                    
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    if (newPlayerObjectState.leftValue != 0)
                    {
                        _stateStolen.inversion = true;  // on retire value de gauche, si != 0 alors on avait un comp à droite
                    }
                    else
                    {
                        _stateStolen.inversion = false;
                    }
                    
                    slot1 = aRecuperer;
                    slot1Text.text = ((FirstState)slot1).ToString();
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
                if (playerObjectState.stateValue !=0 )//left forcément remplis
                {
                    
                    //addition mais doit verif _stateStolen.inversion
                    
                    if (_stateStolen.inversion)
                    {
                        if (playerObjectState.rightValue == 0) // 1 comportement sur player droit et rien sur sa gauche, j'additionne et check inversion
                        {
                            // addition
                            Debug.Log("SIM GAUCHE - Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                    
                            int valueToAdd = slot1;
                            int futurState = playerObjectState.stateValue + valueToAdd;

                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            // controle _stateStolen.inversion ?
                            _stateStolen.inversion = (newPlayerObjectState.leftValue < slot1);
                            
                            
                            slot1 = 0;
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
                            
                            // CAS BRAS GAUCHE (inversion)
                            //bounce3 D  immuable9 G ||| main = magnet27
                            //remplacer bounce par magnet ==> immuable9 magnet27 or on veut magnet27 immuable9 en "visuel"
                            // on doit vérifier si leftValue est < au slot ajouté

                            if (newPlayerObjectState.leftValue < tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                            }
                            else
                            {
                                _stateStolen.inversion = false;
                            }
                            
                            //slot1 = 0;
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
                        
                        // CAS BRAS GAUCHE
                        //bounce3 D  immuable9 G ||| main = magnet27
                        //remplacer bounce par magnet ==> immuable9 magnet27 or on veut magnet27 immuable9 en "visuel"
                        // on doit vérifier si leftValue est < au slot ajouté
                        
                        // verif inversion left < tempValueSlot => inversion
                        if (newPlayerObjectState.leftValue < tempValueSlot)
                        {
                            _stateStolen.inversion = true;
                        }
                        else
                        {
                            _stateStolen.inversion = false;
                        }
                        
                        
                        //slot1 = 0;
                        slot1Text.text = ((FirstState)slot1).ToString();
                        SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                        playeranim.Left_Attribution();
                        LeftArm.Feedback_Slot_Changed();
                        
                    }
                    
                }
                else // j'ai un comp en main gauche et rien sur player donc j'additionne
                {
                    Debug.Log("SIM GAUCHE - Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                    
                    int valueToAdd = slot1;
                    int futurState = playerObjectState.stateValue + valueToAdd;

                    playerObjectState.CalculateNewtState(futurState);
                    //newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    // ici on a rien sur player, on ajoute forcement à gauche et le visuel est forcement a gauche
                    _stateStolen.inversion = false;
                    
                    slot1 = 0;
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
            ComportementState newPlayerObjectState;
            playerSlotRight = playerObjectState.rightValue;
            playerSlotLeft = playerObjectState.leftValue;

            int tempValueSlot = slot2;
            
            Debug.Log($"SimSlot2 playerObjectState left: {playerObjectState.leftValue} / right: {playerObjectState.rightValue} ");
       
            if (slot2 == 0)
            {
                if (playerObjectState.stateValue != 0) // uniquement une soustraction
                {
                    int aRecuperer = 0;
                    if (_stateStolen.inversion)//leftValue est a droite
                    {
                        aRecuperer = playerObjectState.leftValue;
                    }
                    else // rightValue est a droite
                    {
                        if (playerObjectState.rightValue != 0)
                        {
                            aRecuperer = playerObjectState.rightValue;
                        }
                    }

                    if (aRecuperer == 0)// pour ne pas reset le comportement
                        return;
                    
                    //soustraction
                    
                    Debug.Log("SIM DROIT - Soustraction de " + aRecuperer + " a " + playerObjectState.stateValue + " - Objet d'origine : "+ gameManager.player.gameObject.name);
                    
                    // int valueToSteal = playerObjectState.leftValue;
                    int futurState = playerObjectState.stateValue - aRecuperer;
                    playerObjectState.CalculateNewtState(futurState);
                    
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    if (newPlayerObjectState.rightValue > tempValueSlot)
                    {
                        _stateStolen.inversion = true;
                        Debug.Log($"ICI {newPlayerObjectState.rightValue} > {tempValueSlot} donc inversion");
                    }
                    else
                    {
                        _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                        Debug.Log($"ICI {newPlayerObjectState.rightValue} <= {tempValueSlot} donc pas d'inversion");
                    } 
                    
                    slot2 = aRecuperer;
                    slot2Text.text = ((FirstState)slot2).ToString();
                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.steal);
                    playeranim.Right_Aspiration();
                    RightArm.Feedback_Slot_Changed();
                    
                }
                else //si on a rien en main et rien en slot, on a rien à faire
                {
                    Debug.Log("SIM DROIT - rien a recuperer, voler ou echanger");

                }
            }
            else
            {
                if (playerObjectState.stateValue != 0) //left forcément remplis
                {
                    // cas 1 ou 2 comportement
                    int aDonner = 0;
                    if (_stateStolen.inversion) // je suis inversé donc j'ai forcement un comportement sur bras droit
                    {
                        // echange avec leftValue 
                        int aEchanger = 0;
                        
                        // QUOI QU'IL ARRIVE C LEFT car je suis inversé donc left toujours à droite ET SI PAS INVERSION C RIGHT
                        if (playerObjectState.rightValue != 0)// si j'ai 2 comportement, je dois en echanger 1
                        {
                            //echange avec rightValue  check inversion
                            aEchanger = playerObjectState.leftValue; // left car on est inverse QUOI QU'IL ARRIVE C LEFT ET SI PAS INVERSION C RIGHT
                            
                            Debug.Log($"SIM DROIT - Echange (inversion) des valeurs : slot2 ({slot2}) ⇄ player left ({aEchanger})");
                        
                            // Debug.LogError($" avant switch \n playerObjectState.leftValue : {playerObjectState.leftValue} / rightValue: {playerObjectState.rightValue}");
                            
                            (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);
                            // slot2 = playerObjectState.rightValue;
                            // playerObjectState.rightValue = slot2;
                            
                            int futurState = playerObjectState.leftValue + playerObjectState.rightValue;
                            //Debug.Log($"futurState : {futurState}");
                            
                            slot2Text.text = ((FirstState)slot2).ToString();
                            
                            //Debug.LogError($"apres switch \n playerObjectState.leftValue : {playerObjectState.leftValue} / rightValue: {playerObjectState.rightValue}");
                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            //Debug.LogError($"newPlayerObjectState.leftValue : {newPlayerObjectState.leftValue} / rightValue: {newPlayerObjectState.rightValue}");
                            
                            // CAS BRAS DROIT
                            //bounce3 G  magnet27 D ||| main = impulse1
                            //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"
                            // on doit vérifier rightValue et slot2, rightValue < slot 2 on doit inverser 
                            
                            //si right est <= slot2 / tempValueSlot = inverse false
                            if (newPlayerObjectState.rightValue > tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                                Debug.Log($"ICI {newPlayerObjectState.rightValue} > {tempValueSlot} donc inversion");
                            }
                            else
                            {
                                _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                                Debug.Log($"ICI {newPlayerObjectState.rightValue} <= {tempValueSlot} donc pas d'inversion");
                            } 
                        }
                        else // j'ai 1 comportement et je suis inverse
                        {
                            //echange avec leftValue  et inversion = true
                            aEchanger = playerObjectState.leftValue;
                            _stateStolen.inversion = true;
                            
                            Debug.Log($"SIM DROIT - Echange (inversion) des valeurs : slot2 ({slot2}) ⇄ player leftValue ({aEchanger})");
                        
                            (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);
                            slot2Text.text = ((FirstState)slot2).ToString();
                        
                            playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                        }
                        
                        //slot2 = 0;
                        slot2Text.text = ((FirstState)slot2).ToString();
                        SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                        playeranim.Right_Attribution();
                        RightArm.Feedback_Slot_Changed();
                        
                    }
                    else
                    {
                        int aEchanger = 0;

                        if (playerObjectState.rightValue != 0) // si j'ai 2 comportement, et que je ne suis pas inversé
                        {
                            //echange avec rightValue et inversion = true
                            aEchanger = playerObjectState.rightValue;
                            Debug.Log($"SIM DROIT - Echange des valeurs : slot2 ({slot2}) ⇄ player rightValue ({aEchanger})");
                            
                            
                            (slot2, playerObjectState.rightValue) = (playerObjectState.rightValue, slot2);
                            
                            int futurState = playerObjectState.leftValue + playerObjectState.rightValue;
                            
                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            // CAS BRAS DROIT
                            //bounce3 G  magnet27 D ||| main = impulse1
                            //remplacer magnet par impulse ==> impulse1 bounce3 or on veut bounce3 impulse1 en "visuel"
                            // on doit vérifier rightValue et slot2, rightValue < slot 2 on doit inverser 
                            
                            //si right est <= slot2 / tempValueSlot = inverse false
                            if (newPlayerObjectState.rightValue > tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                                Debug.Log($"ICI {newPlayerObjectState.rightValue} > {tempValueSlot} donc inversion");
                            }
                            else
                            {
                                _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                                Debug.Log($"ICI {newPlayerObjectState.rightValue} <= {tempValueSlot} donc pas d'inversion");
                            } 
                            
                            slot2Text.text = ((FirstState)slot2).ToString();
                            SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                            playeranim.Right_Attribution();
                            RightArm.Feedback_Slot_Changed();

                        }
                        else // pas inversé et que 1 comportement
                        {
                            //addition et check si on inverse
                            Debug.Log("SIM DROIT - Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);

                            int futurState = playerObjectState.leftValue + slot2;
                            
                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            if (newPlayerObjectState.rightValue > tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                                Debug.Log($"ICI {newPlayerObjectState.rightValue} > {tempValueSlot} donc inversion");
                            }
                            else
                            {
                                _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                                Debug.Log($"ICI {newPlayerObjectState.rightValue} <= {tempValueSlot} donc pas d'inversion");
                            } 
                            
                            //reset
                            slot2 = 0;
                            slot2Text.text = "";
                            SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                            playeranim.Right_Attribution();
                            RightArm.Feedback_Slot_Changed();
                            
                        }
                    }
                    
                }
                else // je n'ai aucun comportement sur player, j'en ajoute 1 qui se place à droite
                {
                    // addition et _stateStolen.inversion = true
                    _stateStolen.inversion = true;
                    
                    Debug.Log("SIM DROIT - Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                    
                    int valueToAdd = slot2;
                    int futurState = playerObjectState.stateValue + valueToAdd;

                    playerObjectState.CalculateNewtState(futurState);
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    //_stateStolen.inversion = (newPlayerObjectState.leftValue < valueToAdd);
                    
                    slot2 = 0;
                    slot2Text.text = "";
                    SoundManager.Instance.PlaySoundPlayer(SoundManager.SoundPlayer.give);
                    playeranim.Right_Attribution();
                    RightArm.Feedback_Slot_Changed();
                    
                }
            }
            
            
        }
        
    }
    
    void SwitchSlotsValue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            (slot1, slot2) = (slot2, slot1);
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
                                    Debug.Log("Addition de " + slot1 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                                    int futurState = playerObjectState.stateValue + slot1;
                                    playerObjectState.CalculateNewtState(futurState);
                                    slot1 = 0;
                                    slot1Text.text = "";
                                    SoundManager.Instance.PlaySoundComponentPlace(gameObject);
                                    playeranim.Left_Attribution();
                                    LeftArm.Feedback_Slot_Changed(null,null, true);//player à gérer
                                }
                                else
                                {
                                    Debug.Log("Addition de " + slot2 + " et " + playerObjectState.stateValue + " - Objet visé : " + gameManager.player.gameObject.name);
                                    int futurState = playerObjectState.stateValue + slot2;
                                    playerObjectState.CalculateNewtState(futurState);
                                    slot2 = 0;
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
}
