using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEditor;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;


public class FMODEventManager : MonoBehaviour
{
    #region Init
    public static FMODEventManager instance { get; private set; }
    public FMODEvents FMODEvents;
    public FMODSnapshotEvents FMODSnapshots;
    public FMODBus Fmodbus;

    private List<EventInstance> _eventPlaylist = new List<EventInstance>();


    private Dictionary<GameObject, Dictionary<EventReference, EventInstance>> EncyclopediaInstance = new Dictionary<GameObject, Dictionary<EventReference, EventInstance>>();

    private ComportementsStateMachine _comportementState;
    
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

    private void Start()
    {
        GlobalEventManager.Instance.OnComportmentExtracted += OnComportementExtracted;
        GlobalEventManager.Instance.OnComportmentAdded += OnComportementAdded;
        GlobalEventManager.Instance.OnComportmentExchanged += OnComportmentExchanged;
        GlobalEventManager.Instance.OnSelfImpactMod += OnSelfImpactMode;
        GlobalEventManager.Instance.OnComportementStateEnter += OnEnterComportement;
        GlobalEventManager.Instance.OnComportementStateExit += OnExitComportement;
        GlobalEventManager.Instance.OnComportementStatePlay += OnComportementIsPlay;
        GlobalEventManager.Instance.OnFootstep += OnFootstep;
        GlobalEventManager.Instance.OnJump += OnJump;
    }

    private void OnDisable()
    {
        GlobalEventManager.Instance.OnComportmentExtracted -= OnComportementExtracted;
        GlobalEventManager.Instance.OnComportmentAdded -= OnComportementAdded;
        GlobalEventManager.Instance.OnComportmentExchanged -= OnComportmentExchanged;
        GlobalEventManager.Instance.OnSelfImpactMod -= OnSelfImpactMode;
        GlobalEventManager.Instance.OnComportementStateEnter -= OnEnterComportement;
        GlobalEventManager.Instance.OnComportementStateExit -= OnExitComportement;
        GlobalEventManager.Instance.OnComportementStatePlay -= OnComportementIsPlay;
        GlobalEventManager.Instance.OnFootstep -= OnFootstep;
        GlobalEventManager.Instance.OnJump -= OnJump;
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
        //_eventPlaylist.Add(eventInstance);
      
        return eventInstance;
    }

    public void PlayEventInstance(EventInstance eventInstance)
    {

            eventInstance.start();

    }

