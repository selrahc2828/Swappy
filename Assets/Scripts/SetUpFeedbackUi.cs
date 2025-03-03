using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpFeedbackUi : MonoBehaviour
{
    public LayerMask hitLayer;
    private Ray _ray;
    
    [Header("Player")]
    public GameObject vignetteParent;
    public Image vignetteLeft;
    public Image vignetteRight;
    
    [Header("Objet")]
    public GameObject knobIndicationParent;
    public Image knobIndicationLeft;
    public Image knobIndicationRight;
        
    // Start is called before the first frame update
    void Start()
    {
        ParentIndicationActive();
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

        _ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            if (_hit.collider == null || _hit.collider.CompareTag("NotInteract"))
            {
                ParentIndicationActive();
                return;
            }
            
            var stateMachine = _hit.collider.gameObject.GetComponent<ComportementsStateMachine>();
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
                    
                    if (currentObjectState.rightValue == 0)
                    {
                        if (stealerProto.slot1 != 0)
                        {
                            ColorFeedback(knobIndicationLeft, currentObjectState.rightValue);
                            ColorFeedback(knobIndicationRight, currentObjectState.leftValue);
                        }
                        else
                        {
                            ColorFeedback(knobIndicationLeft, currentObjectState.leftValue);
                            ColorFeedback(knobIndicationRight, currentObjectState.rightValue);
                            
                        }
                    }
                    else
                    {
                        ColorFeedback(knobIndicationLeft, currentObjectState.leftValue);
                        ColorFeedback(knobIndicationRight, currentObjectState.rightValue);  
                    }
                    
                    // objet right est vide
                    // objet left a quelque chose
                    // main gauche a quelque chose
                    // main droite est vide
                    
                }
                else
                {
                    ParentIndicationActive();
                }
            }
            else
            {
                ParentIndicationActive();
            }
        }
        else
        {
            ParentIndicationActive();
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
            ColorFeedback(vignetteLeft, currentPlayertate.leftValue);
            ColorFeedback(vignetteRight, currentPlayertate.rightValue);
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
}
