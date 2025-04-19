using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ObjectScatter
{
    [CustomEditor(typeof(ObjectScatterQuad))]
    public class ObjectScatterQuadEditor : ObjectScatterPathEditor
    {
        List<string> setupGroup = new List<string>()
        {
            "isGlobal",
            "distribution",
            "width",
            "autoUpdateOnMoveTransform"
        };

        List<string> optionsGroup = new List<string>()
        {
            "size",
            "pointsSpacing",
            "generateEvenlyPoints"
        };

        List<string> colorsGroup = new List<string>()
        {
            "lineColor",
        };

        List<string> debugGroup = new List<string>()
        {
            "pointDiameter",
        };

        ObjectScatterQuad creator;
        Path Path { get { return creator.path; } }

        protected override void OnEnable()
        {
            base.OnEnable();

            creator = (ObjectScatterQuad)target;
            if (creator.path == null)
            {
                creator.CreatePath();
            }

            if (creator.inspectorFoldoutToggles.Length == 0)
                creator.inspectorFoldoutToggles = new bool[20];
            foldoutToggles = serializedObject.FindProperty("inspectorFoldoutToggles");

            GatherProperties(typeof(ObjectScatterQuad));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            //base.OnInspectorGUI();

            DrawGroup(0, "Setup", setupGroup);

            DrawGroup(1, "Options", optionsGroup, null, null, () =>
            {
                // bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
                // if (isClosed != Path.IsClosed)
                // {
                //     Undo.RecordObject(creator, "Toggle closed");
                //     Path.IsClosed = isClosed;
                // }
            });


            DrawGroup(2, "Colors", colorsGroup);
            DrawGroup(3, "Debug and Editor", debugGroup);

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(creator, "Reset Quad");

                SceneView.RepaintAll();

                creator.dirty = true;
                creator.ResetPath();

                RefreshScatter();
            }

        }

        protected override void OnSceneGUI()
        {
            base.OnSceneGUI();
            EditorGUI.BeginChangeCheck();
            Draw();
            // Debug.Log("Draw");

            if (EditorGUI.EndChangeCheck())
            {
                creator.dirty = true;
            }
        }

        void Draw()
        {
            var t = creator.transform;
            var rot = t.rotation;
            var ea = rot.eulerAngles;
            ea.x = 90f;
            rot.eulerAngles = ea;

            Matrix4x4 TRS = Matrix4x4.TRS(t.position, rot, t.localScale);

            Handles.matrix = TRS;

            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);

                Color segmentCol = creator.lineColor;
                Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
            }


            // for (int i = 0; i < Path.NumPoints; i++)
            // {
            //    // if ((i % 3 == 0 && creator.displayAnchorPoints) || creator.displayControlPoints)
            //     {
            //         Handles.color = creator.lineColor;
            //         float handleSize = creator.pointDiameter;
            //         Vector2 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.SphereHandleCap);
            //         if (Path[i] != newPos)
            //         {
            //            // Undo.RecordObject(creator, "Move point");
            //             //Path.MovePoint(i, newPos);
            //         }
            //     }
            // }


        }
    }
}