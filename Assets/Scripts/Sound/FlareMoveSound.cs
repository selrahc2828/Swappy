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

    private void OnDestroy()
    {
        if (gameObject.GetComponent<Rigidbody>() != null && !eventRef.IsNull)
        {
            FMODEventManager.instance.StopSoundFlare(gameObject,eventRef);
        }
    }
}
