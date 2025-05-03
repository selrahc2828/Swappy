using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public static class Utils
    {
        public static bool LayerMaskContains(int layer, LayerMask lm)
        {
            return (lm == (lm | (1 << layer)));
        }

        public static bool LayerMaskContains(Collider collider, LayerMask lm)
        {
            return LayerMaskContains(collider.gameObject.layer, lm);
        }

        public static bool LayerMaskContains(Collision collision, LayerMask lm)
        {
            return LayerMaskContains(collision.gameObject.layer, lm);
        }

        public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static Vector3 MidPoint(Vector3 a, Vector3 b)
        {
            return new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();  
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}