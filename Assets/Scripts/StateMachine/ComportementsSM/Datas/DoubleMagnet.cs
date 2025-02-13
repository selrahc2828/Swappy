using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class DoubleMagnetData
    {
        public GameObject prefabDoubleMagnetForcefield;
        public Color justePourDiffSimpleMagnet;
        public float doubleMagnetRange = 8f;
        public float doubleMagnetForce = 80f;
        public float doubleMagnetForceOnPlayer = 80f;
        public float doubleMagnetForceWhenGrab = 80f;
    }
}