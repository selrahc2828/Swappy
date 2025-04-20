using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class RocketData
    {
        public float rocketForce = 75f;
        public float rocketForceOnPlayer = 75f;
        public float rocketForceWhenGrab = 75f;
        public float rocketOnCooldown = 5f;
        public float rocketOffCooldown = 6f;
        public float rocketMaxSpeed = 15;
    }
}