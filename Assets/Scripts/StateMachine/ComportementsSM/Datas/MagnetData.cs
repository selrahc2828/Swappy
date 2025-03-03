using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class MagnetData
    {
        public float magnetRange = 8f;
        public float magnetForce = 8f;
        [HideInInspector] public bool magnetGradiantForce;
    }
}