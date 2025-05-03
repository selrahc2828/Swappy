using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    //[System.Serializable]
    public class TransformOffset : ObjectScatterModifier
    {
        public Vector3 offsetPosition;
        public Vector3 offsetRotation;
        public Vector3 offsetScale;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                var point = instances[i];

                Vector3 posOffset = point.worldPosition;
                var scale = point.scale;
                var angles = point.rotationAngle;

                if (offsetRotation.magnitude > 0)
                {
                    angles += offsetRotation;
                }
                if (offsetScale.magnitude > 0)
                {
                    scale += offsetScale;
                }
                if (offsetPosition.magnitude > 0)
                    posOffset += offsetPosition;

                point.worldPosition = posOffset;
                point.scale = scale;
                point.rotationAngle = angles;

                instances[i] = point;
            }
        }
    }
}