using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSoundPLayer : MonoBehaviour
{
    private bool isGrounded;
    private bool isGroundedLastFrame;
    private bool isSprinting;

    [SerializeField] private float actualValueStep;
    [SerializeField] private float maxValueStep;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = gameObject.GetComponent<ControllerPlanete>().grounded;
        isSprinting = gameObject.GetComponent<ControllerPlanete>().isSprinting;
        
        if (isGrounded)
        {
            if (isSprinting)
            {
                actualValueStep += Time.deltaTime*gameObject.GetComponent<Rigidbody>().velocity.magnitude*1.5f;  
            }
            else
            {
                actualValueStep += Time.deltaTime*gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            }

            if (!isGroundedLastFrame)
            {
                PerformActionSound(MovingSound.Land);
            }
        }
        else
        {
            if (isGroundedLastFrame) PerformActionSound(MovingSound.Jump);
        }

        if (actualValueStep > maxValueStep)
        {
            actualValueStep = 0;
            PerformActionSound(MovingSound.Footstep);
        }
        
        isGroundedLastFrame = isGrounded;
    }

    private void PerformActionSound(MovingSound mocingAction)
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hit);
        switch (mocingAction)
        {
            case MovingSound.Footstep:
                GlobalEventManager.Instance.Footstep(hit.collider.gameObject);
                break;
            case MovingSound.Jump:
                GlobalEventManager.Instance.Jump(hit.collider.gameObject);
                break;
            case MovingSound.Land:
                GlobalEventManager.Instance.Land(hit.collider.gameObject);
                break;
            default:
                break;
        }
    }
    
    private enum MovingSound
    {
        None,
        Footstep,
        Jump,
        Land
    }
}
