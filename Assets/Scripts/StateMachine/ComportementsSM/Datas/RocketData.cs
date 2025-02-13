using UnityEngine;

public partial class ComportementManager
{
    [System.Serializable]
    public class RocketData
    {
        public float rocketForce = 75f;
        public float rocketForceOnPlayer = 75f;
        public float rocketForceWhenGrab = 75f;
        public float rocketOnOffCouldown = 5f;
        public float rocketMaxSpeed = 15;
    }
}