using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class MagnetRocketData
    {
        public GameObject prefabMagnetRocketForcefield;
        public float magnetRocketFlyTime = 4;
        public float rocketMagnetForce = 7f;
        public float rocketMagnetForceOnPlayer = 7f;
        public float rocketMagnetForceWhenGrab = 7f;
        public float magnetTrailForce = 5f;
        public float magnetTrailLerp = 1.5f;
        public float magnetTrailTimeBeforeMove = 5f;
    }
}