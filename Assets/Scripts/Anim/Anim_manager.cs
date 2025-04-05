using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_manager : MonoBehaviour
{
    public Animator Left_Arm_Animator;
    public Animator Right_Arm_Animator;
    
    private Transform Start_Local_Left_Transform;
    
    

    void Start()
    {
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
    public void Left_Attribution()
    {
        Left_Arm_Animator.Play("AtributionLeft_");
        
       // Left_Arm_Animator.SetBool("To_Attrib", true);
        
    }
    public void Right_Attribution()
    {
        Right_Arm_Animator.Play("Atribution");
        
       // Right_Arm_Animator.SetBool("To_Attrib", true);
        
    }
    
    #endregion
    
    #region Aspiration
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
    
    #endregion

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

    public void setAspirFalse_Right()
    {
        
        Right_Arm_Animator.SetBool("To_Aspir", false);
    }
    public void setAspirFalse_Left()
    {
        Left_Arm_Animator.gameObject.transform.localPosition = Start_Local_Left_Transform.position;
        Left_Arm_Animator.SetBool("To_Aspir", false);
    }
}
