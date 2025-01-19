using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_feedback : MonoBehaviour
{

    public bool left_Arm = false;

    private ComportementManager comportementManager;
    public ComportementStealer_proto comp_steler_proto;
    private GameObject feedback_Act;

    private void Start()
    {
        comportementManager = ComportementManager.Instance;
    }

    // Update is called once per frame
    public void Feedback_Slot_Changed()
    {
        if (left_Arm)
        {
            switch (comp_steler_proto.slot1)
            {
                case 0:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                        
                    }
                    break;
                case 1:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Impulse, transform);
                    }
                    
                    break;
                case 3:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Bouncing, transform);
                    }
                   
                    break;
                case 9:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Immuable, transform);
                    }
                    
                    break;
                case 27:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Magnet, transform);
                    }
                    
                    break;
                case 81:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Rocket, transform);
                    }
                    
                    break;
            }
            
            
        }
        else
        {
            switch (comp_steler_proto.slot2)
            {
                case 0:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                        
                    }
                    break;
                case 1:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Impulse, transform);
                    }
                    
                    break;
                case 3:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Bouncing, transform);
                    }
                   
                    break;
                case 9:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Immuable, transform);
                    }
                    
                    break;
                case 27:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Magnet, transform);
                    }
                    
                    break;
                case 81:
                    if (feedback_Act != null)
                    {
                        Destroy(feedback_Act);
                    }
                    if (feedback_Act == null)
                    {
                        feedback_Act = Instantiate(comportementManager.hand_Slot_Rocket, transform);
                    }
                    
                    break;
            }
        }
    }
}
