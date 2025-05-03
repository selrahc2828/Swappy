using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class ProjectOnMesh : ObjectScatterModifier
    {
        public int maxAmount = 40;
        public bool followCollisionNormals;
        public MeshFilter meshFilterToProject;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            instances.Clear();

            if (meshFilterToProject != null)
            {
                Vector3[] meshVerts = meshFilterToProject.sharedMesh.vertices;

                for (int i = 0; i < maxAmount; i++)
                {
                    var pointIndex = 0;
                    var point = scatter.CreatePoint(new Vector2(), out pointIndex);

                    var mappedIndex = (int)Utils.Map((float)i, 0, (float)instances.Count, 0, (float)meshVerts.Length);

                    Vector3 vert = meshFilterToProject.transform.TransformPoint(meshVerts[mappedIndex]);
                    Vector3 normal = meshFilterToProject.sharedMesh.normals[mappedIndex];

                    var finalRot = ((followCollisionNormals ? Quaternion.FromToRotation(Vector3.up, normal) : Quaternion.identity) *
                           Quaternion.AngleAxis(point.rotationAngle.x, Vector3.right));

                    finalRot *= Quaternion.AngleAxis(point.rotationAngle.z, Vector3.forward);
                    finalRot *= Quaternion.AngleAxis(point.rotationAngle.y, Vector3.up);

                    point.rotationAngle = finalRot.eulerAngles;
                    point.worldPosition = vert;
                    point.scale = Vector3.one;

                    instances[pointIndex] = point;
                }
            }

        }
    }
}