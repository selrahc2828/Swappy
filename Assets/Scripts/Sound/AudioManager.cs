using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace FMODUnity
{
    public class AudioManager : MonoBehaviour
    {
        public Dictionary<GameObject,Dictionary<EventReference,EventInstance>>  EncyclopediAudio = new Dictionary<GameObject,Dictionary<EventReference,EventInstance>>();
        private bool CheckAudioInstance(GameObject keyObject, EventReference reference, out EventInstance eventInstance, bool isDebbuging = false)
        {
            if (EncyclopediAudio.ContainsKey(keyObject))
            {
                if (EncyclopediAudio[keyObject].ContainsKey(reference))
                {
                    eventInstance = EncyclopediAudio[keyObject][reference]; 
                    if (isDebbuging) Debug.Log("EventInstance FOUND at "+gameObject.name+", "+ reference);
                    return true;
                }
                else if(isDebbuging) Debug.LogError("EventInstance NOT FOUND in "+gameObject.name+".");
            }
            else if (isDebbuging) Debug.LogError("GameObject NOT FOUND in EncyclopediAudio.");
            eventInstance = default;
            return false;
        }

        public bool CheckAudioInstance(AudioInstance audioInstance,bool isDebbuging = false)
        {
            GameObject keyObject = audioInstance.GetKeyObject();
            EventReference reference = audioInstance.GetReference();
            if (EncyclopediAudio.ContainsKey(keyObject))
            {
                if (EncyclopediAudio[keyObject].ContainsKey(reference))
                {
                    if (isDebbuging) Debug.Log("EventInstance FOUND at "+gameObject.name+", "+ reference);
                    return true;
                }
                else if(isDebbuging) Debug.LogError("EventInstance NOT FOUND in "+gameObject.name+".");
            }
            else if (isDebbuging) Debug.LogError("GameObject NOT FOUND in EncyclopediAudio.");
            return false;
        }
        public AudioInstance GetAudioInstance(GameObject keyObject, EventReference reference, bool isDebbuging = false)
        {
            var audioInstance = new AudioInstance();
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                audioInstance.SetInstance(eventInstance);
                audioInstance.SetReference(reference);
                audioInstance.SetKeyObject(keyObject);
                if (isDebbuging) Debug.Log("AudioInstance SUCCESSFULLY GET from EncyclopediAudio in "+ keyObject.name+" at "+ reference.ToString()+".");
            }
            else
            {
                audioInstance = CreateAudioInstance(keyObject, reference);
            }
            return audioInstance;
        }
        public AudioInstance CreateAudioInstance(GameObject keyObject,EventReference reference, bool isDebbuging = false)
        {
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                return GetAudioInstance(keyObject,reference,isDebbuging);
            }
            else
            {
                AudioInstance newinstace = new AudioInstance();
                eventInstance = RuntimeManager.CreateInstance(reference);
                newinstace.SetInstance(eventInstance);
                newinstace.SetReference(reference);
                newinstace.SetKeyObject(keyObject);
                if (!EncyclopediAudio.ContainsKey(keyObject)) EncyclopediAudio.Add(keyObject, new Dictionary<EventReference,EventInstance>());
                EncyclopediAudio[keyObject][reference] = eventInstance;
                if (isDebbuging) Debug.Log("AudioInstance SUCCESSFULLY CREATE in EncyclopediAudio");
                return newinstace;
            }
        }

        public void ReleaseAudioInstance(GameObject keyObject, EventReference reference, bool isDebbuging = false)
        {
            if (CheckAudioInstance(keyObject, reference, out EventInstance eventInstance))
            {
                EncyclopediAudio[keyObject].Remove(reference);
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (EncyclopediAudio[keyObject].Count == 0)
                {
                    EncyclopediAudio.Remove(keyObject);
                    if(isDebbuging) Debug.Log("KeyObject SUCCESSFULLY REMOVE from EncyclopediAudio");
                }
                eventInstance.getPlaybackState(out PLAYBACK_STATE playbackState);
                if (playbackState != PLAYBACK_STATE.STOPPED || playbackState != PLAYBACK_STATE.STOPPING) Debug.LogWarning("EventInstance is about to be released, even if it still playing, assure yourself that the sound can Stop by itself before release it, to avoid memory leaks.");
                eventInstance.release();
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY RELEASE");
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
                EncyclopediAudio[keyObject].Remove(reference);
                if(isDebbuging) Debug.Log("EventInstance SUCCESSFULLY REMOVE from EncyclopediAudio");
                if (EncyclopediAudio[keyObject].Count == 0)
                {
                    EncyclopediAudio.Remove(keyObject);
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
        
    }

    
    
    
    public struct AudioInstance
    {
        private EventInstance Instance;
        private EventReference Reference;
        private GameObject GameObject;

        public EventInstance GetInstance()
        {
            return Instance;
        }
        public void SetInstance(EventInstance instance)
        {
            Instance = instance;
        }

        public EventReference GetReference()
        {
            return Reference;
        }
        public void SetReference(EventReference reference)
        {
            Reference = reference;
        }

        public GameObject GetKeyObject()
        {
            return GameObject;
        }
        public void SetKeyObject(GameObject keyObject)
        {
            GameObject = keyObject;
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
    public enum STATE : int
    {
        PLAYING = 0,
        SUSTAINING = 1,
        STOPPED = 2,
        STARTING = 3,
        STOPPING = 4,
    }
}

