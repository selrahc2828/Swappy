using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundRef : MonoBehaviour
{
    #region Player
    public string slowTime {  get; private set; } = "event:/Player/Time/SlowTime";
    public string unslowTime { get; private set; } = "event:/Player/Time/UnslowTime";
    public string steal { get; private set; } = "event:/Player/Comp/StealComp";
    public string give { get; private set; } = "event:/Player/Comp/GiveComp";
    public string projectionEnter { get; private set; } = "event:/Player/Projection/projectionEnter";
    public string projectionStay { get; private set; } = "event:/Player/Projection/projectionStay";
    public string projectionExit { get; private set; } = "event:/Player/Projection/projectionExit";
    public string footstep { get; private set; } = "event:/Player/Moving/Footstep";
    public string jump { get; private set; } = "event:/Player/Moving/Jump";
    public string land { get; private set; } = "event:/Player/Moving/Land";
    #endregion
    #region System
    public string collision { get; private set; } = "event:/System/Collision";
    public string placeComp { get; private set; } = "event:/System/PLace";
    public string repulseTimer { get; private set; } = "event:/System/Componenent/RepulseTimer";
    public string repulseBoom { get; private set; } = "event:/System/Componenent/RepulseBoom";
    public string immuableHit { get; private set; } = "event:/System/Componenent/ImmuableHit";
    public string bounceHit { get; private set; } = "event:/System/Componenent/ImmuableHit";
    public string propelerStart { get; private set; } = "event:/System/Componenent/PropelerStart";
    public string aimantStart { get; private set; } = "event:/System/Componenent/AimantStart";
    #endregion
    #region Bus
    public string busPlayer { get; private set; } = "bus:/Player";
    public string busSystem { get; private set; } = "bank:/System";
    #endregion



}
