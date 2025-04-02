using FMODUnity;
using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu(fileName = "SOEvents", menuName = "FMod/Events")]
public class FMODEvents : ScriptableObject
{
    [field: Header("SFX Player")]
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }
    [field: SerializeField] public EventReference PlayerJump { get; private set; }
    [field: SerializeField] public EventReference PlayerFall { get; private set; }
    [field: SerializeField] public EventReference PlayerLand { get; private set; }
    [field: SerializeField] public EventReference PlayerStealComp { get; private set; }
    [field: SerializeField] public EventReference PlayerGiveComp { get; private set; }
    [field: SerializeField] public EventReference PlayerSelfStealComp { get; private set; }
    [field: SerializeField] public EventReference PlayerSelfGiveComp { get; private set; }
    [field: Header("SFX Comp")]
    [field: SerializeField] public EventReference Impulse { get; private set; }
    [field: SerializeField] public EventReference Bounce { get; private set; }
    [field: SerializeField] public EventReference Immuable { get; private set; }
    [field: SerializeField] public EventReference Rocket { get; private set; }
    [field: SerializeField] public EventReference Magnet { get; private set; }
    [field: Space]
    [field: Space]
    [field: Space]
    [field: Space]
    [field: SerializeField] public EventReference DoubleImpulseBoom { get; private set; }
    [field: SerializeField] public EventReference ImpulseBounceHit { get; private set; }
    [field: SerializeField] public EventReference ImpulseImmuableHit { get; private set; }
    [field: SerializeField] public EventReference ImpulseRocketStart { get; private set; }
    [field: SerializeField] public EventReference ImpulseRocketBoom { get; private set; }
    [field: SerializeField] public EventReference ImpulseMagnetStart { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference DoubleBounceHit { get; private set; }
    [field: SerializeField] public EventReference BounceImmuableHit { get; private set; }
    [field: SerializeField] public EventReference BounceRocketHit { get; private set; }
    [field: SerializeField] public EventReference BounceRocketStart { get; private set; }
    [field: SerializeField] public EventReference BounceMagnetHit { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference DoubleImmuableHit { get; private set; }
    [field: SerializeField] public EventReference ImmuableRocketHit { get; private set; }
    [field: SerializeField] public EventReference ImmuableMagnetStart { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference DoubleRocketStart { get; private set; }
    [field: SerializeField] public EventReference RocketMagnetStart { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference DoubleMagnet { get; private set; }
    [field: Space]
    [field: Space]
    [field: Space]
    [field: Header("Menu")]
    [field: SerializeField] public EventReference Hover { get; private set; }
    [field: SerializeField] public EventReference Validated { get; private set; }
    [field: SerializeField] public EventReference Back { get; private set; }

}