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

    private void OnEnable()
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
        GlobalEventManager.Instance.OnLand += OnLand;
        GlobalEventManager.Instance.OnCollide += CollisionSound;
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
        GlobalEventManager.Instance.OnLand -= OnLand;
        GlobalEventManager.Instance.OnCollide -= CollisionSound;
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
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
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
                Debug.Log("Event instance not referenced yet in Encyclopedia"+_keyGameObject.name+_keyEventReference);
                eventInstance = default(EventInstance);
                return false;
            }
        }
        else
        {
            Debug.Log("GameObjet not referenced yet in Encyclopedia"+_keyGameObject.name);
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
                Debug.LogWarning("Event instance not registered in Encyclopedia"+_keyEventReference+_keyGameObject.name);
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
                //Debug.Log("Every Game objects found in Encyclopedia got only one Event instance");
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
    
    #region Player Interact
    private void OnComportementExtracted(GameObject _gameObject, bool rightvalue, bool righthand)
    {
        var eventInstance = CreateEventInstance(FMODEvents.PlayerStealComp);
        PickHand(eventInstance,righthand);
        if (_gameObject.CompareTag("Player"))
        {
            SetNamedParamEventInstance(eventInstance,"SIM",1);
        }
        
        PlayEventInstance(eventInstance);
        ReleaseEventInstance(eventInstance);
    }
    private void OnComportementAdded(GameObject _gameObject, bool rightvalue, bool righthand)
    {
        var eventInstance = CreateEventInstance(FMODEvents.PlayerGiveComp);
        PickHand(eventInstance,righthand);
        if (_gameObject.CompareTag("Player"))
        {
            SetNamedParamEventInstance(eventInstance,"SIM",1);
        }
        
        PlayEventInstance(eventInstance);
        ReleaseEventInstance(eventInstance);
    }
    private void OnComportmentExchanged(GameObject _gameObject, bool righthand)
    {
        var eventInstance = CreateEventInstance(FMODEvents.PlayerSelfSwitch);
        PickHand(eventInstance,righthand);
        PlayEventInstance(eventInstance);
        ReleaseEventInstance(eventInstance);
    }
    private void OnSelfImpactMode(GameObject _gameObject, bool isActive)
    {
        if (isActive)
        {
            AddInstanceInEncyclopedia(_gameObject,FMODEvents.PlayerSelfImpactMode,CreateEventInstance(FMODEvents.PlayerSelfImpactMode));
            PlayEventInstance(GetInstanceFromEncyclopediaKey(_gameObject,FMODEvents.PlayerSelfImpactMode));
        }
        else
        {
            StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject,FMODEvents.PlayerSelfImpactMode));
            RemoveInstanceInEncyclopedia(_gameObject,FMODEvents.PlayerSelfImpactMode);
        }
    }

    private void PickHand(EventInstance eventInstance, bool righthand)
    {
        if (righthand)
        {
            SetNamedParamEventInstance(eventInstance,"HAND",2);
        }
        else
        {
            SetNamedParamEventInstance(eventInstance,"HAND",0);
        }
    }
    #endregion

    #region Comportement Sound
    private enum Step 
    {
        Enter,
        Play,
        Exit
    }
    private void OnEnterComportement(GameObject _gameObject)
    {
        Step step = Step.Enter;
        GetComportementState(_gameObject,step,0.8f);
    }
    private void OnComportementIsPlay(GameObject _gameObject,float force=0.8f)
    {
        Step step = Step.Play;
        GetComportementState(_gameObject,step,force);
    }
    private void OnExitComportement(GameObject _gameObject)
    {
        Step step = Step.Exit;
        GetComportementState(_gameObject,step,0.8f);
    }
    private void GetComportementState(GameObject _gameObject, Step step, float force)
    {
        var stateMachine = _gameObject.GetComponent<ComportementsStateMachine>();
        if (stateMachine.currentState is ComportementState)
        {
            ComportementState gameObjectCurrenState = (ComportementState)stateMachine.currentState;
            FindStateComportement(_gameObject, gameObjectCurrenState.stateValue,step,force);
        }
    }
    private void FindStateComportement(GameObject _gameObject, int stateValue, Step step, float force)
    {
        switch (stateValue)
        {
            case 0:
                break;
            case 1:
                ActionOnStateComportement(_gameObject, FMODEvents.Impulse, step, force);
                break;
            case 2:
                ActionOnStateComportement(_gameObject, FMODEvents.DoubleImpulse, step, force);
                break;
            case 3:
                ActionOnStateComportement(_gameObject, FMODEvents.Bounce, step, force);
                break;
            case 4:
                ActionOnStateComportement(_gameObject, FMODEvents.ImpulseBounce, step, force);
                break;
            case 6:
                ActionOnStateComportement(_gameObject, FMODEvents.DoubleBounce, step, force);
                break;
            case 9:
                ActionOnStateComportement(_gameObject, FMODEvents.Immuable, step, force);
                break;
            case 10:
                ActionOnStateComportement(_gameObject, FMODEvents.ImpulseImmuable, step, force);
                break;
            case 12:
                ActionOnStateComportement(_gameObject, FMODEvents.BounceImmuable, step, force);
                break;
            case 18:
                ActionOnStateComportement(_gameObject, FMODEvents.DoubleImmuable, step, force);
                break;
            case 27:
                ActionOnStateComportement(_gameObject, FMODEvents.Magnet, step, force);
                break;
            case 28:
                ActionOnStateComportement(_gameObject, FMODEvents.ImpulseMagnet, step, force);
                break;
            case 30:
                ActionOnStateComportement(_gameObject, FMODEvents.BounceMagnet, step, force);
                break;
            case 36:
                ActionOnStateComportement(_gameObject, FMODEvents.ImmuableMagnet, step, force);
                break;
            case 54:
                ActionOnStateComportement(_gameObject, FMODEvents.DoubleMagnet, step, force);
                break;
            case 81:
                ActionOnStateComportement(_gameObject, FMODEvents.Rocket, step, force);
                break;
            case 82:
                ActionOnStateComportement(_gameObject, FMODEvents.ImpulseRocket, step, force);
                break;
            case 84:
                ActionOnStateComportement(_gameObject, FMODEvents.BounceRocket, step, force);
                break;
            case 90:
                ActionOnStateComportement(_gameObject, FMODEvents.ImmuableRocket, step, force);
                break;
            case 108:
                ActionOnStateComportement(_gameObject, FMODEvents.MagnetRocket, step, force);
                break;
            case 162:
                ActionOnStateComportement(_gameObject, FMODEvents.DoubleRocket, step, force);
                break;
            default:
                Debug.LogError("Si tu fais apparaitre ça c'est que t'es vraiment for");
                break;
        }
    }
    private void ActionOnStateComportement(GameObject _gameObject, EventReference _eventReference, Step step, float force)
    {
        switch (step)
        {
            case Step.Enter:
                AddInstanceInEncyclopedia(_gameObject, _eventReference,CreateEventInstance(_eventReference));
                PlayEventInstance3DMoving(GetInstanceFromEncyclopediaKey(_gameObject,_eventReference),_gameObject,_gameObject.GetComponent<Rigidbody>());
                break;
            case Step.Play:
                SetNamedParamEventInstance(GetInstanceFromEncyclopediaKey(_gameObject,_eventReference), "POWER", force);
                SetNamedParamEventInstance(GetInstanceFromEncyclopediaKey(_gameObject,_eventReference), "Stinger",1);
                break;
            case Step.Exit:
                StopEventInstance(GetInstanceFromEncyclopediaKey(_gameObject,_eventReference));
                RemoveInstanceInEncyclopedia(_gameObject,_eventReference);
                break;
            default:
                break;
        }
    }
   
    
    #endregion
    
    #region Player Moving Sound
    private void OnFootstep(GameObject _groundCollider)
    {
        MovingTypeSound _movingType = MovingTypeSound.Footstep;
        ActionOnPlayerMove(_groundCollider,_movingType);
    }
    private void OnJump(GameObject _groundCollider)
    {
        MovingTypeSound _movingType = MovingTypeSound.Jump;
        ActionOnPlayerMove(_groundCollider,_movingType);
    }
    private void OnLand(GameObject _groundCollider)
    {
        MovingTypeSound _movingType = MovingTypeSound.Land;
        ActionOnPlayerMove(_groundCollider,_movingType);
    }
    private enum MovingTypeSound
    {
        Footstep,
        Jump,
        Land
    }
    private void MovingRef(MovingTypeSound _movingType, out EventReference eventReference)
    {
        switch (_movingType)
        {
            case MovingTypeSound.Footstep:
                eventReference=FMODEvents.PlayerFootsteps;
                break;
            case MovingTypeSound.Jump:
                eventReference=FMODEvents.PlayerJump;
                break;
            case MovingTypeSound.Land:  
                eventReference=FMODEvents.PlayerLand;
                break;
            default:
                eventReference=default(EventReference);
                break;
        }
    }
    private void SwitchGround(GameObject _groundCollider, EventInstance eventInstance)
    {
        switch (_groundCollider.tag)
        {
            case "GRASS":
                SetNamedParamEventInstance(eventInstance,"GROUND",1);
                break;
            case "ROCK":
                SetNamedParamEventInstance(eventInstance,"GROUND",2);
                break;
            case "SAND":
                SetNamedParamEventInstance(eventInstance,"GROUND",3);
                break;
            case "WOOD":
                SetNamedParamEventInstance(eventInstance,"GROUND",4);
                break;
            case "METAL":
                SetNamedParamEventInstance(eventInstance,"GROUND",5);
                break;
            default:
                SetNamedParamEventInstance(eventInstance,"GROUND",0);
                break;
        }
    }
    private void ActionOnPlayerMove(GameObject _gameObject, MovingTypeSound _movingType)
    {
        MovingRef(_movingType, out EventReference _eventReference);
         var _eventInstance = CreateEventInstance(_eventReference);
        SwitchGround(_gameObject,_eventInstance);
        PlayEventInstance(_eventInstance);
        ReleaseEventInstance(_eventInstance);
    }
    #endregion
    
    #region collision

    private void CollisionSound(GameObject _gameObject)
    {
        var getReference = FMODEvents.Collision;
        PlayOneShotAttached(getReference, _gameObject);
        
    }
    #endregion
    
    
    
    #endregion

}