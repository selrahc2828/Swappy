using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class MagnetBounceData
    {
        public float magnetBounceForce = 60f;
        [Tooltip("Ajoute x% de la velocité au moment de la collision et l'ajoute à magnet Force")]
        public float magnetForceMultiplier = 1.5f;
    
        public float magnetBounceForceOnPlayer = 60f;
        public float magnetForceOnPlayerMultiplier = 1.5f;
    
        public float magnetBounceForceWhenGrab = 60f;
        public float magnetForceWhenGrabMultiplier = 1.5f;

        public float magnetBounceRange = 8f;
        public float intervalBetweenBurst = 2f;
        public Color burstColor;
        [Tooltip("delay pour scale magnet range collision quand grab, sinon trop court avec les rebond qui s'enchaine")]
        public float delayDisplay = .2f;
    }
}