using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class EvenSpreading : ObjectScatterModifier
    {
        [Min(0.01f)]
        public float xMargin = 0.5f;
        [Min(0.01f)]
        public float yMargin = 0.5f;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            instances.Clear();

            if (scatter.Path.distribution == DistributionMode.Inside)
            {
                var center = new Vector3(scatter.Boundaries.CornersMidPoint.x, 0f, scatter.Boundaries.CornersMidPoint.y);
                var size = scatter.Boundaries.Dimensions;
                var half_size = new Vector3(size.x, 0f, size.y) * 0.5f;
                var height = transform.position.y;

                var width = Mathf.CeilToInt(size.x / xMargin);
                var length = Mathf.CeilToInt(size.y / yMargin);

                var max_count = width * length;

                for (int i = 0; i < max_count; i++)
                {
                    var pos = Vector3.zero;
                    pos.x = (i % width) * xMargin;
                    pos.z = (i / width) * yMargin;
                    pos += (center - half_size) + new Vector3(.5f, 0f, .5f);
                    pos.y = height;

                    var p = new Vector2(pos.x, pos.z);

                    if (ObjectScatterHelper.ContainsPoint(scatter.Path.Points, p))
                    {
                        scatter.CreatePoint(p);
                    }
                }
            }
            else if (scatter.Path.distribution == DistributionMode.Alongside)
            {
                foreach (var p in scatter.Path.Points)
                {
                    if (scatter.Path.width <= 1f)
                        scatter.CreatePoint(p);
                    else
                    {
                        Vector2 dims = new Vector2(scatter.Path.width, scatter.Path.width);

                        var center = new Vector3(p.x, 0f, p.y);
                        var size = dims;
                        var half_size = new Vector3(size.x, 0f, size.y) * 0.5f;
                        var height = transform.position.y;

                        var width = Mathf.CeilToInt(size.x / xMargin);
                        var length = Mathf.CeilToInt(size.y / yMargin);

                        var max_count = width * length;

                        for (int i = 0; i < max_count; i++)
                        {
                            var pos = Vector3.zero;
                            pos.x = (i % width) * xMargin;
                            pos.z = (i / width) * yMargin;
                            pos += (center - half_size);
                            pos.y = height;

                            scatter.CreatePoint(new Vector2(pos.x, pos.z));
                        }
                    }
                }
            }
            else if (scatter.Path.distribution == DistributionMode.AroundPoints)
            {
                foreach (var point in scatter.Path.Points)
                {
                    // var transformedP = (point.Position);
                    Vector2 dims = new Vector2(scatter.Path.width, scatter.Path.width);

                    var center = new Vector3(point.x, 0f, point.y);
                    var size = dims;
                    var half_size = new Vector3(size.x, 0f, size.y) * 0.5f;
                    var height = transform.position.y;

                    var width = Mathf.CeilToInt(size.x / xMargin);
                    var length = Mathf.CeilToInt(size.y / yMargin);

                    var max_count = width * length;

                    for (int i = 0; i < max_count; i++)
                    {
                        var pos = Vector3.zero;
                        pos.x = (i % width) * xMargin;
                        pos.z = (i / width) * yMargin;
                        pos += (center - half_size);
                        pos.y = height;

                        var p = new Vector2(pos.x, pos.z);
                        scatter.CreatePoint(p);
                    }
                }
            }
        }
    }
}