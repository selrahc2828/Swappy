using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ObjectScatter
{
    public static class Shortcuts
    {
        static GameObject CreateNewScatterObj(string name)
        {
            GameObject newScatter = new GameObject(name);
            if (Selection.activeTransform != null)
            {
                newScatter.transform.SetParent(Selection.activeTransform);
                newScatter.transform.localPosition = Vector3.zero;
                newScatter.transform.localRotation = Quaternion.identity;
                newScatter.transform.localScale = Vector3.one;
            }
			else 
            {
                newScatter.transform.position = Camera.current.transform.TransformPoint(Vector3.forward * 10f);
            }

            Selection.activeGameObject = newScatter;

            return newScatter;
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Circle")]
        static void CreateObjectScatterCircleArea()
        {
            CreateNewScatterObj("New Circle Path")
                .AddComponent<ObjectScatterCircle>();
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Quad")]
        static void CreateObjectScatterQuadArea()
        {
            CreateNewScatterObj("New Quad Path")
                .AddComponent<ObjectScatterQuad>();
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Bezier Curve")]
        static void CreateObjectScatterBezierCurveArea()
        {
            CreateNewScatterObj("New Bezier Curve Path")
                .AddComponent<ObjectScatterBezierPath>();
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Scatter with Circle")]
        static void CreateObjectScatterCircle()
        {
            CreateNewScatterObj("New Object Scatter with Circle")
                .AddComponent<ObjectScatter>().gameObject
                .AddComponent<ObjectScatterCircle>();
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Scatter with Quad")]
        static void CreateObjectScatterQuad()
        {
            CreateNewScatterObj("New Object Scatter with Quad")
                .AddComponent<ObjectScatter>().gameObject
                .AddComponent<ObjectScatterQuad>();
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Scatter with Bezier Curve")]
        static void CreateObjectScatterBezierCurve()
        {
            CreateNewScatterObj("New Object Scatter with Bezier Curve")
                .AddComponent<ObjectScatter>().gameObject
                .AddComponent<ObjectScatterBezierPath>();
        }

        [MenuItem("GameObject/3D Object/Object Scatter/Scatter Only")]
        static void CreateObjectScatterRaw()
        {
             CreateNewScatterObj("New Object Scatter")
                .AddComponent<ObjectScatter>();
        }
    }
}