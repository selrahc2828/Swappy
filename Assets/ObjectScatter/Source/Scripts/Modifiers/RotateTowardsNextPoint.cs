using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class RotateTowardsNextPoint : ObjectScatterModifier
    {
        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                var point = instances[i];

                var nextPoint = instances[i == 0 ? instances.Count - 1 : i - 1];
                if(!scatter.Path.Loop && i == 0)
                {
                    nextPoint = instances[i + 1];            
                }

                var angles = point.rotationAngle;

                var offsetRotation = Vector3.zero;
                Quaternion lookAt = Quaternion.LookRotation((nextPoint.worldPosition - point.worldPosition), Vector3.up);

                angles.y += lookAt.eulerAngles.y;

                if(!scatter.Path.Loop && i == 0) //temporary hack
                {
                    angles.y += 180f;
                }

                point.rotationAngle = angles;
                instances[i] = point;
            }
        }
    }
}