using System;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;


public class FMODMusicManager : MonoBehaviour
{
    #region Init
    public static FMODMusicManager instance { get; private set; }

    public FMODMusicEvents FMODMusicEvents;
    public FMODSnapshotEvents FMODSnapshotEvents;

    //private EventInstance musicInstance;
    
    private List<EventInstance> musicPlaylist = new List<EventInstance>();
    
    private PLAYBACK_STATE musicState;


    private EventInstance musictest;
    private bool ismusichere;
    
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
    #region Param Music Instance
    public EventInstance CreateMusicInstance(EventReference musicReference)
    {
        EventInstance musicInstance = RuntimeManager.CreateInstance(musicReference);
        musicPlaylist.Add(musicInstance);
        return musicInstance;
    }

    public void PlayMusicInstance(EventInstance musicInstance)
    {
        musicInstance.getPlaybackState(out musicState);
        if (musicState != PLAYBACK_STATE.PLAYING)
        {
            musicInstance.start();
        }
        else
        {
            Debug.LogWarning("Music instance already playing");
        }
    }

    public float GetMusicNameParamInstance(EventInstance musicInstance, string paramName)
    {
        musicInstance.getParameterByName(paramName, out float value);
        return (value);
    }

    public void SetMusicNameParamInstance(EventInstance musicInstance, string paramName, float value, bool seekSpeed)
    {
        musicInstance.setParameterByName(paramName, value, seekSpeed);
    }

    public void StopMusic(EventInstance musicInstance)
    {
        musicInstance.getPlaybackState(out musicState);
        if (musicState != PLAYBACK_STATE.STOPPED)
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
            musicPlaylist.Remove(musicInstance);
        }
        else
        {
            Debug.LogWarning("No music instance playing");
        }
    }
    #endregion
    #region Param Music Playlist
    public float GetPlaylistMusicSize()
    {
        return musicPlaylist.Count;
    }

    #endregion
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            musictest = CreateMusicInstance(FMODMusicEvents.TestMusic1);
            PlayMusicInstance(musictest);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetMusicNameParamInstance(musictest, "Layer", 1,false);
            Debug.Log(GetMusicNameParamInstance(musictest, "Layer"));
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetMusicNameParamInstance(musictest, "Layer", 2,false);
            Debug.Log(GetMusicNameParamInstance(musictest, "Layer"));
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetMusicNameParamInstance(musictest, "Layer", 3,false);
            Debug.Log(GetMusicNameParamInstance(musictest, "Layer"));
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            StopMusic(musictest);
        }
    }
    

    #region On destroy
    private void CleanUpMusic()
    {
        foreach (EventInstance musicInstance in musicPlaylist)
        {
            musicInstance.stop(STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
        musicPlaylist.Clear();
    }
    
    private void OnDestroy()
    {
        CleanUpMusic();
    }
    #endregion
}