using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SOMusicEvents", menuName = "FMod/Music")]
public class FMODMusicEvents : ScriptableObject
{
    [field: Header("Biomes1")]
    [field: SerializeField] public EventReference TestMusic1 { get; private set; }
    [field: SerializeField] public EventReference TestMusic2 { get; private set; }
    
    [field: Header("Biomes2")]
    [field: SerializeField] public EventReference TestMusic3 { get; private set; }
    [field: SerializeField] public EventReference TestMusic4 { get; private set; }
    
    [field: Header("Walkman")]
    [field: SerializeField] public EventReference TestMusic5 { get; private set; }
    [field: SerializeField] public EventReference TestMusic6 { get; private set; }


}