using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    [AddComponentMenu("Object Scatter/Object Scatter Quad")]
    public class ObjectScatterQuad : ObjectScatterPath
    {
        [HideInInspector]
        public Path path;

        [Min(0.1f)]
        public Vector2 size = new Vector2(3f, 3f);
        public float pointsSpacing = 0.2f;
        public bool generateEvenlyPoints = false;

        public Color lineColor = Color.green;
        public float pointDiameter = .1f;

        public override bool Loop => path.IsClosed;

        public void CreatePath()
        {
            path = new Path(transform);
            MakeQuad();
        }

        public void ResetPath()
        {
            MakeQuad();
        }

        void MakeQuad()
        {
            path.IsClosed = false;
            path.AutoSetControlPoints = false;

            path.ResetSegments(3f);

            //top right
            path.AddSegment(((transform.right * size.x) + (transform.up * 1 * size.y)), true);

            //top left
            path.AddSegment(((transform.right * -1 * size.x) + (transform.up * 1 * size.y)), true);

            //bottom left
            path.AddSegment(((transform.right * -1 * size.x) + (transform.up * -1 * size.y)), true);

            //bottom right
            path.AddSegment(((transform.right * size.x) + (transform.up * -1 * size.y)), true);

            path.DeleteSegment(0);
            path.DeleteSegment(0);


            path.IsClosed = true;

            //path.MoveSegment(path.NumSegments - 1, ((transform.right * size.x) + (transform.up * -1 * size.y)));
            path.ZeroTangents();
            // Debug.Log(dirty);
        }

        public override void ComputePoints()
        {
            base.ComputePoints();

            if (path == null)
                CreatePath();

            MakeQuad();

            if (path._transform == null)
                path._transform = transform;


            if (generateEvenlyPoints)
            {
                Vector2[] evenlyPoints = path.CalculateEvenlySpacedPoints(pointsSpacing, 0f);
                _computedPoints.AddRange(evenlyPoints);

                if (path.IsClosed)
                    _computedPoints.RemoveAt(_computedPoints.Count - 1);
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