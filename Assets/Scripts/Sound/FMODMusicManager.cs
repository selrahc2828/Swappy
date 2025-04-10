using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    FMOD.Studio.EVENT_CALLBACK walkmanCallback;
    
    private EventInstance musictest;
    
    private EventInstance musicTape;
    
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
    
    void Start()
    {
        // Cr�e explicitement un d�l�gu� pour le callback et le stocke afin d'�viter 
        // qu'il ne soit lib�r� par le garbage collector pendant son utilisation.
        walkmanCallback = new FMOD.Studio.EVENT_CALLBACK(WalkmanEventCallback);
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
        if (musicInstance.getPlaybackState(out musicState) != (RESULT)PLAYBACK_STATE.STOPPED)
        {
            musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            Debug.LogWarning("No music instance playing");
        }
    }

    public void ReleaseMusicInstance(EventInstance musicInstance)
    {
        if (musicInstance.getPlaybackState(out musicState) == (RESULT)PLAYBACK_STATE.STOPPED)
        {
            //Debug.Log("Music instance released");
            musicInstance.release();
            musicPlaylist.Remove(musicInstance);
        }
        else
        {
            Debug.LogWarning("Music instance not released");
        }
    }
    #endregion
    #region Param Music Playlist
    public float GetPlaylistMusicSize()
    {
        return musicPlaylist.Count;
    }

    #endregion
    #region Param Music Walkman

    public void ChooseMusicWalkMan(EventInstance musicTape, string musicName)
    {
        musicTape.setUserData(GCHandle.ToIntPtr(GCHandle.Alloc("CassetteJazzFilmNoir")));
        musicTape.setCallback(walkmanCallback);
    }
    #endregion
    #region Param Music Test 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            musictest = CreateMusicInstance(FMODMusicEvents.TestMusic1);
            PlayMusicInstance(musictest);
            SetMusicNameParamInstance(musictest, "Layer", 1,false);
            Debug.Log(GetMusicNameParamInstance(musictest, "Layer"));
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            StopMusic(musictest);
            ReleaseMusicInstance(musictest);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            musicTape = CreateMusicInstance(FMODMusicEvents.TestMusic5);
            ChooseMusicWalkMan(musicTape, "bounce");
            PlayMusicInstance(musicTape);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            StopMusic(musicTape);
            ChooseMusicWalkMan(musicTape, "bounce");
            PlayMusicInstance(musicTape);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            StopMusic(musicTape);
            ReleaseMusicInstance(musicTape);
        }
     
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            Debug.Log(GetPlaylistMusicSize());
        }
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
    
    
    
    #region oui
    static FMOD.RESULT WalkmanEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        // R�cup�re l'instance d'�v�nement associ�e au pointeur donn�.
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // R�cup�re les donn�es utilisateur associ�es � cet �v�nement (pointeur vers la cl�).
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);

        // Convertit le pointeur en un objet C# (ici une cl� sous forme de cha�ne).
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        string key = stringHandle.Target as string;

        // G�re les diff�rents types de callbacks FMOD.
        switch (type)
        {
            // Cas o� un son doit �tre cr�� dynamiquement.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                {
                    // D�finit les modes pour cr�er un son (par ex., en boucle, non bloquant).
                    FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;

                    // R�cup�re les param�tres du son � partir du pointeur fourni.
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                    // V�rifie si la cl� contient une extension de fichier (exemple : "audio.mp3").
                    if (key.Contains("."))
                    {
                        // Cr�e un son � partir d'un fichier dans le dossier StreamingAssets.
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);

                        if (soundResult == FMOD.RESULT.OK)
                        {
                            // Associe le son � l'�v�nement et met � jour les param�tres.
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = -1; // Pas de sous-son utilis� ici.
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    else
                    {
                        // Cas o� la cl� fait r�f�rence � une entr�e dans la table audio.
                        FMOD.Studio.SOUND_INFO dialogueSoundInfo;
                        var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);

                        if (keyResult != FMOD.RESULT.OK)
                        {
                            break; // Quitte si la cl� est invalide.
                        }

                        // Cr�e un son � partir des informations de la table audio.
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);

                        if (soundResult == FMOD.RESULT.OK)
                        {
                            // Associe le son et ses informations � l'�v�nement.
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    break;
                }

            // Cas o� un son programm� doit �tre d�truit.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release(); // Lib�re les ressources associ�es au son.
                    break;
                }

            // Cas o� l'�v�nement est d�truit : lib�re la m�moire associ�e � la cl�.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    stringHandle.Free(); // Lib�re la m�moire verrouill�e pour la cl�.
                    break;
                }
        }

        // Retourne le r�sultat OK pour indiquer que le callback a r�ussi.
        return FMOD.RESULT.OK;
    }
    #endregion
}