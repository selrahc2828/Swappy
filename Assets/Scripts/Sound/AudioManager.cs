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

        private void OnDestroy()
        {
            EncyclopediAudio.CleanUpSound();
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
    }
    
    #region AudioSystem
    #region EncyclopediAudio
    public struct EncyclopediAudio
    {
        private Dictionary<GameObject,Dictionary<EventReference,AudioInstance>> _encyclopediaAudio;
        
        private bool CheckAudioInstance(GameObject keyObject, EventReference reference, out AudioInstance audioInstance, bool isDebugging = false)
        {//The Method return a bool to express if an AudioInstance exist on the KeyObject and with the EventReference, but also return the AudioInstance. Can be use as a debbug.
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
        {
            GameObject keyObject = audioInstance.GetKeyObject();
            EventReference reference = audioInstance.GetReference();
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
        {
            if (CheckAudioInstance(keyObject, reference, out AudioInstance audioInstance))
            {
                audioInstance = _encyclopediaAudio[keyObject][reference];
                if (isDebugging) Debug.Log("AudioInstance SUCCESSFULLY GET from EncyclopediAudio in "+ keyObject.name+" at "+ reference.ToString()+".");
            }
            else
            {
                audioInstance = CreateAudioInstance(keyObject, reference);
            }
            return audioInstance;
        }
        public AudioInstance CreateAudioInstance(GameObject keyObject, EventReference reference, bool isDebugging = false)
        {
            if (CheckAudioInstance(keyObject, reference, out AudioInstance audioInstance))
            {
                return audioInstance;
            }
            else
            {
                audioInstance.CreateInstance(keyObject, reference);
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
        {
            if (CheckAudioInstance(keyObject, reference, out AudioInstance audioInstance))
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
        {
            if (CheckAudioInstance(audioInstance,isDebugging))
            {
                audioInstance.GetInstance().release();
                _encyclopediaAudio[audioInstance.GetKeyObject()].Remove(audioInstance.GetReference());
                if(isDebugging) Debug.Log("AudioInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (_encyclopediaAudio[audioInstance.GetKeyObject()].Count == 0)
                {
                    _encyclopediaAudio.Remove(audioInstance.GetKeyObject());
                    if(isDebugging) Debug.Log("KeyObject SUCCESSFULLY REMOVE from EncyclopediAudio");
                }
            }
            else
            {
                if(isDebugging) Debug.LogWarning("AudioInstance can't be remove and release from EncyclopediAudio, because it don't exist in it.");
            }
        }

        public int CountKeyPage()
        {
            int count = 0;
            foreach (KeyValuePair<GameObject,Dictionary<EventReference,AudioInstance>> page in _encyclopediaAudio)
            {
                count++;
            }
            return count;
        }
        public int CountAudioInstance(GameObject keyObject)
        {
            int count = 0;
            foreach (KeyValuePair<EventReference, AudioInstance> @event in _encyclopediaAudio[keyObject])
            {
                count++;
            }
            return count;
        }

        public bool CountAll(bool isDebugging = false)
        {
            bool isEmpty = true;
            if (isDebugging)
            {
                int pages = CountKeyPage();
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
        {
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
        
        public EventInstance GetInstance()
        {//Get the EventInstance from somewhere else than here
            return _instance;
        }
        private void SetInstance(EventInstance instance)
        {//Set the EventInstance of this AudioInstance
            _instance = instance;
        }

        public EventReference GetReference()
        {//Get the EventReference from somewhere else than here
            return _reference;
        }
        private void SetReference(EventReference reference)
        {//Set the EventReference of this AudioInstance
            _reference = reference;
        }

        public EventDescription GetDescription()
        {//Get the EventDescription from somewhere else than here
            return _description;
        }
        private void SetDescription(EventDescription description)
        {//Set the EventReference of this AudioInstance
            _description = description;
        }

        public GameObject GetKeyObject()
        {//Get the GameObject from somewhere else than here
            return _gameObject;
        }
        private void SetKeyObject(GameObject keyObject)
        {//Set the GameOnecject of this AudioInstance
            _gameObject = keyObject;
        }
        
        public AudioInstance CreateInstance(GameObject gameObject, EventReference reference)
        {//The method retrun an AudioInstance from a GameObject And a EventReference. Usefull when you create a new instance of a EventInstance to store it in the EncyclopediAudio
            SetInstance(RuntimeManager.CreateInstance(reference));
            SetReference(reference);
            _instance.getDescription(out EventDescription description);
            SetDescription(description);
            SetKeyObject(gameObject);
            return this;
        }
        public void ReleaseInstance(bool isDebugging = false)
        {
            if(isDebugging && IsPlaying()) Debug.LogWarning("AudioInstance is about to be released, even if it still playing, assure yourself that the sound can Stop by itself before release it, to avoid memory leaks.");
            _instance.release();
            if(isDebugging) Debug.Log("AudioInstance SUCCESSFULLY RELEASE");
            _reference = default;
            _description = default;
            _gameObject = null;
        }

        public void Play()
        {//The method retrun nothing but play the sound of the AudioInstance, if it's a 3D sound, it take the position and the rigidbody as a 3D reference point
            _description.is3D(out bool is3D);
            if (is3D)
            {
                if (_gameObject.GetComponent<Rigidbody>() != null)
                {
                    RuntimeManager.AttachInstanceToGameObject(_instance, _gameObject.transform,_gameObject.GetComponent<Rigidbody>());
                }
                else
                {
                    RuntimeManager.AttachInstanceToGameObject(_instance, _gameObject.transform);
                }
            }
            _instance.start();
        }
        public void Stop(bool immediate = false)
        {//The method retrun nothing but Stop the sound of the AudioInstance. When used, it's possible to choose if the sound should be release instantly or allow it own fade out 
            if (IsPlaying())
            {
                if (immediate) _instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                else _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        public float GetActualNamedParameter(string parameterName)
        {//The method retrun a float that is a actual or aiming value, in case of seek speed using, of a named Parameter of the EventInstance in the AudioInstance 
            _instance.getParameterByName(parameterName,out float value);
            return value;
        }
        public float GetFinalNamedParameter(string parameterName)
        {//The method retrun a float that is final value, which is the actual or aiming value with all effect applied on it like seek speed or velocity on it, of a named Parameter of the EventInstance in the AudioInstance 
            _instance.getParameterByName(parameterName,out float value, out float var);
            return var;
        }
        public void SetNamedParameter(string parameterName, float value, bool ignoneSeekSpeed = false)
        {//The method retrun nothing but set a Named Parameter to a value. when used, it's possible to ignore the seek speed, if necessary 
            _instance.setParameterByName(parameterName, value, ignoneSeekSpeed);
        }
        public void Set3Dparameter()
        {//The method return nothing but set the 3D parameter of a EventInstance on the GameObject of the AudioInstance, [It might be useless but just in case]
            _instance.set3DAttributes(RuntimeUtils.To3DAttributes(_gameObject));
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
            return state;
        }
        public bool IsPlaying()
        {//The method retrun a bool to express if the EventInstance is playing or about to be played
            if (GetState() == STATE.PLAYING || GetState() == STATE.STARTING) return true;
            return false;
        }
        public bool IsStopped()
        {//The method return a bool to express if the EventInstance is stopped or about to be stopped
            if (GetState() == STATE.STOPPED || GetState() == STATE.STOPPING) return true;
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

