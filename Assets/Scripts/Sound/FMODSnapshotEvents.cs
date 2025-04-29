using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSnapshotEvents", menuName = "FMod/Snapshots")]
public class FMODSnapshotEvents : ScriptableObject
{
    [field: Header("Game")]
    [field: SerializeField] public EventReference Snapshot1 { get; private set; }
    [field: Space]
    [field: Space]
    [field: Space]
    [field: Header("Menu")]
    [field: SerializeField] public EventReference Snapshot2 { get; private set; }
    [field: Space]
    [field: Space]
    [field: Space]
    [field: Header("Music")]
    [field: SerializeField] public EventReference WalkMan { get; private set; }

}