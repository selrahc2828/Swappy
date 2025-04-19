using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class ProjectOnColliders : ObjectScatterModifier
    {
        public LayerMask ignoreMask;
        public LayerMask avoidMask;
        public bool followCollisionNormals = false;
        public bool useSlope;
        public float slope;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            List<int> _pointsNotCasted = new List<int>();
            int pointsCount = instances.Count;
            int currentPoint = 0;

            while (currentPoint < pointsCount)
            {
                var point = instances[currentPoint];

                Vector3 position = point.worldPosition;
                var angles = point.rotationAngle;

                RaycastHit hit;
                if (Physics.Raycast((position),
                                    -transform.up, out hit, Mathf.Infinity, ~(ignoreMask)))
                {
                    if (!Utils.LayerMaskContains(hit.collider.gameObject.layer, avoidMask))
                    {
                        // slope process
                        var pointNormal = hit.normal;
                        if (useSlope && Vector3.Angle(pointNormal, Vector3.up) >= slope)
                        {
                            instances.RemoveAt(currentPoint);
                            pointsCount--;
                            continue;
                        }

                        position = hit.point;

                        var rotationTarget = (followCollisionNormals ? Quaternion.FromToRotation(Vector3.up, hit.normal) : Quaternion.identity) *
                            Quaternion.AngleAxis(point.rotationAngle.x, Vector3.right);

                        rotationTarget *= Quaternion.AngleAxis(point.rotationAngle.z, Vector3.forward);
                        rotationTarget *= Quaternion.AngleAxis(point.rotationAngle.y, Vector3.up);

                        angles = rotationTarget.eulerAngles;
                    }
                    else
                    {
                        instances.RemoveAt(currentPoint);
                        pointsCount--;
                        continue;
                    }
                }
                else
                {
                    instances.RemoveAt(currentPoint);
                    pointsCount--;
                    continue;
                }

                point.worldPosition = position;
                point.rotationAngle = angles;

                instances[currentPoint] = point;

                currentPoint++;
            }
        }
    }
}