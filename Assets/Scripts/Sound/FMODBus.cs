using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class FMODBus : MonoBehaviour
{
    public Bus busMaster { get; private set; }
    public Bus busPlayer { get; private set; }
    public Bus busSystem { get; private set; }
    public Bus busMusic { get; private set; }
    public Bus busMenu { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        busMaster = RuntimeManager.GetBus("bus:/");
        busPlayer = RuntimeManager.GetBus("bus:/Player");
        busSystem = RuntimeManager.GetBus("bus:/System");
        busMusic = RuntimeManager.GetBus("bus:/Music");
        //busMenu = RuntimeManager.GetBus("bus:/Menu");
    }

    public void Mute(Bus bus)
    {
        bus.getMute(out bool busMuteState);
        if (busMuteState)
        { 
            Debug.LogWarning(bus + "Unmute");
            bus.setMute(false);
        }
        else
        {
            Debug.LogWarning(bus + "Mute");
            bus.setMute(true);
        }
    }

    public void ChangeVolume(Bus bus,float volume)
    {
        bus.setVolume(volume);
    }
    
}
