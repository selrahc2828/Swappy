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
        [SerializeField] private FMODMusicManager.Biomes biomes = FMODMusicManager.Biomes.None;
        [SerializeField] private FMODMusicManager.Layer layers = FMODMusicManager.Layer.None;

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

            if (When == FMODMusicManager.OnWhat.OnStart )
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
                    SetParameter(biomes , layers);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&& When == FMODMusicManager.OnWhat.OnTriggerEnter)
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
                        SetParameter(biomes, layers);
                    }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && When == FMODMusicManager.OnWhat.OnTriggerExit)
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
                    SetParameter(biomes, layers);
                }
            }
        }

        private void OnDestroy()
        {
            if (When == FMODMusicManager.OnWhat.OnDestroy)
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
                    SetParameter(biomes, layers);
                }
            }
        }
        

        private void SetParameter(FMODMusicManager.Biomes biome, FMODMusicManager.Layer layer)
        {
            float newValueParam = -1;
            float actualValueParam = FMODMusicManager.instance.GetMusicNameParamInstance(_musicInstance, "Layer");
            
            switch (biome)
            {
                case FMODMusicManager.Biomes.Biome1:
                    if (actualValueParam > 6)
                    {
                        FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, "Layer",3,false);
                    }
                    switch (layers)
                    {
                        case FMODMusicManager.Layer.Layer1:
                            newValueParam = 2;
                            break;
                        case FMODMusicManager.Layer.Layer2:
                            newValueParam = 1;
                            break;
                        case FMODMusicManager.Layer.Layer3:
                            newValueParam = 0;
                            break;
                        default:
                            newValueParam = 3;
                            break;
                    }
                    break;
                
                case FMODMusicManager.Biomes.Biome2:
                    if (actualValueParam > 6)
                    {
                        switch (layers)
                        {
                            case FMODMusicManager.Layer.Layer1:
                                newValueParam = 8;
                                break;
                            case FMODMusicManager.Layer.Layer2:
                                newValueParam = 7;
                                break;
                            case FMODMusicManager.Layer.Layer3:
                                newValueParam = 6;
                                break;
                            default:
                                newValueParam = 9;
                                break;
                        }
                    }
                    else
                    {
                        switch (layers)
                        {
                            case FMODMusicManager.Layer.Layer1:
                                newValueParam = 4;
                                break;
                            case FMODMusicManager.Layer.Layer2:
                                newValueParam = 5;
                                break;
                            case FMODMusicManager.Layer.Layer3:
                                newValueParam = 6;
                                break;
                            default:
                                newValueParam = 3;
                                break;
                        }
                    }
                    break;
                
                case FMODMusicManager.Biomes.Biome3:
                    if (actualValueParam < 6)
                    {
                        FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, "Layer",9,false);
                    }
                    switch (layers)
                    {
                        case FMODMusicManager.Layer.Layer1:
                            newValueParam = 10;
                            break;
                        case FMODMusicManager.Layer.Layer2:
                            newValueParam = 11;
                            break;
                        case FMODMusicManager.Layer.Layer3:
                            newValueParam = 12;
                            break;
                        default:
                            newValueParam = 9;
                            break;
                    }
                    break;
            }
            FMODMusicManager.instance.SetMusicNameParamInstance(_musicInstance, "Layer", newValueParam);
        }

    }
    
}

