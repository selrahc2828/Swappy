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

    public bool simActive;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        controls = GameManager.controls;
        
        controls.Player.ActionSlot1.performed += ActionSlot1;//clic gauche
        controls.Player.ActionSlot2.performed += ActionSlot2;//clic droit
        controls.Player.SwitchSlotsValue.performed += SwitchSlotsValue;//T
        controls.Player.SIMAction.performed += SimAction;//Alt press
        controls.Player.SIMAction.canceled += SimAction;//Alt release
        
        slot1 = 0;
        slot2 = 0;

        mainCam = GameManager.Instance.mainCamera;
    }

    private void OnDisable()
    {
        controls.Player.ActionSlot1.performed -= ActionSlot1;
        controls.Player.ActionSlot2.performed -= ActionSlot2;
        controls.Player.SwitchSlotsValue.performed -= SwitchSlotsValue;
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
            if (GameManager.Instance.grabScript.isCarrying)//on ne peut pas voler/attribuer si on porte un objet
            {
                return;
            }

            if (simActive)
            {
                SimSlot1();
                return;
            }
            Slot1();
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
            Slot2();
        }
    }
    
    void Slot1()
    {
        _ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out var hit, Mathf.Infinity, hitLayer)) //mask
        {
            if (hit.collider == null || hit.collider.CompareTag("NotInteract"))
            {
                return;
            }
            var stateMachine = hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
            if (stateMachine != null)
            {
                //On verifie si slot1 est superieur a 0, s'il l'est, on cherche alors a donner un comportement a l'objet vise, sinon on cherche a prelever un comportement � l'objet vis�
                if (slot1 == 0)
                {
                    _stateStolen = stateMachine; // Stocker la r�f�rence
                    if (_stateStolen.currentState is ComportementState)
                    {
                        ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                        
                        //On v�rifie que la stateValue de l'objet vis� est superieur � 0, on ne peut pr�lever un comportement que si c'est le cas.
                        if(currentObjectState.stateValue != 0)
                        {
                            //stateValue est supperieur � 0, leftValue est donc obligatoirement remplie, etant donn� qu'il s'agit du clique gauche, on ne cherche que la leftValue
                            if (currentObjectState.leftValue != 0)//left
                            {
                                ExecuteChangeStateSubtractive(currentObjectState, false);//on vole le comportement gauche
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
                        //On verifie si l'objet vis� est vide, si c'est le cas on lui donne directement un comp�rtement avec leftValue, sinon on va v�rifier si sa rightValue est vide
                        if(currentObjectState.stateValue != 0)
                        {
                            //On v�rifie si le rightValue de l'objet Vis� est vide, si c'est le cas, on lui ajoute le comportement stoqu� dans slot1.
                            if (currentObjectState.rightValue == 0)
                            {
                                ExecuteChangeStateAdditive(currentObjectState, ref slot1);//on donne le comportement gauche
                            }
                            else
                            {
                                Debug.Log("L'objet contiens d�j� 2 comportements");
                            }
                        }
                        else
                        {
                            ExecuteChangeStateAdditive(currentObjectState, ref slot1);//on donne le comportement gauche
                        }
                    }
                }
            }
        }
    }

    void Slot2()
    {
        _ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out var hit, Mathf.Infinity, hitLayer)) //mask
        {
            if (hit.collider == null || hit.collider.CompareTag("NotInteract"))
            {
                return;
            }
            var stateMachine = hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
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
                            if (currentObjectState.rightValue != 0)//right
                            {
                                ExecuteChangeStateSubtractive(currentObjectState, true);//on prend le comportement de droite
                            }
                            else//left
                            {
                                ExecuteChangeStateSubtractive(currentObjectState, false);//on prend le comportement de gauche
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
                                ExecuteChangeStateAdditive(currentObjectState,ref slot2);//on donne le comportement de droite
                            }
                            else
                            {
                                Debug.Log("L'objet contiens d�j� 2 comportements");
                            }
                        }
                        else
                        {
                            ExecuteChangeStateAdditive(currentObjectState,ref slot2);//on donne le comportement de droite
                        }
                    }
                }
            }
        }
    }
    
    void SimSlot1() // left
    {
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference
        
        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            ComportementState newPlayerObjectState;
            
            int tempValueSlot = slot1;

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
                        aRecuperer = playerObjectState.rightValue;
                    }
                    
                    int futurState = playerObjectState.stateValue - aRecuperer;
                    playerObjectState.CalculateNewtState(futurState);
                    slot1 = aRecuperer;
                    
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //setup de la variable du nouveau state actuel
                    
                    if (newPlayerObjectState.leftValue != 0)
                    {
                        _stateStolen.inversion = true;  // on retire value de gauche, si != 0 alors on avait un comp à droite
                    }
                    else
                    {
                        _stateStolen.inversion = false;
                    }
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
                    if (_stateStolen.inversion)
                    {
                        if (playerObjectState.rightValue == 0) // 1 comportement sur player droit et rien sur sa gauche, j'additionne et check inversion
                        {
                            int valueToAdd = slot1;
                            int futurState = playerObjectState.stateValue + valueToAdd;

                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            _stateStolen.inversion = (newPlayerObjectState.leftValue < slot1);
                            slot1 = 0;
                        }
                        else // rightValue est le comportement sur la gauche du player, je dois echanger avec slot 1
                        {
                            (slot1, playerObjectState.rightValue) = (playerObjectState.rightValue, slot1);

                            playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState; 

                            if (newPlayerObjectState.leftValue < tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                            }
                            else
                            {
                                _stateStolen.inversion = false;
                            }
                        }
                    }
                    else // si j'ai 1 comp sur bras gauche, et que je ne suis pas inversé, j'echange le comp bras gauche et le slot1
                    {
                        (slot1, playerObjectState.leftValue) = (playerObjectState.leftValue, slot1);
                        
                        playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
                        newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                        
                        // verif inversion left < tempValueSlot => inversion
                        if (newPlayerObjectState.leftValue < tempValueSlot)
                        {
                            _stateStolen.inversion = true;
                        }
                        else
                        {
                            _stateStolen.inversion = false;
                        }
                    }
                }
                else // j'ai un comp en main gauche et rien sur player donc j'additionne
                {
                    int valueToAdd = slot1;
                    int futurState = playerObjectState.stateValue + valueToAdd;

                    playerObjectState.CalculateNewtState(futurState);
                    
                    // ici on a rien sur player, on ajoute forcement à gauche et le visuel est forcement a gauche
                    _stateStolen.inversion = false;
                    
                    slot1 = 0;
                } 
            }
        }
    }

    void SimSlot2()//right
    {
        _stateStolen = gameManager.player.GetComponent<ComportementsStateMachine>(); // Stocker la reference
        
        if (_stateStolen.currentState is ComportementState)
        {
            ComportementState playerObjectState = (ComportementState)_stateStolen.currentState;
            ComportementState newPlayerObjectState;

            int tempValueSlot = slot2;
       
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
                    
                    // int valueToSteal = playerObjectState.leftValue;
                    int futurState = playerObjectState.stateValue - aRecuperer;
                    playerObjectState.CalculateNewtState(futurState);
                    
                    newPlayerObjectState = (ComportementState)_stateStolen.currentState; //reset du state actuel
                    
                    if (newPlayerObjectState.rightValue > tempValueSlot)
                    {
                        _stateStolen.inversion = true;
                    }
                    else
                    {
                        _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                    } 
                    
                    slot2 = aRecuperer;
                    
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
                    if (_stateStolen.inversion) // je suis inversé donc j'ai forcement un comportement sur bras droit
                    {
                        // QUOI QU'IL ARRIVE C LEFT car je suis inversé donc left toujours à droite ET SI PAS INVERSION C RIGHT
                        if (playerObjectState.rightValue != 0)// si j'ai 2 comportement, je dois en echanger 1
                        {
                            (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);
                            
                            int futurState = playerObjectState.leftValue + playerObjectState.rightValue;
                            
                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            //si right est <= slot2 / tempValueSlot = inverse false
                            if (newPlayerObjectState.rightValue > tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                            }
                            else
                            {
                                _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                            } 
                        }
                        else // j'ai 1 comportement et je suis inverse
                        {
                            //echange avec leftValue  et inversion = true
                            _stateStolen.inversion = true;
                        
                            (slot2, playerObjectState.leftValue) = (playerObjectState.leftValue, slot2);
                        
                            playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
                        }
                    }
                    else
                    {
                        if (playerObjectState.rightValue != 0) // si j'ai 2 comportement, et que je ne suis pas inversé
                        {
                            (slot2, playerObjectState.rightValue) = (playerObjectState.rightValue, slot2);
                            
                            int futurState = playerObjectState.leftValue + playerObjectState.rightValue;
                            
                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            //si right est <= slot2 / tempValueSlot = inverse false
                            if (newPlayerObjectState.rightValue > tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                            }
                            else
                            {
                                _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                            } 
                        }
                        else // pas inversé et que 1 comportement
                        {
                            int futurState = playerObjectState.leftValue + slot2;
                            
                            playerObjectState.CalculateNewtState(futurState);
                            newPlayerObjectState = (ComportementState)_stateStolen.currentState;
                            
                            if (newPlayerObjectState.rightValue > tempValueSlot)
                            {
                                _stateStolen.inversion = true;
                            }
                            else
                            {
                                _stateStolen.inversion = false; // slot est == ou supérieur, donc l'orde du systeme est bon 
                            } 
                            //reset
                            slot2 = 0;
                        }
                    }
                }
                else // je n'ai aucun comportement sur player, j'en ajoute 1 qui se place à droite
                {
                    _stateStolen.inversion = true;
                    
                    int valueToAdd = slot2;
                    int futurState = playerObjectState.stateValue + valueToAdd;

                    playerObjectState.CalculateNewtState(futurState);
                    
                    slot2 = 0;
                }
            }
        }
    }
    
    void SwitchSlotsValue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            (slot1, slot2) = (slot2, slot1);
        }
    }

    void ExecuteChangeStateAdditive(ComportementState currentObjectState,ref int addedSlotValue)
    {
        currentObjectState.CalculateNewtState(currentObjectState.stateValue + addedSlotValue);
        addedSlotValue = 0;
    }
    
    void ExecuteChangeStateSubtractive(ComportementState currentObjectState, bool right)
    {
        if (right)
        {
            currentObjectState.CalculateNewtState(currentObjectState.stateValue - currentObjectState.rightValue);
            slot2 = currentObjectState.rightValue;
        }
        else
        {
            currentObjectState.CalculateNewtState(currentObjectState.stateValue - currentObjectState.leftValue);
            slot2 = currentObjectState.leftValue;
        }
    }
}
