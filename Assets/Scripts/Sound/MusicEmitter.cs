using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


namespace FMODUnity
{
    public class MusicEmitter : MonoBehaviour
    {
        public EventReference Music;
        private EventInstance _musicInstance;
        public FMODMusicManager.MusicAction Action = FMODMusicManager.MusicAction.None;
        public OnWhat When;
        [SerializeField] private string musicNameParameter;
        [SerializeField] private int musicValueParameter;

        private void Reset()
        {
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
        }

        void Start()
        {
            _musicInstance = FMODMusicManager.instance.GetMusicPlaylistInstance(Music);

            if (When == OnWhat.OnStart )
            {
                if (Action == FMODMusicManager.MusicAction.Play)
                {
                    FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Stop)
                {
                    FMODMusicManager.instance.StopMusic(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Switch)
                {
                    FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, musicNameParameter,musicValueParameter);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&& When == OnWhat.OnTriggerEnter)
            {
                    if (Action == FMODMusicManager.MusicAction.Play)
                    {
                        FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                    }
                    else if (Action == FMODMusicManager.MusicAction.Stop)
                    {
                        FMODMusicManager.instance.StopMusic(_musicInstance);
                    }
                    else if (Action == FMODMusicManager.MusicAction.Switch)
                    {
                        FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, musicNameParameter,musicValueParameter);
                    }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && When == OnWhat.OnTriggerExit)
            {
                if (Action == FMODMusicManager.MusicAction.Play)
                {
                    FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                }

                if (Action == FMODMusicManager.MusicAction.Stop)
                {
                    FMODMusicManager.instance.StopMusic(_musicInstance);
                }

                if (Action == FMODMusicManager.MusicAction.Switch)
                {
                    FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, musicNameParameter,musicValueParameter);
                }
            }
        }

        private void OnDestroy()
        {
            if (When == OnWhat.OnDestroy)
            {
                if (Action == FMODMusicManager.MusicAction.Play)
                {
                    FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Stop)
                {
                    FMODMusicManager.instance.StopMusic(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Switch)
                {
                    FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, musicNameParameter,musicValueParameter);
                }
            }
        }


        public enum OnWhat : int
        {
            None,
            OnStart,
            OnDestroy,
            OnTriggerEnter,
            OnTriggerExit,
        }
    }
    
}

