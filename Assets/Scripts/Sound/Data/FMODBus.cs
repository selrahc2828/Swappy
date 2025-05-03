using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

[CreateAssetMenu(fileName = "SOBus", menuName = "FMod/Bus")]
public class FMODBus : ScriptableObject
{
    [field: SerializeField] public string busMaster { get; private set; } = "bus:/";
    [field: SerializeField] public string busPlayer { get; private set; } = "bus:/Player";
    [field: SerializeField] public string busSystem { get; private set; } = "bus:/System";
    [field: SerializeField] public string busMusic { get; private set; } = "bus:/Music";
    [field: SerializeField] public string busMenu { get; private set; } = "bus:/Menu";
    
}
