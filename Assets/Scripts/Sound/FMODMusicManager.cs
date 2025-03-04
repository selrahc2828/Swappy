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

    private EventInstance musicInstance;
    
    private List<EventInstance> musicPlaylist = new List<EventInstance>();
    
    private PLAYBACK_STATE musicState;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region Param Music Instance
    public void CreateMusicInstance(EventReference musicReference)
    {
        EventInstance musicInstance = RuntimeManager.CreateInstance(musicReference);
        musicPlaylist.Add(musicInstance);
        musicInstance.start();
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

    public float GetMusicNameParamInstance(EventInstance musicInstance, string name)
    {
        return (float)musicInstance.getParameterByName(name, out float value);
    }

    public void SetMusicNameParamInstance(EventInstance musicInstance, string name, float value)
    {
        musicInstance.setParameterByName(name, value);
    }

    public void StopMusic()
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