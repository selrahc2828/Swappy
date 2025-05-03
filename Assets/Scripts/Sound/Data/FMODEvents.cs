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
    [field: SerializeField] public EventReference PlayerSelfSwitch { get; private set; }
    [field: SerializeField] public EventReference PlayerSelfImpactMode { get; private set; }
    [field: Header("SFX Comp")]
    [field: SerializeField] public EventReference Impulse { get; private set; }
    [field: SerializeField] public EventReference Bounce { get; private set; }
    [field: SerializeField] public EventReference Immuable { get; private set; }
    [field: SerializeField] public EventReference Magnet { get; private set; }
    [field: SerializeField] public EventReference Rocket { get; private set; }
    [field: SerializeField] public EventReference DoubleImpulse { get; private set; }
    [field: SerializeField] public EventReference DoubleBounce { get; private set; }
    [field: SerializeField] public EventReference DoubleImmuable{ get; private set; }
    [field: SerializeField] public EventReference DoubleMagnet { get; private set; }
    [field: SerializeField] public EventReference DoubleRocket { get; private set; }
    [field: SerializeField] public EventReference ImpulseBounce { get; private set; }
    [field: SerializeField] public EventReference ImpulseImmuable { get; private set; }
    [field: SerializeField] public EventReference ImpulseMagnet { get; private set; }
    [field: SerializeField] public EventReference ImpulseRocket { get; private set; }
    [field: SerializeField] public EventReference BounceImmuable { get; private set; }
    [field: SerializeField] public EventReference BounceRocket { get; private set; }
    [field: SerializeField] public EventReference BounceMagnet { get; private set; }
    [field: SerializeField] public EventReference ImmuableRocket { get; private set; }
    [field: SerializeField] public EventReference ImmuableMagnet { get; private set; }
    [field: SerializeField] public EventReference MagnetRocket { get; private set; }
    [field: Space]
    [field: SerializeField] public EventReference Collision { get; private set; }
    [field: Space]
    [field: Space]
    [field: Header("Menu")]
    [field: SerializeField] public EventReference Hover { get; private set; }
    [field: SerializeField] public EventReference Validated { get; private set; }
    [field: SerializeField] public EventReference Back { get; private set; }

}