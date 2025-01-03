using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_manager : MonoBehaviour
{
    public Animator Left_Arm_Animator;
    public Animator Right_Arm_Animator;

    void Start()
    {
        Left_Arm_Animator = GameObject.FindGameObjectWithTag("leftArm").GetComponent<Animator>();
        Right_Arm_Animator = GameObject.FindGameObjectWithTag("rightArm").GetComponent<Animator>();
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
        Left_Arm_Animator.SetBool("To_Attrib", true);
    }
    public void Right_Attribution()
    {
        Right_Arm_Animator.SetBool("To_Attrib", true);
    }
    
    #endregion
    
    #region Aspiration
    public void Left_Aspiration()
    {
        Left_Arm_Animator.SetBool("To_Aspir", true);
    }
    public void Right_Aspiration()
    {
        Right_Arm_Animator.SetBool("To_Aspir", true);
    }
    
    #endregion
}
