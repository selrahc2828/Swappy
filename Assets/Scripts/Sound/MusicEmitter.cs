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
        [SerializeField] private FMODMusicManager.MusicAction Action = FMODMusicManager.MusicAction.None;
        [SerializeField] private FMODMusicManager.OnWhat When =FMODMusicManager.OnWhat.None;
        [SerializeField] private FMODMusicManager.Layer layers = FMODMusicManager.Layer.None;
        

        private void Reset()
        {
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<MeshCollider>();
            }
        }

        void Start()
        {
            _musicInstance = FMODMusicManager.instance.GetMusicPlaylistInstance(Music);

            if (When == FMODMusicManager.OnWhat.OnStart )
            {
                if (Action == FMODMusicManager.MusicAction.Play)
                {
                    SetParameter(layers);
                    FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Stop)
                {
                    FMODMusicManager.instance.StopMusic(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Switch)
                {
                    SetParameter(layers);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&& When == FMODMusicManager.OnWhat.OnTriggerEnter)
            {
                    if (Action == FMODMusicManager.MusicAction.Play)
                    {
                        SetParameter(layers);
                        FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                    }
                    else if (Action == FMODMusicManager.MusicAction.Stop)
                    {
                        FMODMusicManager.instance.StopMusic(_musicInstance);
                    }
                    else if (Action == FMODMusicManager.MusicAction.Switch)
                    {
                        SetParameter( layers);
                    }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && When == FMODMusicManager.OnWhat.OnTriggerExit)
            {
                if (Action == FMODMusicManager.MusicAction.Play)
                {
                    SetParameter(layers);
                    FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                }

                if (Action == FMODMusicManager.MusicAction.Stop)
                {
                    FMODMusicManager.instance.StopMusic(_musicInstance);
                }

                if (Action == FMODMusicManager.MusicAction.Switch)
                {
                    SetParameter(layers);
                }
            }
        }

        private void OnDestroy()
        {
            if (When == FMODMusicManager.OnWhat.OnDestroy)
            {
                if (Action == FMODMusicManager.MusicAction.Play)
                {
                    SetParameter(layers);
                    FMODMusicManager.instance.PlayMusicInstance(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Stop)
                {
                    FMODMusicManager.instance.StopMusic(_musicInstance);
                }
                else if (Action == FMODMusicManager.MusicAction.Switch)
                {
                    SetParameter(layers);
                }
            }
        }
        

        private void SetParameter( FMODMusicManager.Layer layer)
        {
            float newValueParam = -1;
            float actualValueParam = FMODMusicManager.instance.GetMusicNameParamInstance(_musicInstance, "Layer");

            switch (layer)
            {
                case FMODMusicManager.Layer.Layer1:
                    newValueParam = 1;
                    break;
                case FMODMusicManager.Layer.Layer2:
                    newValueParam = 2;
                    break;
                case FMODMusicManager.Layer.Layer3:
                    newValueParam = 3;
                    break;
                case FMODMusicManager.Layer.None:
                    newValueParam = 0;
                    break;
            }

            FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, "Layer", newValueParam);
        }

    }
    
}

