using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_manager : MonoBehaviour
{
    public Animator Left_Arm_Animator;
    public Animator Right_Arm_Animator;
    
    private Transform Start_Local_Left_Transform;
    
    

    private void OnDisable()
    {
        GlobalEventManager.Instance.OnComportmentExtracted -= HandleComportmentExtraction;
        GlobalEventManager.Instance.OnComportmentAdded -= HandleComportmentAttribution;
        GlobalEventManager.Instance.OnSelfImpactMod -= HandleSelfImpactMod;
    }

    void Start()
    {
        GlobalEventManager.Instance.OnComportmentExtracted += HandleComportmentExtraction;
        GlobalEventManager.Instance.OnComportmentAdded += HandleComportmentAttribution;
        GlobalEventManager.Instance.OnSelfImpactMod += HandleSelfImpactMod;
        
        Left_Arm_Animator = GameObject.FindGameObjectWithTag("leftArm").GetComponent<Animator>();
        Right_Arm_Animator = GameObject.FindGameObjectWithTag("rightArm").GetComponent<Animator>();
        Start_Local_Left_Transform = Left_Arm_Animator.transform;
    }

    

    #region IDLE 
    //Pour l'instant inutil mais au cas ou pour plus tard
    public void Left_Idle()
    {
        
    }
    public void Right_Idle()
    {
        
    }

    #endregion
    
    #region Attribution

    public void HandleComportmentAttribution(GameObject target, bool rightValue, bool righthand)
    {
        if (!target.CompareTag("Player"))
        {
            if (righthand)
            {
                Right_Attribution();
            }
            else
            {
                Left_Attribution();
            }
        }
        else
        {
            if (righthand)
            {
                Right_SelfAssign();
            }
            else
            {
                Left_SelfAssign();
            }
        }
    }
    
    public void Left_Attribution()
    {
        Left_Arm_Animator.Play("Attribution_Left");
        
       
        
    }
    public void Right_Attribution()
    {
        Right_Arm_Animator.Play("Attribution");
        
       
        
    }
    
    #region SelfAssign

    public void Left_SelfAssign()
    {
        Left_Arm_Animator.Play("Self_Assign_Left");
    }
    public void Right_SelfAssign()
    {
        Left_Arm_Animator.Play("Self_Assign");
    }

    

    #endregion
    
    #endregion
    
    #region Aspiration

    public void HandleComportmentExtraction(GameObject target, bool rightValue, bool rightHand)
    {
        if (rightHand)
        {
            if (!target.CompareTag("Player"))
            {
                Right_SelfAspiration();
            }
            else
            {
                Right_Aspiration();
            }
        }
        else
        {
            if (!target.CompareTag("Player"))
            {
                Left_SelfAspiration();
            }
            else
            {
                Left_Aspiration();
            }
        }
        
        if (!target.CompareTag("Player"))
        {
            if (rightHand)
            {
                Debug.Log("droite");
                Right_Aspiration();
            }
            else
            {
                Debug.Log("gauche");
                Left_Aspiration();
            }
        }
        else
        {
            if (rightHand)
            {
                Right_SelfAspiration();
            }
            else
            {
                Left_SelfAspiration();
            }
        }
    }
    
    public void Left_Aspiration()
    {
        
        Left_Arm_Animator.Play("Aspiration_Left");
        
       
        
    }
    public void Right_Aspiration()
    {
        Right_Arm_Animator.Play("Aspiration");
        
       
        
    }
    
    #region SelfAspiration

    public void Right_SelfAspiration()
    {
        
        Right_Arm_Animator.Play("SelfSteal");
    }
    public void Left_SelfAspiration()
    {
        
        Left_Arm_Animator.Play("SelfSteal_Left");
    }
    
    #endregion
    
    #endregion

    
    #region SelfImpactMod

    public void HandleSelfImpactMod(GameObject _gameObject,bool active)
    {
        if (active)
        {
            SelfImpactModIN();
        }
        else
        {
            SelfImpactModOUT();
        }
    }

    public void SelfImpactModIN()
    {
        Right_Arm_Animator.Play("SelfImpactMod_IN");
        Left_Arm_Animator.Play("SelfImpactMod_IN_Left");
    }

    public void SelfImpactModOUT()
    {
        Right_Arm_Animator.Play("SelfImpactMod_OUT");
        Left_Arm_Animator.Play("SelfImpactMod_OUT_Left");
    }
    
    #endregion
}
