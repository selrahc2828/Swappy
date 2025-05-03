using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class PoissonDiscSamplingSpreading : ObjectScatterModifier
    {
        [Min(0.1f)]
        public float radius = 1f;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            instances.Clear();

            if (scatter.Path.distribution == DistributionMode.Inside)
            {
                var genPoints = PoissonDiscSampling.GeneratePoints(radius, scatter.Boundaries.Dimensions);
                for (int i = 0; i < genPoints.points.Count; i++)
                {
                    var offsetedPoint = scatter.Boundaries.CornersMidPoint +
                        new Vector2(genPoints.points[i].x - scatter.Boundaries.Dimensions.x / 2, genPoints.points[i].y - scatter.Boundaries.Dimensions.y / 2);

                    if (ObjectScatterHelper.ContainsPoint(scatter.Path.Points, offsetedPoint))
                    {
                        scatter.CreatePoint(offsetedPoint);
                    }
                }
            }
            else if (scatter.Path.distribution == DistributionMode.AroundPoints || scatter.Path.distribution == DistributionMode.Alongside)
            {
                foreach (var p in scatter.Path.Points)
                {
                    var transformedP = p;
                    Vector2 dims = new Vector2(scatter.Path.width, scatter.Path.width);
                    var genPoints = PoissonDiscSampling.GeneratePoints(radius, dims);

                    for (int i = 0; i < genPoints.points.Count; i++)
                    {
                        scatter.CreatePoint(p + (genPoints.points[i] - dims / 2));
                    }
                }
            }
        }
    }
}