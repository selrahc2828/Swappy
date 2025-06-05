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
    }
    
    #region AudioSystem
    #region EncyclopediAudio
    public struct EncyclopediAudio
    {
        private Dictionary<GameObject,Dictionary<EventReference,EventInstance>>  EncyclopediaAudio;
        
        private bool CheckAudioInstance(GameObject keyObject, EventReference reference, out EventInstance eventInstance, bool isDebbuging = false)
        {
            if (EncyclopediaAudio.ContainsKey(keyObject))
            {
                if (EncyclopediaAudio[keyObject].ContainsKey(reference))
                {
                    eventInstance = EncyclopediaAudio[keyObject][reference]; 
                    if (isDebbuging) Debug.Log("EventInstance FOUND at "+keyObject.name+", "+ reference);
                    return true;
                }
                else if(isDebbuging) Debug.LogError("EventInstance NOT FOUND in "+keyObject.name+".");
            }
            else if (isDebbuging) Debug.LogError("GameObject NOT FOUND in EncyclopediAudio.");
            eventInstance = default;
            return false;
        }

        public bool CheckAudioInstance(AudioInstance audioInstance, bool isDebbuging = false)
        {
            GameObject keyObject = audioInstance.GetKeyObject();
            EventReference reference = audioInstance.GetReference();
            if (EncyclopediaAudio.ContainsKey(keyObject))
            {
                if (EncyclopediaAudio[keyObject].ContainsKey(reference))
                {
                    if (isDebbuging) Debug.Log("EventInstance FOUND at "+keyObject.name+", "+ reference);
                    return true;
                }
                else if(isDebbuging) Debug.LogError("EventInstance NOT FOUND in "+keyObject.name+".");
            }
            else if (isDebbuging) Debug.LogError("GameObject NOT FOUND in EncyclopediAudio.");
            return false;
        }
        public AudioInstance GetAudioInstance(GameObject keyObject, EventReference reference, bool isDebbuging = false)
        {
            var audioInstance = new AudioInstance();
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                audioInstance.SetUpInstance(keyObject, reference,eventInstance);
                if (isDebbuging) Debug.Log("AudioInstance SUCCESSFULLY GET from EncyclopediAudio in "+ keyObject.name+" at "+ reference.ToString()+".");
            }
            else
            {
                audioInstance = CreateAudioInstance(keyObject, reference);
            }
            return audioInstance;
        }
        public AudioInstance CreateAudioInstance(GameObject keyObject, EventReference reference, bool isDebbuging = false)
        {
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                return GetAudioInstance(keyObject,reference,isDebbuging);
            }
            else
            {
                AudioInstance audioInstance = new AudioInstance();
                audioInstance.CreateInstance(keyObject, reference);
                if (!EncyclopediaAudio.ContainsKey(keyObject))
                {
                    EncyclopediaAudio.Add(keyObject, new Dictionary<EventReference,EventInstance>());
                    if (isDebbuging) Debug.Log("New KeyObject SUCCESSFULLY CREATE in EncyclopediAudio");
                }
                EncyclopediaAudio[keyObject][reference] = audioInstance.GetInstance();
                if (isDebbuging) Debug.Log("AudioInstance SUCCESSFULLY CREATE and ADD in EncyclopediAudio");
                return audioInstance;
            }
        }

        public void ReleaseAudioInstance(GameObject keyObject, EventReference reference, bool isDebbuging = false)
        {
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                eventInstance.getPlaybackState(out PLAYBACK_STATE playbackState);
                if (playbackState != PLAYBACK_STATE.STOPPED || playbackState != PLAYBACK_STATE.STOPPING) Debug.LogWarning("EventInstance is about to be released, even if it still playing, assure yourself that the sound can Stop by itself before release it, to avoid memory leaks.");
                eventInstance.release();
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY RELEASE");
                EncyclopediaAudio[keyObject].Remove(reference);
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (EncyclopediaAudio[keyObject].Count == 0)
                {
                    EncyclopediaAudio.Remove(keyObject);
                    if(isDebbuging) Debug.Log("KeyObject SUCCESSFULLY REMOVE from EncyclopediAudio");
                }
            }
            else
            {
                if(isDebbuging) Debug.LogWarning("EventInstance can't be remove and release from EncyclopediAudio, because it don't exist in it.");
            }
        }

        public void ReleaseAudioInstance(AudioInstance audioInstance, bool isDebbuging = false)
        {
            GameObject keyObject = audioInstance.GetKeyObject();
            EventReference reference = audioInstance.GetReference();
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                EncyclopediaAudio[keyObject].Remove(reference);
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (EncyclopediaAudio[keyObject].Count == 0)
                {
                    EncyclopediaAudio.Remove(keyObject);
                    if(isDebbuging) Debug.Log("KeyObject SUCCESSFULLY REMOVE from EncyclopediAudio");
                }
                if (audioInstance.GetState() != STATE.STOPPED || audioInstance.GetState() != STATE.STOPPING) Debug.LogWarning("EventInstance is about to be released, even if it still playing, assure yourself that the sound can Stop by itself before release it, to avoid memory leaks.");
                eventInstance.release();
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY RELEASE");
            }
            else
            {
                if(isDebbuging) Debug.LogWarning("EventInstance can't be remove and release from EncyclopediAudio, because it don't exist in it.");
            }
        }

        public int CountKeyPage()
        {
            int count = 0;
            foreach (KeyValuePair<GameObject,Dictionary<EventReference,EventInstance>> Page in EncyclopediaAudio)
            {
                count++;
            }
            return count;
        }
        public int CountAudioInstance(GameObject keyObject)
        {
            int count = 0;
            foreach (KeyValuePair<EventReference, EventInstance> Event in EncyclopediaAudio[keyObject])
            {
                count++;
            }
            return count;
        }

        public bool CountAll(bool isDebbuging = false)
        {
            bool isEmpty = true;
            if (isDebbuging)
            {
                int pages = CountKeyPage();
                if(pages > 0) isEmpty = false;
                Debug.Log("There is "+pages+" Pages in EncyclopediAudio.");
                if (!isEmpty)
                {
                    foreach (KeyValuePair<GameObject,Dictionary<EventReference,EventInstance>> Page in EncyclopediaAudio)
                    {
                        int count = CountAudioInstance(Page.Key);
                        Debug.Log("There is "+count+" Event in the Page "+Page.Key.name +" in EncyclopediAudio.");
                    }
                }
            }
            return isEmpty;
        }
        
        
        public void CleanUpSound( bool IsDebbugging = false)
        {
            CountAll(IsDebbugging);
            foreach (KeyValuePair<GameObject,Dictionary<EventReference,EventInstance>> Page in EncyclopediaAudio)
            {
                foreach (KeyValuePair<EventReference, EventInstance> Event in Page.Value)
                {
                    AudioInstance Instance = new AudioInstance();
                    Instance.SetUpInstance(Page.Key, Event.Key, Event.Value);
                    if(Instance.IsPlaying()) Instance.Stop(true);
                    ReleaseAudioInstance(Instance,IsDebbugging);
                    if(IsDebbugging) Debug.Log("EventInstance SUCCESSFULLY RELEASE at Page "+Page.Key.name+", on Event "+Event.Key+"." );
                }
                if(IsDebbugging) Debug.Log("Page "+Page.Key.name+"SUCCESSFULLY REMOVE from EncyclopediAudio." );
            }

            if (CountAll())
            {
                if(IsDebbugging) Debug.Log("EncyclopediAudio SUCCESSFULLY ClEAN");
            }
            else Debug.LogError("A ERROR has occured in the CLEANING of EncyclopediAudio, and it DOES'NT RELEASE all its content.");
            
            EncyclopediaAudio.Clear();
        }
    }
    #endregion
    #region AudioInstance
    public struct AudioInstance
    {
        private EventInstance Instance;
        private EventReference Reference;
        private EventDescription Description;
        private GameObject GameObject;
        
        public EventInstance GetInstance()
        {
            return Instance;
        }
        private void SetInstance(EventInstance instance)
        {
            Instance = instance;
        }

        public EventReference GetReference()
        {
            return Reference;
        }
        private void SetReference(EventReference reference)
        {
            Reference = reference;
        }
        
        public EventDescription GetDescription()
        {return Description;}
        private void SetDescription(EventDescription description)
        {
            Description = description;
        }

        public GameObject GetKeyObject()
        {
            return GameObject;
        }
        private void SetKeyObject(GameObject keyObject)
        {
            GameObject = keyObject;
        }
        
        

        public AudioInstance CreateInstance(GameObject gameObject, EventReference reference)
        {
            AudioInstance audioInstance = new AudioInstance();
            audioInstance.SetInstance(RuntimeManager.CreateInstance(reference));
            audioInstance.SetReference(reference);
            Instance.getDescription(out EventDescription description);
            audioInstance.SetDescription(description);
            audioInstance.SetKeyObject(gameObject);
            return audioInstance;
        }
        public AudioInstance SetUpInstance(GameObject gameObject, EventReference reference,EventInstance eventInstance)
        {
            AudioInstance audioInstance = new AudioInstance();
            audioInstance.SetInstance(eventInstance);
            audioInstance.SetReference(reference);
            Instance.getDescription(out EventDescription description);
            audioInstance.SetDescription(description);
            audioInstance.SetKeyObject(gameObject);
            return audioInstance;
        }
        public void Play(bool isSpacialized = false )
        {
            if (isSpacialized)
            {
                if (GameObject.GetComponent<Rigidbody>() != null)
                {
                    RuntimeManager.AttachInstanceToGameObject(Instance, GameObject.transform,GameObject.GetComponent<Rigidbody>());
                }
                else
                {
                    RuntimeManager.AttachInstanceToGameObject(Instance, GameObject.transform);
                }
            }
            Instance.start();
        }
        

        public void Stop(bool immediate = false)
        {
            if (IsPlaying())
            {
                if (immediate) Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                else Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        public float GetActualNamedParameter(string parameterName)
        {
            Instance.getParameterByName(parameterName,out float value);
            return value;
        }
        public float GetFinalNamedParameter(string parameterName)
        {
            Instance.getParameterByName(parameterName,out float value, out float var);
            return var;
        }
        public void SetNamedParameter(string parameterName, float value, bool ignoneSeekSpeed = false)
        {
            Instance.setParameterByName(parameterName, value, ignoneSeekSpeed);
        }

        public void Set3Dparameter()
        {
            Instance.set3DAttributes(RuntimeUtils.To3DAttributes(GameObject));
        }
        
        
        public STATE GetState(bool isdebbuging = false )
        {
            STATE state = new STATE();
            Instance.getPlaybackState(out PLAYBACK_STATE playState);
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
        {
            if (GetState() == STATE.PLAYING || GetState() == STATE.STARTING) return true;
            return false;
        }
        public bool IsStopped()
        {
            if (GetState() == STATE.STOPPED || GetState() == STATE.STOPPING) return true;
            return false;
        }
     
        
        

        public bool IsValid(bool isDebugging = false)
        {
            bool instanceIsValid = true;
            bool referenceIsValid = true;
            bool gameObjectIsValid = true;

            if (!Instance.isValid()) instanceIsValid = false;
            if (Reference.IsNull) referenceIsValid = false;
            if (GameObject == null) gameObjectIsValid = false;
            if (instanceIsValid && referenceIsValid && gameObjectIsValid) return true;

            if (isDebugging)
            {
                if (!instanceIsValid && (referenceIsValid || gameObjectIsValid))
                {
                    if (referenceIsValid && gameObjectIsValid) Debug.LogError("AudioInstance is not valid. Missing EventInstance ON KeyObject " + GameObject.name + " WITH EventReference "+ Reference.ToString()+".");
                    else if (referenceIsValid && !gameObjectIsValid) Debug.LogError("AudioInstance is not valid. Missing EventInstance AND KeyObject WITH EventReference"+ Reference.ToString()+".");
                    else Debug.LogError("AudioInstance is not valid. Missing EventInstance AND EventRerence ON KeyObject"+GameObject.name+".");
                }
                else if (!referenceIsValid && (gameObjectIsValid || instanceIsValid))
                {
                    Instance.getDescription(out EventDescription description);
                    description.getPath(out string path);
                    if (gameObjectIsValid) Debug.LogError("AudioInstance is not valid. Missing EventReference ON KeyObject " + GameObject.name + " WITH an EventInstance"+ path+".");
                    else Debug.LogError("AudioInstance is not valid. Missing EventReference AND KeyObject WITH EventInstance"+ path+".");
                }
                else
                {
                    Debug.LogError("AudioInstance is not valid. Missing all component of the Instance");
                }
            }
            return false;
        }


    }
    #endregion
    #region Enum
    public enum STATE : int
    {
        PLAYING = 0,
        SUSTAINING = 1,
        STOPPED = 2,
        STARTING = 3,
        STOPPING = 4,
    }
    #endregion
    #endregion
}

