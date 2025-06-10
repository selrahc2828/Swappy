using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace FMODUnity
{
    public class AudioManager : Singleton<AudioManager>
    {
        public EncyclopediAudio EncyclopediAudio;
        
        protected override void Initialize()
        {
            base.Initialize();
            EncyclopediAudio = new EncyclopediAudio();
        }

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }

        protected override void OnDestroy()
        {
            EncyclopediAudio.CleanUpSound();
            base.OnDestroy();
        }
        #region TabSetting
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                RuntimeManager.GetBus("bus:/").setMute(false);
            }
            else
            {
                RuntimeManager.GetBus("bus:/").setMute(true);
            }
        }
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                RuntimeManager.GetBus("bus:/").setMute(true);
            }
            else
            {
                RuntimeManager.GetBus("bus:/").setMute(false);
            }
        }
        #endregion
        #region SoundSetting

        public void SetBusVolume(string path, float volume, bool isDebugging = false)
        {//The method return nothing but set the volume value of the choosen bus.
            RuntimeManager.GetBus(path).setVolume(volume);
            if(isDebugging) Debug.Log("The volume of the Bus "+path+" is now set to "+volume);
        }
        public void SetMute(string path, bool mute, bool isDebugging = false)
        {//The method return nothing but set the mute state of the choosen bus. Can be used as a debug. 
            RuntimeManager.GetBus(path).getMute(out bool isMute);
            if (mute)
            {
                if (!isMute)
                {
                    RuntimeManager.GetBus(path).setMute(true);
                    if(isDebugging) Debug.Log("Bus "+path+" is now muted.");
                }
                else
                {
                    if (isDebugging) Debug.Log("Bus "+path+" is already muted.");
                }
                
            }
            else
            {
                if (isMute)
                {
                    RuntimeManager.GetBus(path).setMute(false);
                    if(isDebugging) Debug.Log("Bus "+path+" is now unmuted.");
                }
                else
                {
                    if (isDebugging) Debug.Log("Bus "+path+" is already unmuted.");
                }
            }
        }
        #endregion
    }
    
    #region AudioSystem
    #region EncyclopediAudio
    public struct EncyclopediAudio
    {
        private Dictionary<GameObject,Dictionary<EventReference,AudioInstance>> _encyclopediaAudio;     //The Dictionary of dictionaries which represent the EncyclopediAudio
        
        private bool CheckAudioInstance(GameObject keyObject, EventReference reference, out AudioInstance audioInstance, bool isDebugging = false)
        {//The method return a bool to express if an AudioInstance exist on the KeyObject and with the EventReference, but also return the AudioInstance. Can be used as a debug.
            if (_encyclopediaAudio.ContainsKey(keyObject))
            {
                if (_encyclopediaAudio[keyObject].ContainsKey(reference))
                {
                    audioInstance = _encyclopediaAudio[keyObject][reference]; 
                    if (isDebugging) Debug.Log("AudioInstance FOUND at "+keyObject.name+", "+ reference);
                    return true;
                }
                else if(isDebugging) Debug.LogError("AudioInstance NOT FOUND in "+keyObject.name+".");
            }
            else if (isDebugging) Debug.LogError("GameObject NOT FOUND in EncyclopediAudio.");
            audioInstance = default;
            return false;
        }
        public bool CheckAudioInstance(AudioInstance audioInstance, bool isDebugging = false)
        {//The method return a bool to express if an AudioInstance exist, but also return the AudioInstance. Can be used as a debug.
            GameObject keyObject = audioInstance.GetKeyObject(isDebugging);
            EventReference reference = audioInstance.GetReference(isDebugging);
            if (_encyclopediaAudio.ContainsKey(keyObject))
            {
                if (_encyclopediaAudio[keyObject].ContainsKey(reference))
                {
                    if (isDebugging) Debug.Log("AudioInstance FOUND at "+keyObject.name+", "+ reference);
                    return true;
                }
                else if(isDebugging) Debug.LogError("AudioInstance NOT FOUND in "+keyObject.name+".");
            }
            else if (isDebugging) Debug.LogError("GameObject NOT FOUND in EncyclopediAudio.");
            return false;
        }
        
        public AudioInstance GetAudioInstance(GameObject keyObject, EventReference reference, bool isDebugging = false)
        {//The method return a AudioInstance corresponding to its KeyObject and its reference from the EncyclopediAudio. Can be used as a debug
            if (CheckAudioInstance(keyObject, reference, out AudioInstance audioInstance, isDebugging))
            {
                if (isDebugging) Debug.Log("AudioInstance SUCCESSFULLY GET from EncyclopediAudio in "+ keyObject.name+" at "+ reference.ToString()+".");
            }
            else
            {
                audioInstance = CreateAudioInstance(keyObject, reference, isDebugging);
            }
            return audioInstance;
        }
        public AudioInstance CreateAudioInstance(GameObject keyObject, EventReference reference, bool isDebugging = false)
        {//The method return a AudioInstance create from its KeyObject and its Reference and put in the EncyclopediAudio. Can be used as a debug
            if (CheckAudioInstance(keyObject, reference, out AudioInstance audioInstance,isDebugging))
            {
                if (isDebugging) Debug.Log("AudioInstance GET from EncyclopediAudio, instead of creating it, because it already exists.");
                return audioInstance;
            }
            else
            {
                audioInstance.CreateInstance(keyObject, reference,isDebugging);
                if (!_encyclopediaAudio.ContainsKey(keyObject))
                {
                    _encyclopediaAudio.Add(keyObject, new Dictionary<EventReference,AudioInstance>());
                    if (isDebugging) Debug.Log("New KeyObject SUCCESSFULLY CREATE in EncyclopediAudio");
                }
                _encyclopediaAudio[keyObject][reference] = audioInstance;
                if (isDebugging) Debug.Log("AudioInstance SUCCESSFULLY CREATE and ADD in EncyclopediAudio");
                return audioInstance;
            }
        }

        private void ReleaseAudioInstance(GameObject keyObject, EventReference reference, bool isDebugging = false)
        {//The method return nothing but release and remove the AudioInstance corresponding to its KeyObject and its reference from the EncyclopediAudio. Can be used as a debug
            if (CheckAudioInstance(keyObject, reference, out AudioInstance audioInstance, isDebugging))
            {
                audioInstance.ReleaseInstance(isDebugging);
                _encyclopediaAudio[keyObject].Remove(reference);
                if(isDebugging) Debug.Log("AudioInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (_encyclopediaAudio[keyObject].Count == 0)
                {
                    _encyclopediaAudio.Remove(keyObject);
                    if(isDebugging) Debug.Log("KeyObject SUCCESSFULLY REMOVE from EncyclopediAudio");
                }
            }
            else
            {
                if(isDebugging) Debug.LogWarning("AudioInstance can't be remove and release from EncyclopediAudio, because it don't exist in it.");
            }
        }
        public void ReleaseAudioInstance(AudioInstance audioInstance, bool isDebugging = false)
        {//The method return nothing but release and remove the AudioInstance from the EncyclopediAudio. Can be used as a debug
            if (CheckAudioInstance(audioInstance,isDebugging))
            {
                audioInstance.ReleaseInstance(isDebugging);
                _encyclopediaAudio[audioInstance.GetKeyObject(isDebugging)].Remove(audioInstance.GetReference(isDebugging));
                if(isDebugging) Debug.Log("AudioInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (_encyclopediaAudio[audioInstance.GetKeyObject(isDebugging)].Count == 0)
                {
                    _encyclopediaAudio.Remove(audioInstance.GetKeyObject(isDebugging));
                    if(isDebugging) Debug.Log("KeyObject SUCCESSFULLY REMOVE from EncyclopediAudio");
                }
            }
            else
            {
                if(isDebugging) Debug.LogWarning("AudioInstance can't be remove and release from EncyclopediAudio, because it don't exist in it.");
            }
        }

        public int CountKeyPage(bool isDebugging = false)
        {//The method return the number of GameObject referenced in the EncyclopediAudio
            int count = 0;
            foreach (KeyValuePair<GameObject,Dictionary<EventReference,AudioInstance>> _page in _encyclopediaAudio)
            {
                count++;
            }
            if(isDebugging) Debug.Log("There is "+count+" pages KeyObject referenced in EncyclopediAudio");
            return count;
        }
        public int CountAudioInstance(GameObject keyObject, bool isDebugging = false)
        {//The method return the number of AudioInstance referenced in a KeyObject, in the EncyclopediAudio
            int count = 0;
            foreach (KeyValuePair<EventReference, AudioInstance> _event in _encyclopediaAudio[keyObject])
            {
                count++;
            }
            if(isDebugging) Debug.Log("There is "+count+" AudioInstance on the "+keyObject.name+" KeyObject referenced in EncyclopediAudio");
            return count;
        }

        public bool CountAll(bool isDebugging = false)
        {//The method return a bool to express if the EncyclopediAudio is empty. Can be used as a debug to express in string the whole EncyclopediAudio
            bool isEmpty = true;
            if (isDebugging)
            {
                int pages = CountKeyPage(isDebugging);
                if(pages > 0) isEmpty = false;
                Debug.Log("There is "+pages+" Pages in EncyclopediAudio.");
                if (!isEmpty)
                {
                    foreach (KeyValuePair<GameObject,Dictionary<EventReference,AudioInstance>> page in _encyclopediaAudio)
                    {
                        int count = CountAudioInstance(page.Key);
                        Debug.Log("There is "+count+" Event in the Page "+page.Key.name +" in EncyclopediAudio.");
                    }
                }
            }
            return isEmpty;
        }
        
        
        public void CleanUpSound(bool isDebbugging = false)
        {//The method return nothing but clean the whole EncyclopediAudio, by releasing and removing all the AudioInstance from it. Can be used as a debug
            CountAll(isDebbugging);
            foreach (KeyValuePair<GameObject,Dictionary<EventReference,AudioInstance>> page in _encyclopediaAudio)
            {
                foreach (KeyValuePair<EventReference, AudioInstance> line in page.Value)
                {
                    AudioInstance audioInstance = line.Value;
                    if(audioInstance.IsPlaying()) audioInstance.Stop(true);
                    ReleaseAudioInstance(audioInstance,isDebbugging);
                    if(isDebbugging) Debug.Log("AudioInstance SUCCESSFULLY RELEASE at Page "+page.Key.name+", on Event "+line.Key+"." );
                }
                if(isDebbugging) Debug.Log("Page "+page.Key.name+"SUCCESSFULLY REMOVE from EncyclopediAudio." );
            }

            if (CountAll())
            {
                if(isDebbugging) Debug.Log("EncyclopediAudio SUCCESSFULLY ClEAN");
            }
            else Debug.LogError("A ERROR has occured in the CLEANING of EncyclopediAudio, and it DOES'NT RELEASE all its content.");
            
            _encyclopediaAudio.Clear();
        }
    }
    #endregion
    #region AudioInstance
    public struct AudioInstance
    {
        private EventInstance _instance;         //The EventInstance of the AudioInstance
        private EventReference _reference;       //The EventReference of the AudioInstance
        private EventDescription _description;   //The EventDescription of the AudioInstance
        private GameObject _gameObject;          //The GameObject of the AudioInstance
        
        public EventInstance GetInstance(bool isDebugging = false)
        {//Get the EventInstance from somewhere else than here
            _description.getPath(out string path);
            if(isDebugging) Debug.Log("EventInstance "+path+" SUCCESSFULLY GET from AudioInstance.");
            return _instance;
        }
        private void SetInstance(EventInstance instance,bool isDebugging = false)
        {//Set the EventInstance of this AudioInstance
            _instance = instance;
            _instance.getDescription(out EventDescription description);
            SetDescription(description);
            description.getPath(out string path);
            if (isDebugging) Debug.Log("EventInstance "+path+" SUCCESSFULLY SET from AudioInstance.");
        }

        public EventReference GetReference(bool isDebugging = false)
        {//Get the EventReference from somewhere else than here
            if (isDebugging) Debug.Log("EventReference "+_reference.ToString()+" SUCCESSFULLY GET from AudioInstance.");
            return _reference;
        }
        private void SetReference(EventReference reference,bool isDebugging = false)
        {//Set the EventReference of this AudioInstance
            _reference = reference;
            if (isDebugging) Debug.Log("EventReference "+_reference.ToString()+" SUCCESSFULLY SET from AudioInstance.");
        }

        public EventDescription GetDescription(bool isDebugging = false)
        {//Get the EventDescription from somewhere else than here
            _description.getPath(out string path);
            if (isDebugging) Debug.Log("EventDescription "+path+" SUCCESSFULLY GET from AudioInstance.");
            return _description;
        }
        private void SetDescription(EventDescription description,bool isDebugging = false)
        {//Set the EventReference of this AudioInstance
            _description = description;
            _description.getPath(out string path);
            if (isDebugging) Debug.Log("EventDescription "+path+" SUCCESSFULLY SET from AudioInstance.");
        }

        public GameObject GetKeyObject(bool isDebugging = false)
        {//Get the GameObject from somewhere else than here
            if(isDebugging) Debug.Log("GameObject "+_gameObject.name+" SUCCESSFULLY GET from AudioInstance.");
            return _gameObject;
        }
        private void SetKeyObject(GameObject keyObject,bool isDebugging = false)
        {//Set the GameOnecject of this AudioInstance
            _gameObject = keyObject;
            if(isDebugging) Debug.Log("GameObject "+_gameObject.name+" SUCCESSFULLY SET from AudioInstance.");
        }
        
        public AudioInstance CreateInstance(GameObject gameObject, EventReference reference,bool isDebugging = false)
        {//The method returns an AudioInstance from a GameObject And a EventReference. Usefull when you create a new instance of a AudioInstance to store it in the EncyclopediAudio
            SetInstance(RuntimeManager.CreateInstance(reference));
            SetReference(reference);
            SetKeyObject(gameObject);
            _description.getPath(out string path);
            if(isDebugging) Debug.Log("AudioInstance SUCCESSFULLY CREATE. Instance = "+path+". Reference = "+_reference.ToString()+". GameObject = "+gameObject.name);
            return this;
        }
        public void ReleaseInstance(bool isDebugging = false)
        {//The method returns nothing but release the AudioInstance by removing every element in it. Usefull when you remove a AudioInstance from the EncyclopediAudio
            if(isDebugging && IsPlaying()) Debug.LogWarning("AudioInstance is about to be released, even if it still playing, assure yourself that the sound can Stop by itself before release it, to avoid memory leaks.");
            _instance.release();
            if(isDebugging) Debug.Log("AudioInstance SUCCESSFULLY RELEASE");
            _reference = default;
            _description = default;
            _gameObject = null;
        }

        public void Play(bool isDebugging = false)
        {//The method retrun nothing but play the sound of the AudioInstance, if it's a 3D sound, it take the position and the rigidbody as a 3D reference point
            _description.is3D(out bool is3D);
            if (is3D)
            {
                if (_gameObject.GetComponent<Rigidbody>() != null)
                {
                    RuntimeManager.AttachInstanceToGameObject(_instance, _gameObject.transform,_gameObject.GetComponent<Rigidbody>());
                    if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" 3D and set based on the Rigidbody of the GameObject.");
                }
                else
                {
                    RuntimeManager.AttachInstanceToGameObject(_instance, _gameObject.transform);
                    if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" 3D and set based on the transform of the GameObject.");
                }
            }
            _instance.start();
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is now playing.");
        }
        public void Stop(bool immediate = false, bool isDebugging = false)
        {//The method retrun nothing but Stop the sound of the AudioInstance. When used, it's possible to choose if the sound should be release instantly or allow it own fade out 
            if (IsPlaying())
            {
                if (immediate)
                {
                    _instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is stopped instantly.");
                }
                else
                {
                    _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is stopping.");
                }
            }
        }

        public float GetActualNamedParameter(string parameterName, bool isDebugging = false)
        {//The method retrun a float that is a actual or aiming value, in case of seek speed using, of a named Parameter of the EventInstance in the AudioInstance 
            _instance.getParameterByName(parameterName,out float value);
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" have a "+value.ToString()+" as final value in the parameter "+parameterName+".");
            return value;
        }
        public float GetFinalNamedParameter(string parameterName, bool isDebugging = false)
        {//The method retrun a float that is final value, which is the actual or aiming value with all effect applied on it like seek speed or velocity on it, of a named Parameter of the EventInstance in the AudioInstance 
            _instance.getParameterByName(parameterName,out float value, out float var);
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" have a "+value.ToString()+" as final value in the parameter "+parameterName+".");
            return var;
        }
        public void SetNamedParameter(string parameterName, float value, bool ignoneSeekSpeed = false, bool isDebugging = false)
        {//The method retrun nothing but set a Named Parameter to a value. when used, it's possible to ignore the seek speed, if necessary 
            _instance.setParameterByName(parameterName, value, ignoneSeekSpeed);
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" have a now "+value.ToString()+" as value in the parameter "+parameterName+".");
        }
        public void Set3Dparameter(bool isDebugging = false)
        {//The method return nothing but set the 3D parameter of a EventInstance on the GameObject of the AudioInstance, [It might be useless but just in case]
            _instance.set3DAttributes(RuntimeUtils.To3DAttributes(_gameObject));
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" now set in 3D to GameObject "+_gameObject.name+".");
        }
        
        
        public STATE GetState(bool isDebugging = false )
        {//The method return the Actual Playing State of the EventInstance
            STATE state = new STATE();
            _instance.getPlaybackState(out PLAYBACK_STATE playState);
            switch (playState)
            {
                case PLAYBACK_STATE.PLAYING:
                    state = STATE.PLAYING;
                    break;
                case PLAYBACK_STATE.SUSTAINING:
                    state = STATE.SUSTAINING;
                    break;
                case PLAYBACK_STATE.STOPPED:
                    state = STATE.STOPPED;
                    break;
                case PLAYBACK_STATE.STARTING:
                    state = STATE.STARTING;
                    break;
                case PLAYBACK_STATE.STOPPING:
                    state = STATE.STOPPING;
                    break;
            }
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is now "+state+".");
            return state;
        }
        public bool IsPlaying(bool isDebugging = false)
        {//The method retrun a bool to express if the EventInstance is playing or about to be played
            if (GetState() == STATE.PLAYING || GetState() == STATE.STARTING)
            {
                if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is now Playing.");
                return true;
            }
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is NOT Playing.");
            return false;
        }
        public bool IsStopped(bool isDebugging = false)
        {//The method return a bool to express if the EventInstance is stopped or about to be stopped
            if (GetState() == STATE.STOPPED || GetState() == STATE.STOPPING)
            {
                if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is now Stopped.");
                return true;
            }
            if(isDebugging) Debug.Log("AudioInstance "+_gameObject.name+", "+_reference.ToString()+" is NOT Stopped.");
            return false;
        }
        
        public bool IsValid(bool isDebugging = false)
        {//The method return a bool to express if the AudioInstance is correctly set up, the debbugging factor is usefull in case of debbugging to tell what missing exactly
            bool instanceIsValid = true;
            bool referenceIsValid = true;
            bool gameObjectIsValid = true;

            if (!_instance.isValid()) instanceIsValid = false;
            if (_reference.IsNull) referenceIsValid = false;
            if (_gameObject == null) gameObjectIsValid = false;

            if (isDebugging)
            {
                if (!instanceIsValid && (referenceIsValid || gameObjectIsValid))
                {
                    if (referenceIsValid && gameObjectIsValid) Debug.LogError("AudioInstance is not valid. Missing EventInstance ON KeyObject " + _gameObject.name + " WITH EventReference "+ _reference.ToString()+".");
                    else if (referenceIsValid) Debug.LogError("AudioInstance is not valid. Missing EventInstance AND KeyObject WITH EventReference"+ _reference.ToString()+".");
                    else Debug.LogError("AudioIntance is not valid. Missing EventInstance AND EventRefence ON KeyObject "+ _gameObject.name+".");
                }
                else if (!referenceIsValid && (gameObjectIsValid || instanceIsValid))
                {
                    _description.getPath(out string path);
                    if (gameObjectIsValid) Debug.Log("AudioInstance is not valid. Missing EventReference ON KeyObject "+ _gameObject.name +" WITH an EventInstance"+path+".");
                    else Debug.LogError("AudioInstance is not valid. Missing EventReference AND KeyObject WITH EventInstance"+ path+".");
                }
                else
                {
                    Debug.LogError("AudioInstance is not valid. Missing all component of the Instance");
                }
            }
            return instanceIsValid && referenceIsValid && gameObjectIsValid;
        }


    }
    #endregion
    #region Enum
    public enum STATE : int
    {//Playing State of an EventInstance, copy of PLAYING_STATE from FMOD System, for easier use
         PLAYING = 0,
        SUSTAINING = 1,
        STOPPED = 2,
        STARTING = 3,
        STOPPING = 4,
    }
    #endregion
    #endregion
}

