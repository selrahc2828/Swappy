using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class ImpulseBounceData
    {
        public float impulseBounceForce = 80f;
        public float impulseBounceRange = 10f;
        public float impulseBounceTimer = .3f;
        [Tooltip("Ajoute x% de la velocité au moment de la collision et l'ajoute à impulse bounce Force")]
        public float impulseForceMultiplier = 3f;
    }
}