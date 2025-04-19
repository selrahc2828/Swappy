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
    }

    void Start()
    {
        GlobalEventManager.Instance.OnComportmentExtracted += HandleComportmentExtraction;
        GlobalEventManager.Instance.OnComportmentAdded += HandleComportmentAttribution;
        
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

    public void HandleComportmentAttribution(GameObject target, bool righthand)
    {
        if (target.tag != "Player")
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
        Left_Arm_Animator.Play("AtributionLeft_");
        
       
        
    }
    public void Right_Attribution()
    {
        Right_Arm_Animator.Play("Atribution");
        
       
        
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

    public void HandleComportmentExtraction(GameObject target, bool righthand)
    {
        if (target.tag != "Player")
        {
            if (righthand)
            {
                Right_Aspiration();
            }
            else
            {
                Left_Aspiration();
            }
        }
        else
        {
            if (righthand)
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
        
        Left_Arm_Animator.Play("AspirationLeft");
        
       // Left_Arm_Animator.SetBool("To_Aspir", true);
        
    }
    public void Right_Aspiration()
    {
        Right_Arm_Animator.Play("Aspiration");
        
       // Right_Arm_Animator.SetBool("To_Aspir", true);
        
    }
    
    #region SelfAspiration

    public void Right_SelfAspiration()
    {
        
        Right_Arm_Animator.SetBool("To_Aspir", false);
    }
    public void Left_SelfAspiration()
    {
        Left_Arm_Animator.gameObject.transform.localPosition = Start_Local_Left_Transform.position;
        Left_Arm_Animator.SetBool("To_Aspir", false);
    }
    
    #endregion
    
    #endregion

    
}
