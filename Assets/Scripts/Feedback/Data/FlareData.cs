using UnityEngine;
using UnityEngine.Serialization;


public partial class ComportementManager
{
    [System.Serializable]
    public class FlareData //private internal
    {
        public GameObject prefabFlareSlotHand;
        public float speed;
        public AnimationCurve speedCurve;
        public Material matFlareImpulse;
        public Color particleFlareColorImpulse;
        public Material matFlareBounce;
        public Color particleFlareColorBounce;
        public Material matFlareImmuable;
        public Color particleFlareColorImmuable;
        public Material matFlareMagnet;
        public Color particleFlareColorMagnet;
        public Material matFlareRocket;
        public Color particleFlareColorRocket;
    }
}


