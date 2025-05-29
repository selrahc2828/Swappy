using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpFeedbackUi : MonoBehaviour
{
    public LayerMask hitLayer;
    private Ray _ray;
    private AimAssist _aimAssist;
    
    [Header("Player")]
    public GameObject vignetteParent;
    public Image vignetteLeft;
    public Image vignetteRight;
    
    [Header("Objet")]
    public GameObject knobIndicationParent;
    public GameObject[] feedbackIndicationLeft;
    public GameObject[] feedbackIndicationRight;
    public ComportementStealer_proto playerSlots;
        
    // Start is called before the first frame update
    void Start()
    {
        _aimAssist = GameManager.Instance.player.GetComponent<AimAssist>();
        ParentIndicationActive();
        playerSlots = GameManager.Instance.player.GetComponent<ComportementStealer_proto>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckComp();

        CheckCompOnPlayer();
    }

    public void CheckComp()
    {
        RaycastHit _hit;

        // if (Physics.Raycast(_ray, out var hit, Mathf.Infinity, hitLayer)) //mask
        if (_aimAssist.cible is not null)
        {
            var stateMachine = _aimAssist.cible; // hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
            if (stateMachine != null)
            {
                if (stateMachine.currentState is ComportementState)
                {
                    ParentIndicationActive(true);
                    ComportementState currentObjectState = (ComportementState)stateMachine.currentState;
                    
                    ComportementStealer_proto stealerProto = GameManager.Instance.player.GetComponent<ComportementStealer_proto>();
                    if (stealerProto == null)
                    {
                        return;
                    }

                    SymboleFeedback(currentObjectState.leftValue, currentObjectState.rightValue);                    
                }
                else
                {
                    SymboleFeedback(0, 0);
                }
            }
            else
            {
                SymboleFeedback(0, 0);
            }
        }
        else
        {
            SymboleFeedback(0, 0);
        }  
    }

    public void CheckCompOnPlayer()
    {
        var stateMachine = GameManager.Instance?.player.gameObject.GetComponent<ComportementsStateMachine>();
        if (stateMachine == null)
        {
            return;
        }

        if (stateMachine.currentState is ComportementState)
        {
            ComportementState currentPlayertate = (ComportementState)stateMachine.currentState;
            // check currentPlayertate.inversion
            if (currentPlayertate.smGet.inversion)
            {
                ColorFeedback(vignetteLeft, currentPlayertate.rightValue);
                ColorFeedback(vignetteRight, currentPlayertate.leftValue);
            }
            else
            {
                ColorFeedback(vignetteLeft, currentPlayertate.leftValue);
                ColorFeedback(vignetteRight, currentPlayertate.rightValue);              
            }

        }
    }
    
    public void ParentIndicationActive(bool isActive = false)
    {
        knobIndicationParent.SetActive(isActive);
    }
    
    public void ChangeVignetLeftColor(float comportementValue)
    {
        ColorFeedback(vignetteLeft, comportementValue);
    }
    
    public void ChangeVignetteRightColor(float comportementValue)
    {
        ColorFeedback(vignetteRight, comportementValue);
    }
    
    public void ColorFeedback(Image i, float value)
    {
        switch (value)
        {
            case 0 : // NoComportement
                i.color = ComportementManager.Instance.noComportementColor;
                break;
            case 1 : // SoloImpulse
                i.color = ComportementManager.Instance.impulseColor;
                break;
            case 3 : // SoloBouncing
                i.color = ComportementManager.Instance.bouncingColor;
                break;
            case 9 : // SoloImmuable
                i.color = ComportementManager.Instance.immuableColor;
                break;
            case 27 : // SoloMagnet
                i.color = ComportementManager.Instance.magnetColor;
                break;
            case 81 : // SoloRocket
                i.color = ComportementManager.Instance.rocketColor;
                break;
            default:
                break;
        }
    }

    public void SymboleFeedback(int leftValue, int rightValue)
    {
        if(rightValue == 0 && leftValue != 0) // si le cube a un comportement seulement a gauche
        {
            if(playerSlots.slot1 != 0 && playerSlots.slot2 == 0) // si seule la main gauche a un comportement stoqué
            {
                switch (leftValue)
                {
                    case 0: // NoComportement
                        feedbackIndicationRight[0].SetActive(false);
                        feedbackIndicationRight[1].SetActive(false);
                        feedbackIndicationRight[2].SetActive(false);
                        feedbackIndicationRight[3].SetActive(false);
                        feedbackIndicationRight[4].SetActive(false);

                        break;
                    case 1: // SoloImpulse
                        feedbackIndicationRight[0].SetActive(true);
                        feedbackIndicationRight[1].SetActive(false);
                        feedbackIndicationRight[2].SetActive(false);
                        feedbackIndicationRight[3].SetActive(false);
                        feedbackIndicationRight[4].SetActive(false);

                        break;
                    case 3: // SoloBouncing
                        feedbackIndicationRight[0].SetActive(false);
                        feedbackIndicationRight[1].SetActive(true);
                        feedbackIndicationRight[2].SetActive(false);
                        feedbackIndicationRight[3].SetActive(false);
                        feedbackIndicationRight[4].SetActive(false);

                        break;
                    case 9: // SoloImmuable
                        feedbackIndicationRight[0].SetActive(false);
                        feedbackIndicationRight[1].SetActive(false);
                        feedbackIndicationRight[2].SetActive(true);
                        feedbackIndicationRight[3].SetActive(false);
                        feedbackIndicationRight[4].SetActive(false);

                        break;
                    case 27: // SoloMagnet
                        feedbackIndicationRight[0].SetActive(false);
                        feedbackIndicationRight[1].SetActive(false);
                        feedbackIndicationRight[2].SetActive(false);
                        feedbackIndicationRight[3].SetActive(true);
                        feedbackIndicationRight[4].SetActive(false);

                        break;
                    case 81: // SoloRocket
                        feedbackIndicationRight[0].SetActive(false);
                        feedbackIndicationRight[1].SetActive(false);
                        feedbackIndicationRight[2].SetActive(false);
                        feedbackIndicationRight[3].SetActive(false);
                        feedbackIndicationRight[4].SetActive(true);

                        break;
                    default:
                        break;
                }
                feedbackIndicationLeft[0].SetActive(false);
                feedbackIndicationLeft[1].SetActive(false);
                feedbackIndicationLeft[2].SetActive(false);
                feedbackIndicationLeft[3].SetActive(false);
                feedbackIndicationLeft[4].SetActive(false);
                return;
            }
        }
        switch(leftValue)
        {
            case 0: // NoComportement
                feedbackIndicationLeft[0].SetActive(false);
                feedbackIndicationLeft[1].SetActive(false);
                feedbackIndicationLeft[2].SetActive(false);
                feedbackIndicationLeft[3].SetActive(false);
                feedbackIndicationLeft[4].SetActive(false);

                break;
            case 1: // SoloImpulse
                feedbackIndicationLeft[0].SetActive(true);
                feedbackIndicationLeft[1].SetActive(false);
                feedbackIndicationLeft[2].SetActive(false);
                feedbackIndicationLeft[3].SetActive(false);
                feedbackIndicationLeft[4].SetActive(false);

                break;
            case 3: // SoloBouncing
                feedbackIndicationLeft[0].SetActive(false);
                feedbackIndicationLeft[1].SetActive(true);
                feedbackIndicationLeft[2].SetActive(false);
                feedbackIndicationLeft[3].SetActive(false);
                feedbackIndicationLeft[4].SetActive(false);

                break;
            case 9: // SoloImmuable
                feedbackIndicationLeft[0].SetActive(false);
                feedbackIndicationLeft[1].SetActive(false);
                feedbackIndicationLeft[2].SetActive(true);
                feedbackIndicationLeft[3].SetActive(false);
                feedbackIndicationLeft[4].SetActive(false);

                break;
            case 27: // SoloMagnet
                feedbackIndicationLeft[0].SetActive(false);
                feedbackIndicationLeft[1].SetActive(false);
                feedbackIndicationLeft[2].SetActive(false);
                feedbackIndicationLeft[3].SetActive(true);
                feedbackIndicationLeft[4].SetActive(false);

                break;
            case 81: // SoloRocket
                feedbackIndicationLeft[0].SetActive(false);
                feedbackIndicationLeft[1].SetActive(false);
                feedbackIndicationLeft[2].SetActive(false);
                feedbackIndicationLeft[3].SetActive(false);
                feedbackIndicationLeft[4].SetActive(true);

                break;
            default:
                break;
        }

        switch (rightValue)
        {
            case 0: // NoComportement
                feedbackIndicationRight[0].SetActive(false);
                feedbackIndicationRight[1].SetActive(false);
                feedbackIndicationRight[2].SetActive(false);
                feedbackIndicationRight[3].SetActive(false);
                feedbackIndicationRight[4].SetActive(false);

                break;
            case 1: // SoloImpulse
                feedbackIndicationRight[0].SetActive(true);
                feedbackIndicationRight[1].SetActive(false);
                feedbackIndicationRight[2].SetActive(false);
                feedbackIndicationRight[3].SetActive(false);
                feedbackIndicationRight[4].SetActive(false);

                break;
            case 3: // SoloBouncing
                feedbackIndicationRight[0].SetActive(false);
                feedbackIndicationRight[1].SetActive(true);
                feedbackIndicationRight[2].SetActive(false);
                feedbackIndicationRight[3].SetActive(false);
                feedbackIndicationRight[4].SetActive(false);

                break;
            case 9: // SoloImmuable
                feedbackIndicationRight[0].SetActive(false);
                feedbackIndicationRight[1].SetActive(false);
                feedbackIndicationRight[2].SetActive(true);
                feedbackIndicationRight[3].SetActive(false);
                feedbackIndicationRight[4].SetActive(false);

                break;
            case 27: // SoloMagnet
                feedbackIndicationRight[0].SetActive(false);
                feedbackIndicationRight[1].SetActive(false);
                feedbackIndicationRight[2].SetActive(false);
                feedbackIndicationRight[3].SetActive(true);
                feedbackIndicationRight[4].SetActive(false);

                break;
            case 81: // SoloRocket
                feedbackIndicationRight[0].SetActive(false);
                feedbackIndicationRight[1].SetActive(false);
                feedbackIndicationRight[2].SetActive(false);
                feedbackIndicationRight[3].SetActive(false);
                feedbackIndicationRight[4].SetActive(true);

                break;
            default:
                break;
        }
    }
}
