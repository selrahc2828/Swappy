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
                FMODEventManager.instance.GetInstanceFromEncyclopediaKey(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall).getPlaybackState(out PLAYBACK_STATE fallingState);
                if (fallingState == PLAYBACK_STATE.PLAYING)
                {
                    FMODEventManager.instance.StopEventInstance(FMODEventManager.instance.GetInstanceFromEncyclopediaKey(gameObject,FMODEventManager.instance.FMODEvents.PlayerFall));
                    FMODEventManager.instance.RemoveInstanceInEncyclopedia(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall);
                }
                if (actualValueStep < maxValueStep) fallForce = actualAirTime/maxAirTime;
                else fallForce = 1;
                PerformActionSound(MovingSound.Land,fallForce);
            }
        }
        else
        {
            if (isGroundedLastFrame)
            {
                PerformActionSound(MovingSound.Jump);
                FMODEventManager.instance.AddInstanceInEncyclopedia(gameObject, FMODEventManager.instance.FMODEvents.PlayerFall,FMODEventManager.instance.CreateEventInstance(FMODEventManager.instance.FMODEvents.PlayerFall));
                FMODEventManager.instance.PlayEventInstance(FMODEventManager.instance.GetInstanceFromEncyclopediaKey(gameObject,FMODEventManager.instance.FMODEvents.PlayerFall));
            }
            else
            {
                actualAirTime += Time.deltaTime;
            }
            
        }

        if (actualValueStep > maxValueStep)
        {
            actualValueStep = 0;
            PerformActionSound(MovingSound.Footstep);
        }
        
        isGroundedLastFrame = isGrounded;
    }

    private void PerformActionSound(MovingSound mocingAction,float fallForce = -1)
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