    public void PlayEventInstance3DNotMoving(EventInstance eventInstance, Vector3 position)
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
        eventInstance.getPlaybackState(out PLAYBACK_STATE eventState);
        if (eventState != PLAYBACK_STATE.STOPPED || eventState != PLAYBACK_STATE.STOPPING)
        {
            eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    public void ReleaseEventInstance(EventInstance eventInstance)
    {
            eventInstance.release();
            _eventPlaylist.Remove(eventInstance);
    }
    #endregion
    #region Param Encyclopedia
    public int GetPlaylistEventSize()
    {
        return _eventPlaylist.Count;
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
            Debug.Log("GameObjet not referenced yet in Encyclopedia");
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
            EncyclopediaInstance[_keyGameObject].Remove(_keyEventReference);
            ReleaseEventInstance(eventInstance);
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
        else if (playlistSize < eventInstanceNumber)
        {
            Debug.LogError("Event instance referenced in Encyclopedia but not exist");
        }

        if (encyclopediaSize == 0)
        {
            Debug.Log("No Event instance in Encyclopedia");
        }
        else
        {
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
        CheckAllInstanceInEncyclopedia();
        foreach (EventInstance eventInstance in _eventPlaylist)
        {
            eventInstance.stop(STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        _eventPlaylist.Clear();
        EncyclopediaInstance.Clear();
    }

    private void OnDestroy()
    {
        CleanUpAllSound();
    }
    #endregion
    
    #region C# Event

    private void OnComportementExtracted(GameObject _gameObject, bool rightvalue, bool righthand)
    {
        if (!CheckInstanceInEncylopedia(gameObject, FMODEvents.PlayerStealComp, out EventInstance eventInstance))
        {
            eventInstance = CreateEventInstance(FMODEvents.PlayerStealComp);
        }

        if (righthand)
        {
            SetNamedParamEventInstance(eventInstance,"HAND",2);
        }
        else
        {
            SetNamedParamEventInstance(eventInstance,"HAND",0);
        }
        
        PlayEventInstance(eventInstance);
        ReleaseEventInstance(eventInstance);
    }

    private void OnComportementAdded(GameObject _gameObject, bool rightvalue, bool righthand)
    {
        if (!CheckInstanceInEncylopedia(gameObject, FMODEvents.PlayerGiveComp, out EventInstance eventInstance))
        {
            eventInstance = CreateEventInstance(FMODEvents.PlayerGiveComp);
        }

        if (righthand)
        {
            SetNamedParamEventInstance(eventInstance,"HAND",2);
        }
        else
        {
            SetNamedParamEventInstance(eventInstance,"HAND",0);
        }
        
        PlayEventInstance(eventInstance);
        ReleaseEventInstance(eventInstance);
    }

    private void OnComportmentExchanged(GameObject _gameObject, bool righthand, bool comportementIn, bool comportementOut)
    {
        if (!CheckInstanceInEncylopedia(_gameObject, FMODEvents.PlayerSelfSwitch, out EventInstance eventInstance))
        {
            eventInstance = CreateEventInstance(FMODEvents.PlayerSelfSwitch);
        }
        if(righthand)
        {
            SetNamedParamEventInstance(eventInstance,"HAND",2);
        }
        else
        {
            SetNamedParamEventInstance(eventInstance,"HAND",0);
        }

        if (comportementIn)
        {
            SetNamedParamEventInstance(eventInstance,"IN",1);
        }
        if (comportementOut)
        {
            SetNamedParamEventInstance(eventInstance,"OUT",1);
        }
        
        PlayEventInstance(eventInstance);
        ReleaseEventInstance(eventInstance);
    }
    
    private void OnSelfImpactMode(GameObject _gameObject, bool isActive)
    {
        var GetInstance=GetInstanceFromEncyclopediaKey(_gameObject,FMODEvents.PlayerSelfImpactMode);
        if (isActive)
        {
            AddInstanceInEncyclopedia(_gameObject,FMODEvents.PlayerSelfImpactMode,CreateEventInstance(FMODEvents.PlayerSelfImpactMode));
            PlayEventInstance(GetInstance);
        }
        else
        {
            StopEventInstance(GetInstance);
            RemoveInstanceInEncyclopedia(_gameObject,FMODEvents.PlayerSelfImpactMode);
        }
    }


    #region Comportement Sound
    private void OnEnterComportement(GameObject _gameObject)
    {
        var stateMachine = _gameObject.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
                ComportementState gameObjectCurrenState = (ComportementState) stateMachine.currentState ;
                switch (gameObjectCurrenState.stateValue)
                {
                    case 0:
                        break;
                    case 1: 
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.Impulse,CreateEventInstance(FMODEvents.Impulse));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Impulse),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 2:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleImpulse,CreateEventInstance(FMODEvents.DoubleImpulse));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleImpulse),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 3:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.Bounce,CreateEventInstance(FMODEvents.Bounce));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Bounce),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 4:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseBounce,CreateEventInstance(FMODEvents.ImpulseBounce));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseBounce),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 6:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleBounce,CreateEventInstance(FMODEvents.DoubleBounce));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleBounce),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 9:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.Immuable,CreateEventInstance(FMODEvents.Immuable));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Immuable),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 10:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseImmuable,CreateEventInstance(FMODEvents.ImpulseImmuable));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseImmuable),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 12:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.BounceImmuable,CreateEventInstance(FMODEvents.BounceImmuable));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.BounceImmuable),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 18:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleImmuable,CreateEventInstance(FMODEvents.DoubleImmuable));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleImmuable),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 27:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.Magnet,CreateEventInstance(FMODEvents.Magnet));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Magnet),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 28:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseMagnet,CreateEventInstance(FMODEvents.ImpulseMagnet));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseMagnet),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 30:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.BounceMagnet,CreateEventInstance(FMODEvents.BounceMagnet));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.BounceMagnet),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 36:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.ImmuableMagnet,CreateEventInstance(FMODEvents.ImmuableMagnet));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImmuableMagnet),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 54:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleMagnet,CreateEventInstance(FMODEvents.DoubleMagnet));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleMagnet),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 81:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.Rocket,CreateEventInstance(FMODEvents.Rocket));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Rocket),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 82:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseRocket,CreateEventInstance(FMODEvents.ImpulseRocket));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseRocket),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 84:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.BounceRocket,CreateEventInstance(FMODEvents.BounceRocket));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.BounceRocket),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 90:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.ImmuableRocket,CreateEventInstance(FMODEvents.ImmuableRocket));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImmuableRocket),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 108:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.RocketMagnet,CreateEventInstance(FMODEvents.RocketMagnet));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.RocketMagnet),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    case 162:
                        AddInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleRocket,CreateEventInstance(FMODEvents.DoubleRocket));
                        PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleRocket),_gameObject,_gameObject.GetComponent<Rigidbody>());
                        break;
                    default:
                        Debug.LogError("Si tu fais apparaitre ça c'est que t'es vraiment for");
                        break;
                }
        }
    }
    private void OnComportementIsPlay(GameObject _gameObject)
    {
        var stateMachine = _gameObject.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState gameObjectCurrentState = (ComportementState)stateMachine.currentState;
            switch (gameObjectCurrentState.stateValue)
            {
                case 0:
                    break;
                case 1:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.Impulse,"Stinger",1);
                    break;
                case 2:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleImpulse,"Stinger",1);
                    break;
                case 3:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.Bounce,"Stinger",1);
                    break;
                case 4:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseBounce,"Stinger",1);
                    break;
                case 6:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleBounce,"Stinger",1);
                    break;
                case 9:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.Immuable,"Stinger",1);
                    break;
                case 10:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseImmuable,"Stinger",1);
                    break;
                case 12:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.BounceImmuable,"Stinger",1);
                    break;
                case 18:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleImmuable,"Stinger",1);
                    break;
                case 27:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.Magnet,"Stinger",1);
                    break;
                case 28:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseMagnet,"Stinger",1);
                    break;
                case 30:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.BounceMagnet,"Stinger",1);
                    break;
                case 36:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.ImmuableMagnet,"Stinger",1);
                    break;
                case 54:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleMagnet,"Stinger",1);
                    break;
                case 81:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.Rocket,"Stinger",1);
                    break;
                case 82:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.ImpulseRocket,"Stinger",1);
                    break;
                case 84:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.BounceRocket,"Stinger",1);
                    break;
                case 90:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.ImmuableRocket,"Stinger",1);
                    break;
                case 108:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.RocketMagnet,"Stinger",1);
                    break;
                case 162:
                    SwitchParameterInstanceInEncyclopedia(_gameObject,FMODEvents.DoubleRocket,"Stinger",1);
                    break;
                default:
                    Debug.LogError("Si tu fais apparaitre ça c'est que t'es vraiment for");
                    break;
            }
        }
    }
    private void OnExitComportement(GameObject _gameObject)
    {
        var stateMachine = _gameObject.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState gameObjectCurrenState = (ComportementState)stateMachine.currentState;
            switch (gameObjectCurrenState.stateValue)
            {
                case 0:
                    break;
                case 1:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Impulse));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.Impulse);
                    break;
                case 2:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleImpulse));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.DoubleImpulse);
                    break;
                case 3:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Bounce));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.Bounce);
                    break;
                case 4:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseBounce));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.ImpulseBounce);
                    break;
                case 6:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleBounce));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.DoubleBounce);
                    break;
                case 9:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Immuable));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.Immuable);
                    break;
                case 10:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseImmuable));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.ImpulseImmuable);
                    break;
                case 12:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.BounceImmuable));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.BounceImmuable);
                    break;
                case 18:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleImmuable));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.DoubleImmuable);
                    break;
                case 27:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Magnet));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.Magnet);
                    break;
                case 28:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseMagnet));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.ImpulseMagnet);
                    break;
                case 30:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.BounceMagnet));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.BounceMagnet);
                    break;
                case 36:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImmuableMagnet));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.ImmuableMagnet);
                    break;
                case 54:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleMagnet));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.DoubleMagnet);
                    break;
                case 81:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.Rocket));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.Rocket);
                    break;
                case 82:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImpulseRocket));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.ImpulseRocket);
                    break;
                case 84:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.BounceRocket));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.BounceRocket);
                    break;
                case 90:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.ImmuableRocket));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.ImmuableRocket);
                    break;
                case 108:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.RocketMagnet));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.RocketMagnet);
                    break;
                case 162:
                    StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject, FMODEvents.DoubleRocket));
                    RemoveInstanceInEncyclopedia(_gameObject, FMODEvents.DoubleRocket);
                    break;
                default:
                    Debug.LogError("Si tu fais apparaitre ça c'est que t'es vraiment for");
                    break;
            }
        }
    }
    
    private enum Step 
    {
        Enter,
        Play,
        Exit
    }

    private void ActionOnStateComportement(GameObject _gameObject, EventReference eventReference, Step step)
    {
        var GetInstance = GetInstanceFromEncyclopediaKey(_gameObject, eventReference);
        switch (step)
        {
            case Step.Enter:
                AddInstanceInEncyclopedia(_gameObject, eventReference,CreateEventInstance(eventReference));
                PlayEventInstance3DMoving(GetInstance,_gameObject,_gameObject.GetComponent<Rigidbody>());
                break;
            default:
                break;
            
        }
    }
    
    #endregion
    
    #region Player Moving Sound

    private void OnFootstep(GameObject _groundCollider)
    {
        AddInstanceInEncyclopedia(_groundCollider,FMODEvents.PlayerFootsteps,CreateEventInstance(FMODEvents.PlayerFootsteps));
        var GetInstance = GetInstanceFromEncyclopediaKey(_groundCollider, FMODEvents.PlayerFootsteps);
        SwitchGround(_groundCollider,GetInstance);
        PlayEventInstance(GetInstance);
        RemoveInstanceInEncyclopedia(_groundCollider,FMODEvents.PlayerFootsteps);
    }

    private void OnJump(GameObject _groundCollider)
    {
        AddInstanceInEncyclopedia(_groundCollider,FMODEvents.PlayerJump,CreateEventInstance(FMODEvents.PlayerJump));
        var GetInstance = GetInstanceFromEncyclopediaKey(_groundCollider, FMODEvents.PlayerJump);
        SwitchGround(_groundCollider,GetInstance);
        PlayEventInstance(GetInstance);
        RemoveInstanceInEncyclopedia(_groundCollider,FMODEvents.PlayerJump);
    }

    private void OnLand(GameObject _groundCollider)
    {
        AddInstanceInEncyclopedia(_groundCollider,FMODEvents.PlayerLand,CreateEventInstance(FMODEvents.PlayerLand));
        var GetInstance = GetInstanceFromEncyclopediaKey(_groundCollider, FMODEvents.PlayerLand);
        SwitchGround(_groundCollider,GetInstance);
        PlayEventInstance(GetInstance);
        
    }

    private void SwitchGround(GameObject _groundCollider, EventInstance eventInstance)
    {
        switch (_groundCollider.tag)
        {
            case "Grass":
                SetNamedParamEventInstance(eventInstance,"GROUND",1);
                break;
            case "Rock":
                SetNamedParamEventInstance(eventInstance,"GROUND",1);
                break;
            case "Sand":
                SetNamedParamEventInstance(eventInstance,"GROUND",1);
                break;
            case "Wood":
                SetNamedParamEventInstance(eventInstance,"GROUND",1);
                break;
            default:
                SetNamedParamEventInstance(eventInstance,"GROUND",1);
                break;
        }
    }
    #endregion
    
    
    
    #endregion

}