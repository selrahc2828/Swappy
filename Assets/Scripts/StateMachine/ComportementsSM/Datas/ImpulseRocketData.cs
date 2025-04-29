using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class ImpulseRocketData
    {
        public float impulseRocketExplosionForce = 6f;
        public float impulseRocketExplosionRange = 4f;
        public float impulseRocketFlyForceOnPlayer = 75f;
        public float timeBetweenImpulses = .5f;
    }
}