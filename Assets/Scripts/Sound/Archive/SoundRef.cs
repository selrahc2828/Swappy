using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundRef : MonoBehaviour
{
    #region Player
    public string slowTime {  get; private set; } = "event:/Player/Time/slowTime";
    public string unslowTime { get; private set; } = "event:/Player/Time/unslowTime";
    public string steal { get; private set; } = "event:/Player/Comp/stealComp";
    public string give { get; private set; } = "event:/Player/Comp/giveComp";
    public string projectionEnter { get; private set; } = "event:/Player/Projection/projectionEnter";
    public string projectionStay { get; private set; } = "event:/Player/Projection/projectionStay";
    public string projectionExit { get; private set; } = "event:/Player/Projection/projectionExit";
    public string footstep { get; private set; } = "event:/Player/Moving/Footstep";
    public string jump { get; private set; } = "event:/Player/Moving/Jump";
    public string land { get; private set; } = "event:/Player/Moving/Land";
    #endregion




}
