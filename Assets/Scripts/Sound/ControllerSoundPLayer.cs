using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class ControllerSoundPLayer : MonoBehaviour
{
    private bool isGrounded;
    private bool isGroundedLastFrame;
    private bool isSprinting;
    

    [SerializeField] private float actualValueStep;
    [SerializeField] private float maxValueStep;

    [SerializeField] private float actualAirTime;
    [SerializeField] private float maxAirTime;
    [SerializeField] private float fallForce;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = gameObject.GetComponent<ControllerPlanete>().grounded;
        isSprinting = gameObject.GetComponent<ControllerPlanete>().isSprinting;
        
        if (isGrounded)
        {
            actualAirTime = 0f;
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
                if (FMODEventManager.instance.CheckInstanceInEncylopedia(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall, out EventInstance eventInstance))
                {
                    eventInstance.getPlaybackState(out PLAYBACK_STATE fallingState);
                    if (fallingState == PLAYBACK_STATE.PLAYING)
                    {
                        FMODEventManager.instance.StopEventInstance(eventInstance);
                        FMODEventManager.instance.RemoveInstanceInEncyclopedia(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall);
                    }
                    if (actualValueStep < maxValueStep) fallForce = actualAirTime/maxAirTime;
                    else fallForce = 1f;
                }
                PerformActionSound(MovingSound.Land,fallForce);
            }
        }
        else
        {
            if (isGroundedLastFrame)
            {
                PerformActionSound(MovingSound.Jump);
                
            }
            else
            {
                
                if (transform.InverseTransformDirection(gameObject.GetComponent<Rigidbody>().velocity).y < 0f)
                {
                    actualAirTime += Time.deltaTime;
                    if (!FMODEventManager.instance.CheckInstanceInEncylopedia(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall, out EventInstance PlayerSoundFalling))
                    {
                        FMODEventManager.instance.AddInstanceInEncyclopedia(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall,FMODEventManager.instance.CreateEventInstance(FMODEventManager.instance.FMODEvents.PlayerFall));
                        FMODEventManager.instance.PlayEventInstance(FMODEventManager.instance.GetInstanceFromEncyclopediaKey(gameObject,FMODEventManager.instance.FMODEvents.PlayerFall));
                    }
                }
            }
            
        }

        if (actualValueStep > maxValueStep)
        {
            actualValueStep = 0;
            PerformActionSound(MovingSound.Footstep);
        }
        
        isGroundedLastFrame = isGrounded;
    }

    private void PerformActionSound(MovingSound mocingAction,float fallForce = -1f)
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
                if (fallForce < 0f) fallForce = 0f;
                GlobalEventManager.Instance.Land(hit.collider.gameObject, fallForce);
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
