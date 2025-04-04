using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;


public class FMODEventManager : MonoBehaviour
{
    #region Init
    public static FMODEventManager instance { get; private set; }
    public FMODEvents FMODEvents;
    public FMODSnapshotEvents FMODSnapshots;
    public FMODBus Fmodbus;

    private List<EventInstance> eventPlaylist = new List<EventInstance>();
    
    private void Awake()
    {
        if (instance != null)
        {
           Destroy(gameObject);
           return;
        }
        instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region Play Once
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public void PlayOneShotAttached(EventReference sound, GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(sound, gameObject);
    }
    #endregion
    #region Param Instances
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventPlaylist.Add(eventInstance);
        return eventInstance;
    }

    public void PlayEventInstance(EventInstance eventInstance)
    {
        eventInstance.start();
    }
    
    public void PlayEventInstance3D(EventInstance eventInstance, GameObject gameObject, Rigidbody rigidbody)
    {
        RuntimeManager.AttachInstanceToGameObject(eventInstance,gameObject.transform,rigidbody);
        eventInstance.start();
    }
    
    public float GetNamedParamEventInstance(EventInstance eventInstance, string name)
    {
        eventInstance.getParameterByName(name, out float value);
        return value;
    }
    
    public void SetNamedParamEventInstance(EventInstance eventInstance, string name, float value)
    {
        eventInstance.setParameterByName(name, value);
    }
    
    public void Set3DparamEventInstance(EventInstance eventInstance,Vector3 position)
    {
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
    }

    public void StopEventInstance(EventInstance eventInstance)
    {
        if (eventInstance.getPlaybackState(out PLAYBACK_STATE eventState) != (RESULT)PLAYBACK_STATE.STOPPED)
        {
            eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    public void ReleaseEventInstance(EventInstance eventInstance)
    {
        if (eventInstance.getPlaybackState(out PLAYBACK_STATE eventState) == (RESULT)PLAYBACK_STATE.STOPPED || eventState == PLAYBACK_STATE.STOPPING)
        {
            eventInstance.release();
            eventPlaylist.Remove(eventInstance);
            //Debug.Log("event instance is release");
        }
        else
        {
            Debug.LogWarning("event instance isn't release");
        }
        
        
    }
    #endregion
    #region Param Playlist
    public float GetPlaylistEventSize()
    {
        return eventPlaylist.Count;
    }
    #endregion
    #region Param Bus
    public void Mute(string busRef)
    {
        Bus bus = RuntimeManager.GetBus(busRef);
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

    public void ChangeVolume(string busRef,float volume)
    {
        Bus bus = RuntimeManager.GetBus(busRef);
        bus.setVolume(volume);
    }
    #endregion
    #region On destroy
    private void CleanUpAllSound()
    {
        foreach (EventInstance eventInstance in eventPlaylist)
        {
            eventInstance.stop(STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        eventPlaylist.Clear();
    }

    private void OnDestroy()
    {
        CleanUpAllSound();
    }
    #endregion
}