using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;


public class FMODEventManager : MonoBehaviour
{
    #region Init
    public static FMODEventManager instance { get; private set; }
    public FMODEvents FMODEvents;
    public FMODSnapshotEvents FMODSnapshots;
    public FMODBus Fmodbus;

    private List<EventInstance> eventPlaylist;


    private Dictionary<GameObject, Dictionary<EventReference, EventInstance>> EncyclopediaInstance;
    
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

    public void PlayEvenntInstance3DNotMoving(EventInstance eventInstance, Vector3 position)
    {
        Set3DparamEventInstance(eventInstance, position);
        PlayEventInstance(eventInstance);
    }    
    public void PlayEventInstance3DMoving(EventInstance eventInstance, GameObject gameObject, Rigidbody rigidbody)
    {
        RuntimeManager.AttachInstanceToGameObject(eventInstance,gameObject.transform,rigidbody);
        PlayEventInstance(eventInstance);
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
    #region Param Encyclopedia
    public int GetPlaylistEventSize()
    {
        return eventPlaylist.Count;
    }
    
    public bool CheckInstanceInEncylopedia(GameObject _keyGameObject, EventReference _keyEventReference, out EventInstance eventInstance)
    {

        if (EncyclopediaInstance.ContainsKey(_keyGameObject))
        {
            if (EncyclopediaInstance[_keyGameObject].ContainsKey(_keyEventReference))
            {
                eventInstance = EncyclopediaInstance[_keyGameObject][_keyEventReference];
                return true;
            }
            else
            {
                Debug.Log("Event instance not referenced yet in Encyclopedia");
                eventInstance = default(EventInstance);
                return false;
            }
        }
        else
        {
            Debug.Log("Game objet not referenced yet in Encyclopedia");
            eventInstance = default(EventInstance);
            return false;
        }
    }

    public EventInstance GetInstanceFromEncyclopediaKey(GameObject _keyGameObject, EventReference _keyEventReference)
    {
        CheckInstanceInEncylopedia(_keyGameObject, _keyEventReference, out EventInstance eventInstance);
        return eventInstance;
    }

    public int GetEncyclopediaSize()
    {
        return EncyclopediaInstance.Count;
    }

    public int GetEncyclopediaPageSize(GameObject _keyGameObject)
    {
        if (EncyclopediaInstance.ContainsKey(_keyGameObject))
        {
            return EncyclopediaInstance[_keyGameObject].Count;
        }
        else
        {
            Debug.Log("Game objet not referenced yet in Encyclopedia");
            return default;
        }
    }
    

    public void AddInstanceInEncyclopedia(GameObject _keyGameObject, EventReference _keyEventReference, EventInstance newEventInstanceToAdd)
    {
        if (CheckInstanceInEncylopedia(_keyGameObject, _keyEventReference, out EventInstance eventInstance))
        {
            Debug.LogWarning("Event instance already exists");
        }
        else
        {
            if (!EncyclopediaInstance.ContainsKey(_keyGameObject))
            {
                EncyclopediaInstance.Add(_keyGameObject, new Dictionary<EventReference, EventInstance>());
            }
            EncyclopediaInstance[_keyGameObject].Add(_keyEventReference, newEventInstanceToAdd);
        }
        
    }

    public void SwitchParameterInstanceInEncyclopedia(GameObject _keyGameObject, EventReference _keyEventReference, string parameterName, float parameterValue)
    {
        if (CheckInstanceInEncylopedia(_keyGameObject, _keyEventReference, out EventInstance eventInstance))
        {
            SetNamedParamEventInstance(eventInstance, parameterName, parameterValue);
        }
        else
        {
            Debug.LogWarning("Event instance not referenced yet in Encyclopedia");
        }
    }

    public void RemoveInstanceInEncyclopedia(GameObject _keyGameObject, EventReference _keyEventReference)
    {
        if (CheckInstanceInEncylopedia(_keyGameObject, _keyEventReference, out EventInstance eventInstance))
        {
            ReleaseEventInstance(eventInstance);
            EncyclopediaInstance[_keyGameObject].Remove(_keyEventReference);
            if (EncyclopediaInstance[_keyGameObject].Count == 0)
            {
                EncyclopediaInstance[_keyGameObject] = null;
                EncyclopediaInstance.Remove(_keyGameObject);
            }
            else
            {
                Debug.LogWarning("Event instance not registered in Encyclopedia");
            }
        }
        else
        {
            Debug.Log("Game objet not referenced yet in Encyclopedia");
        }
    }

    public void CheckAllInstanceInEncyclopedia()
    {
        int playlistSize = GetPlaylistEventSize();
        int encyclopediaSize = GetEncyclopediaSize();
        int eventInstanceNumber = 0;
        foreach (KeyValuePair<GameObject, Dictionary<EventReference, EventInstance>> gameObject in EncyclopediaInstance)
        {
            eventInstanceNumber += GetEncyclopediaPageSize(gameObject.Key);
        }

        if (playlistSize > eventInstanceNumber)
        {
            Debug.LogWarning("Couple of Event instance not referenced in Encyclopedia");
        }
        else if (playlistSize == eventInstanceNumber)
        {
            Debug.Log("All Event instance found in Encyclopedia");
        }
        else if (playlistSize < eventInstanceNumber)
        {
            Debug.LogError("Event instance referenced in Encyclopedia but not exist");
        }

        float eventInstanceRateByGameObject = eventInstanceNumber / encyclopediaSize;
        if (eventInstanceRateByGameObject < 1)
        {
            Debug.LogError("More Game objects found in Encyclopedia than Event instance");
        }
        else if (eventInstanceRateByGameObject == 1)
        {
            Debug.Log("Every Game objects found in Encyclopedia got only one Event instance");
        }
        else if (eventInstanceRateByGameObject > 1)
        {
            Debug.Log("Average number of Event instance per Game Objects in Encyclopedia"+ Mathf.Round(eventInstanceRateByGameObject));
        }
        
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
        EncyclopediaInstance.Clear();
    }

    private void OnDestroy()
    {
        CleanUpAllSound();
    }
    #endregion
    
    

}