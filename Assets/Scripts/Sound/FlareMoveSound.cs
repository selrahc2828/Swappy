using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FlareMoveSound : MonoBehaviour
{
    public EventReference eventRef;
    
    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null && !eventRef.IsNull)
        {
            FMODEventManager.instance.PlaySoundFlare(gameObject,eventRef);
        }
    }

    // private void OnPause()
    // {
    //     FMODEventManager.instance.PauseSoundFlare(gameObject,eventRef);
    // }
    //
    // private void OnPlay()
    // {
    //     FMODEventManager.instance.PlaySoundFlare(gameObject,eventRef);
    // }
    //
    // private void Update()
    // {
    //     if (gameObject.GetComponent<Rigidbody>() != null && !eventRef.IsNull)
    //     {
    //         if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < 1f)
    //         {
    //             OnPause();
    //         }
    //         else if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1f)
    //         {
    //             OnPlay();
    //         }
    //     }
    // }

    private void OnDestroy()
    {
        if (gameObject.GetComponent<Rigidbody>() != null && !eventRef.IsNull)
        {
            FMODEventManager.instance.StopSoundFlare(gameObject,eventRef);
        }
    }
}
