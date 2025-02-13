using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class ImpulseData
    {
        public float impulseTime = 5f;
        public float doubleImpulseTime = 6f;
        public float impulseRange = 10;
        public float impulseForce = 90;
        public bool destroyOnUse = false;
        public bool impulseGradiantForce = false;
        public GameObject impulseFeedback;
        [Tooltip("Si Rigidbody sur lui")]
        public bool applyOnMe = false;
    }
}