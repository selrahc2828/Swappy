using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    [AddComponentMenu("Object Scatter/Object Scatter Circle")]
    public class ObjectScatterCircle : ObjectScatterPath
    {
        [HideInInspector]
        public Path path;

        [Min(0.1f)]
        public float radius = 1f;
        public float pointsSpacing = 0.2f;
        public bool generateEvenlyPoints = true;

        public Color lineColor = Color.green;
        public float pointDiameter = .1f;

        public override bool Loop => path.IsClosed;

        public void CreatePath()
        {
            path = new Path(transform);
            MakeCircle();
        }

        public void ResetPath()
        {
            MakeCircle();
        }

        public void MakeCircle()
        {
            path.IsClosed = false;

            path.ResetSegments(1f);

            int amountToSpawn = 13;
            for (int i = 0; i < amountToSpawn; i++)
            {
                float angle = i * Mathf.PI * 2f / amountToSpawn;
                Vector2 newPos = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                path.AddSegment(newPos, true);
            }

            path.DeleteSegment(0);
            path.DeleteSegment(0);

            path.AutoSetControlPoints = true;

            path.IsClosed = true;
        }

        public override void ComputePoints()
        {
            base.ComputePoints();

            if (path == null)
                CreatePath();

            MakeCircle();

            if (path._transform == null)
                path._transform = transform;


            if (generateEvenlyPoints)
            {
                Vector2[] evenlyPoints = path.CalculateEvenlySpacedPoints(pointsSpacing, 1);
                _computedPoints.AddRange(evenlyPoints);
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