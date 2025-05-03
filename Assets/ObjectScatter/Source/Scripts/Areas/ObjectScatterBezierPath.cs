using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectScatter;
using System;

namespace ObjectScatter
{
    [AddComponentMenu("Object Scatter/Object Scatter Bezier Curve")]
    public class ObjectScatterBezierPath : ObjectScatterPath
    {
        [HideInInspector]
        public Path path;

        //[Header("Options")]
        [Min(0.1f)]
        public float pointsSpacing = 0.6f;
        public float pointsResolution = 1f;
        public bool generateEvenlyPoints = true;

        //[Header("Colors")]
        public Color pointColor = Color.red;
        public Color controlPointColor = Color.white;
        public Color lineColor = Color.green;
        public Color selectedLineColor = Color.yellow;

        //[Header("Debug and Editor")]
        public bool displayAnchorPoints = true;
        public bool displayControlPoints = true;
        public float anchorDiameter = .1f;
        public float controlDiameter = .075f;

        public override bool Loop => path.IsClosed;

        public void CreatePath()
        {
            path = new Path(transform);
        }

        public void MakeBeziers()
        {
            if (path.IsClosed)
                path.IsClosed = false;
            path.AutoSetControlPoints = false;

            path.ResetSegments(1f);

            float sc = 2f;


            path.AddSegment(((Vector2)transform.right * -1 + (Vector2)transform.up) * sc);
            path.AddSegment(((Vector2)transform.right * 1 + (Vector2)transform.up) * sc);

            path.AddSegment(((Vector2)transform.right * 1 + (Vector2)transform.up * -1) * sc);
            path.AddSegment(((Vector2)transform.right * -1 + (Vector2)transform.up * -1) * sc);

            path.DeleteSegment(0);
            path.DeleteSegment(0);

            path.IsClosed = true;
            path.AutoSetControlPoints = true;

            path.AutoSetControlPoints = false;
        }

        public override void ComputePoints()
        {
            base.ComputePoints();

            if (path == null)
                CreatePath();

            if (path._transform == null)
                path._transform = transform;

            pointsResolution = Mathf.Clamp(pointsResolution, 0f, 1000f);

            if (generateEvenlyPoints)
            {
                Vector2[] evenlyPoints = path.CalculateEvenlySpacedPoints(pointsSpacing, pointsResolution);
                _computedPoints.AddRange(evenlyPoints);

                if (path.IsClosed)
                {
                    _computedPoints.RemoveAt(_computedPoints.Count - 1);
                }

                // foreach(var p in evenlyPoints)
                // {
                //     var p2 = transform.TransformPoint(new Vector3(p.x, 0f, p.y));
                //     Debug.DrawLine(p2, p2 + Vector3.up, Color.red, 20f);
                // }
            }
            else
            {
                for (int i = 0; i < path.NumPoints; i++)
                {
                    if (!path.IsAnchor(i)) continue;

                    var p = path[i];
                    _computedPoints.Add(p);
                }
            }
        }
    }
}