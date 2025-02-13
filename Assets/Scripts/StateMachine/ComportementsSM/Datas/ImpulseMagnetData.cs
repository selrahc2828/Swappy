using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class ImpulseMagnetData
    {
        public float zoneImpulseRange = 8f;
        public float zoneImpulseForce = 25f;
        public GameObject prefabImpulseMagnet;
    }
}