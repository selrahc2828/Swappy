using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace FMODUnity
{
    public class emitter : MonoBehaviour
    {
        public EventReference Music;
        public FMODMusicManager.MusicAction Action = FMODMusicManager.MusicAction.None;
        public List<FMOD.Studio.EventInstance> activeMusic = new List<FMOD.Studio.EventInstance>();


    }
}

