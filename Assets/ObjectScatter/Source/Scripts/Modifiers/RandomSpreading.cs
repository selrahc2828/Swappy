using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class RandomSpreading : ObjectScatterModifier
    {
        public int intensity = 10;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            instances.Clear();

            if (scatter.Path.distribution == DistributionMode.Inside)
            {
                for (int i = 0; i < intensity; i++)
                {
                    Vector2 p = new Vector2(Random.Range(scatter.Boundaries.MinCorner.x, scatter.Boundaries.MaxCorner.x),
                        Random.Range(scatter.Boundaries.MinCorner.y, scatter.Boundaries.MaxCorner.y));

                    if (ObjectScatterHelper.ContainsPoint(scatter.Path.Points, p))
                    {
                        scatter.CreatePoint(p);
                    }
                }
            }
            else if (scatter.Path.distribution == DistributionMode.Alongside)
            {
                if (scatter.Path.width <= 1f)
                {
                    for (int i = 0; i < intensity; i++)
                    {
                        var p = scatter.Path.Points[Random.Range(0, scatter.Path.Points.Count)];

                        scatter.CreatePoint(p);

                    }
                }
                else
                {
                    for (int i = 0; i < intensity; i++)
                    {
                        Vector2 p = new Vector2(Random.Range(scatter.Boundaries.MinCorner.x, scatter.Boundaries.MaxCorner.x),
                            Random.Range(scatter.Boundaries.MinCorner.y, scatter.Boundaries.MaxCorner.y));

                        var pt = transform.TransformPoint(new Vector3(p.x, 0f, p.y));
                        if (ObjectScatterHelper.IsCloseToPoint(scatter.Path.transform, scatter.Path.Points, new Vector2(pt.x, pt.z), scatter.Path.width))
                        {
                            scatter.CreatePoint(p);
                        }
                    }
                }
            }
            else if (scatter.Path.distribution == DistributionMode.AroundPoints)
            {
                // Temporary
                for (int i = 0; i < intensity; i++)
                {
                    Vector2 p = new Vector2(Random.Range(scatter.Boundaries.MinCorner.x, scatter.Boundaries.MaxCorner.x),
                        Random.Range(scatter.Boundaries.MinCorner.y, scatter.Boundaries.MaxCorner.y));

                    var pt = transform.TransformPoint(new Vector3(p.x, 0f, p.y));
                    if (ObjectScatterHelper.IsCloseToPoint(scatter.Path.transform, scatter.Path.Points, new Vector2(pt.x, pt.z), scatter.Path.width))
                    {
                        scatter.CreatePoint(p);
                    }
                }
            }
        }
    }
}