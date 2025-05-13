using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class MagnetData
    {
        public float magnetRange = 8f;
        public float magnetForce = 8f;
        public float equilibriumDistance = 2f;
        public float dampingFactor = 2f;
    }
}