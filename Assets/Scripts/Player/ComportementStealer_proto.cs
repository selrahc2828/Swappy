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
        GlobalEventManager.Instance.SelfImpactMod(simActive);
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
                //On verifie si slot1 est superieur a 0, s'il l'est, on cherche alors a donner un comportement a l'objet vise, sinon on cherche a prelever un comportement a l'objet vise
                if (slot1 == 0)
                {
                    _stateStolen = stateMachine; // Stocker la reference
                    if (_stateStolen.currentState is ComportementState)
                    {
                        ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                        
                        //On verifie que la stateValue de l'objet vise est superieur a 0, on ne peut prelever un comportement que si c'est le cas.
                        if(currentObjectState.stateValue != 0)
                        {
                            if (_stateStolen.inversion)
                            {
                                //stateValue est supperieur a 0, leftValue est donc obligatoirement remplie, etant donn� qu'il s'agit du clique gauche, on ne cherche que la leftValue
                                if (currentObjectState.rightValue != 0)//left
                                {
                                    ExecuteChangeStateSubtractive(currentObjectState, ref slot1, true, false); //on vole le comportement de gauche de l'objet vise
                                }
                                else
                                {
                                    ExecuteChangeStateSubtractive(currentObjectState, ref slot1, false, false);
                                }
                            }
                            else
                            {
                                ExecuteChangeStateSubtractive(currentObjectState, ref slot1, false, false);
                            }
                        }
                        else
                        {
                            Debug.Log("L'objet ne contiens aucun comportement a prelever");
                        }
                    }
                }
                else
                {
                    _stateStolen = stateMachine; // Stocker la reference
                    if (_stateStolen.currentState is ComportementState)
                    {
                        ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                        //On verifie si l'objet vise est vide, si c'est le cas on lui donne directement un comp�rtement avec leftValue, sinon on va v�rifier si sa rightValue est vide
                        if(currentObjectState.stateValue != 0)
                        {
                            //On verifie si le rightValue de l'objet Vise est vide, si c'est le cas, on lui ajoute le comportement stoque dans slot1.
                            if (currentObjectState.rightValue == 0)
                            {
                                ExecuteChangeStateAdditive(currentObjectState, ref slot1, false); //on donne le comportement de la main gauche a l'objet vise
                            }
                            else
                            {
                                Debug.Log("L'objet contiens deja 2 comportements");
                            }
                        }
                        else
                        {
                            ExecuteChangeStateAdditive(currentObjectState, ref slot1, false); //on donne le comportement de la main gauche a l'objet vise
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
                //On verifie si slot2 est superieur a 0, s'il l'est, on cherche alors a donner un comportement a l'objet vise, sinon on cherche � pr�lever un comportement a l'objet vise
                if (slot2 == 0)
                {
                    _stateStolen = stateMachine; // Stocker la r�f�rence
                    if (_stateStolen.currentState is ComportementState)
                    {
                        ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                        //On verifie si l'objet vise contiens une stateValue, si la stateValue est superieur a 0, l'objet a un comportement
                        if (currentObjectState.stateValue != 0) // l'objet a deja au moin un comportement
                        {
                            //L'objet vise a une stateValue superieur a 0 donc sa leftValue est forc�ment remplis, on test dans un premier temps la rightValue etant donne que c'ets le click droit.
                            //Si la rightValue est superieur a 0, on la stoque, sinon on stoque la leftValue.
                            if (currentObjectState.rightValue != 0)//right
                            {
                                if (_stateStolen.inversion)
                                {
                                    ExecuteChangeStateSubtractive(currentObjectState, ref slot2, false, true); //on prend le comportement de droite de l'objet vise
                                }
                                else
                                {
                                    ExecuteChangeStateSubtractive(currentObjectState, ref slot2, true, true);
                                }
                            }
                            else//left
                            {
                                ExecuteChangeStateSubtractive(currentObjectState, ref slot2, false, true); //on prend le comportement de gauche de l'objet vise
                            }
                        }
                        else
                        {
                            Debug.Log("L'objet ne contiens aucun comportement a prelever");
                        }
                    }
                }
                else
                {
                    _stateStolen = stateMachine; // Stocker la reference
                    if (_stateStolen.currentState is ComportementState)
                    {
                        ComportementState currentObjectState = (ComportementState)_stateStolen.currentState;
                        //On test si l'objet vise est vide ou non, s'il est vide, on lui ajoute directement le comportement, sinon on verifie s'il a une place libre
                        if(currentObjectState.stateValue != 0)
                        {
                            //L'objet vise a une stateValue superieur a 0 donc sa leftValue est forc�ment remplis, on ne test que la rightValue, si elle a une valeur de 0 on lui ajoute le comportement stoqu�
                            if (currentObjectState.rightValue == 0)
                            {
                                ExecuteChangeStateAdditive(currentObjectState,ref slot2, true); //on donne le comportement de la main droite du joueur a l'objet vise
                            }
                            else
                            {
                                Debug.Log("L'objet contiens deja 2 comportements");
                            }
                        }
                        else
                        {
                            ExecuteChangeStateAdditive(currentObjectState,ref slot2, true); //on donne le comportement de la main droite du joueur a l'objet vise
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
            
            int originValueSlot1 = slot1;

            if (slot1 == 0) // il n y a pas de comportement dans main gauche
            {
                if (playerObjectState.stateValue !=0) // le joueur a au moins 1 comportement sur lui  --> la left Value est forcement positive
                {

                    if (_stateStolen.inversion)
                    {
                        if (playerObjectState.rightValue == 0)
                        {
                            return;
                        }
                        ExecuteChangeStateSubtractive(playerObjectState, ref slot1, true, true);
                        _stateStolen.inversion = false;
                    }
                    else
                    {
                        ExecuteChangeStateSubtractive(playerObjectState, ref slot1, false, false);
                        _stateStolen.inversion = playerObjectState.rightValue != 0;// on viens de prendre le comportement gauche du joueur en mode non inverse
                                                                                   // soit la valeur du comportement de droite est positive (le joueur avais un double comportement) et l'inversion est obligatoire
                                                                                   // soit la valeur du comportement de droite est nulle et l'inversion ne se fais pas
                    }
                }
                else // le joueur n'a aucun comportement sur lui
                {
                    Debug.Log("SIM GAUCHE - rien a recuperer, voler ou echanger");
                }
            }
            else //le joueur a un comportement sur sa main gauche
            {
                if (playerObjectState.stateValue !=0 ) // le joueur a au moins 1 comportement sur lui --> la left Value est forcement positive
                {
                    if (_stateStolen.inversion) // le comportement sur le joueur est inversé (la gauche est devenu la droite)
                    {
                        ExchangeStateWithPlayerAndHand(playerObjectState, ref slot1, true);
                        _stateStolen.inversion = (originValueSlot1 <= playerObjectState.leftValue); //Le seul moment ou on change inversion c'est quand il y a une inversion naturelle.
                                                                                                    //une inversion naterelle ne se produit avec le slot1 que lorsque la valeur du slot1 est inferieur ou égale a left value avant l'echange
                    }
                    else // le comportement sur le joueur n'est pas inversé
                    {
                        ExchangeStateWithPlayerAndHand(playerObjectState, ref slot1, false);
                        _stateStolen.inversion = (originValueSlot1 < playerObjectState.rightValue && playerObjectState.rightValue != 0);//Le seul moment ou on change inversion c'est quand il y a une inversion naturelle.
                                                                                                                                        //une inversion naterelle ne se produit avec le slot1 que lorsque la valeur du slot1 est inferieur right value avant l'echange
                                                                                                                                        //tant que right value est différent de 0
                    }
                }
                else // le joueur n'a aucun comportement sur lui
                {
                    ExchangeStateWithPlayerAndHand(playerObjectState, ref slot1, true);
                    _stateStolen.inversion = false;
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

            int originValueSlot2 = slot2;
       
            if (slot2 == 0) // si on a rien dans la main droite
            {
                if (playerObjectState.stateValue != 0) // le joueur a au moins 1 comportement sur lui --> la left Value est forcement positive
                {
                    if(_stateStolen.inversion)
                    {
                        if(playerObjectState.leftValue == 0)
                        {
                            return;
                        }
                        ExecuteChangeStateSubtractive(playerObjectState, ref slot2, false, false);
                    }
                    else
                    {
                        if (playerObjectState.rightValue == 0)
                        {
                            return;
                        }
                        ExecuteChangeStateSubtractive(playerObjectState, ref slot2, true,true);
                    }          
                    _stateStolen.inversion = false;                    
                }
                else //si on a rien en main et rien en slot, on a rien à faire
                {
                    Debug.Log("SIM DROIT - rien a recuperer, voler ou echanger");
                }
            }
            else // si on a un comportement dans la main droite
            {
                if (playerObjectState.stateValue != 0) // le joueur a au moins 1 comportement sur lui  --> la left Value est forcement positive
                {
                    if (_stateStolen.inversion) // le comportement sur le joueur est inversé (la gauche est devenu la droite)
                    {
                        ExchangeStateWithPlayerAndHand(playerObjectState, ref slot2, false);

                        if (originValueSlot2 != 0 && originValueSlot2 >= playerObjectState.rightValue)//Le seul moment ou on change inversion c'est quand il y a une inversion naturelle.
                                                                                                     // une inversion naterelle ne se produit avec le slot2 que lorsque la valeur du slot2 est superieur ou égale a right value avant l'echange
                                                                                                     // tant que right value est differente de 0
                        {
                            _stateStolen.inversion = !_stateStolen.inversion;
                        }
                    }
                    else // le comportement sur le joueur n'est pas inverse
                    {
                        ExchangeStateWithPlayerAndHand(playerObjectState, ref slot2, true);

                        if (originValueSlot2 < playerObjectState.leftValue) //Le seul moment ou on change inversion c'est quand il y a une inversion naturelle.
                                                                            //une inversion naterelle ne se produit avec le slot2 que lorsque la valeur du slot2 est inferieur a left value avant l'echange
                        {
                            _stateStolen.inversion = !_stateStolen.inversion;
                        }
                    }
                }
                else // le joueur n'a aucun comportement sur lui
                {
                    ExchangeStateWithPlayerAndHand(playerObjectState, ref slot2, true);

                    _stateStolen.inversion = true;
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
    void ExchangeStateWithPlayerAndHand(ComportementState playerObjectState, ref int exchangedSlotValue, bool right)
    {
        if(right)
        {
            (exchangedSlotValue, playerObjectState.rightValue) = (playerObjectState.rightValue, exchangedSlotValue);
        }
        else
        {
            (exchangedSlotValue, playerObjectState.leftValue) = (playerObjectState.leftValue, exchangedSlotValue);
        }
        playerObjectState.CalculateNewtState(playerObjectState.leftValue + playerObjectState.rightValue);
        GlobalEventManager.Instance.ComportmentExchanged(right);
    }

    void ExecuteChangeStateAdditive(ComportementState currentObjectState,ref int addedSlotValue, bool right)
    {
        currentObjectState.CalculateNewtState(currentObjectState.stateValue + addedSlotValue);
        addedSlotValue = 0;
        GlobalEventManager.Instance.ComportmentAdded(currentObjectState.GetGameObject(), right);
    }
    
    void ExecuteChangeStateSubtractive(ComportementState currentObjectState, ref int substractedSlotValue, bool rightValue, bool rightHand)
    {
        if (rightValue)
        {
            currentObjectState.CalculateNewtState(currentObjectState.stateValue - currentObjectState.rightValue);
            substractedSlotValue = currentObjectState.rightValue;
        }
        else
        {
            currentObjectState.CalculateNewtState(currentObjectState.stateValue - currentObjectState.leftValue);
            substractedSlotValue = currentObjectState.leftValue;
        }
        GlobalEventManager.Instance.ComportmentExtracted(currentObjectState.GetGameObject(), rightValue, rightHand);
    }
}