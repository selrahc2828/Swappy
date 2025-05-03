using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ObjectScatter
{
    [CustomEditor(typeof(ObjectScatterBezierPath))]
    public class ObjectScatterBezierPathEditor : ObjectScatterPathEditor
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
            "pointsSpacing",
            "pointsResolution",
            "generateEvenlyPoints"
        };

        List<string> colorsGroup = new List<string>()
        {
            "pointColor",
            "controlPointColor",
            "lineColor",
            "selectedLineColor"
        };

        List<string> debugGroup = new List<string>()
        {
            "displayAnchorPoints",
            "displayControlPoints",
            "anchorDiameter",
            "selectecontrolDiameterdLineColor"
        };

        ObjectScatterBezierPath creator;
        Path Path { get { return creator.path; } }

        const float segmentSelectDistanceThreshold = .1f;
        int selectedSegmentIndex = -1;

        protected override void OnEnable()
        {
            base.OnEnable();

            creator = (ObjectScatterBezierPath)target;
            if (creator.path == null)
            {
                creator.CreatePath();
                creator.MakeBeziers();
                // creator
            }

            if (creator.inspectorFoldoutToggles.Length == 0)
                creator.inspectorFoldoutToggles = new bool[20];
            foldoutToggles = serializedObject.FindProperty("inspectorFoldoutToggles");

            GatherProperties(typeof(ObjectScatterBezierPath));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            //base.OnInspectorGUI();

            DrawGroup(0, "Setup", setupGroup);

            DrawGroup(1, "Options", optionsGroup, null, null, () =>
            {
                bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
                if (isClosed != Path.IsClosed)
                {
                    Undo.RecordObject(creator, "Toggle closed");
                    Path.IsClosed = isClosed;
                }

                bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
                if (autoSetControlPoints != Path.AutoSetControlPoints)
                {
                    Undo.RecordObject(creator, "Toggle auto set controls");
                    Path.AutoSetControlPoints = autoSetControlPoints;
                }
            });


            DrawGroup(2, "Colors", colorsGroup);
            DrawGroup(3, "Debug and Editor", debugGroup);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                Undo.RecordObject(creator, "Changing Bezier");

                SceneView.RepaintAll();

                creator.dirty = true;

                RefreshScatter();
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnSceneGUI()
        {
            base.OnSceneGUI();
            
            Input();

            EditorGUI.BeginChangeCheck();
            Draw();

            if (EditorGUI.EndChangeCheck())
            {
                creator.dirty = true;
                RefreshScatter();
            }
        }

        void Input()
        {
            Event guiEvent = Event.current;

            Plane p = new Plane(creator.transform.up, creator.transform.position);

            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            float enter = 0f;
            Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            if (!p.Raycast(ray, out enter))
                return;

            Vector3 hitPoint = creator.transform.InverseTransformPoint(ray.GetPoint(enter));

            mousePos.x = hitPoint.x;
            mousePos.y = hitPoint.z;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                if (selectedSegmentIndex != -1)
                {
                    Undo.RecordObject(creator, "Split segment");
                    Path.SplitSegment(mousePos, selectedSegmentIndex);
                    creator.dirty = true;
                }
                else if (!Path.IsClosed)
                {
                    Undo.RecordObject(creator, "Add segment");
                    Path.AddSegment(mousePos);
                    creator.dirty = true;
                }
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                float minDstToAnchor = creator.anchorDiameter * .5f;
                int closestAnchorIndex = -1;

                for (int i = 0; i < Path.NumPoints; i += 3)
                {
                    float dst = Vector2.Distance(mousePos, Path[i]);
                    if (dst < minDstToAnchor)
                    {
                        minDstToAnchor = dst;
                        closestAnchorIndex = i;
                    }
                }

                if (closestAnchorIndex != -1)
                {
                    Undo.RecordObject(creator, "Delete segment");
                    Path.DeleteSegment(closestAnchorIndex);
                    creator.dirty = true;
                }
            }

            if (guiEvent.type == EventType.MouseMove)
            {
                float minDstToSegment = segmentSelectDistanceThreshold;
                int newSelectedSegmentIndex = -1;

                for (int i = 0; i < Path.NumSegments; i++)
                {
                    Vector2[] points = Path.GetPointsInSegment(i);
                    float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                    if (dst < minDstToSegment)
                    {
                        minDstToSegment = dst;
                        newSelectedSegmentIndex = i;
                    }
                }

                if (newSelectedSegmentIndex != selectedSegmentIndex)
                {
                    selectedSegmentIndex = newSelectedSegmentIndex;
                    HandleUtility.Repaint();
                }
            }

            HandleUtility.AddDefaultControl(0);
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

                if (creator.displayControlPoints)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(points[1], points[0]);
                    Handles.DrawLine(points[2], points[3]);
                }

                Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedLineColor : creator.lineColor;
                Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
            }


            for (int i = 0; i < Path.NumPoints; i++)
            {
                if ((i % 3 == 0 && creator.displayAnchorPoints) || creator.displayControlPoints)
                {
                    Handles.color = (i % 3 == 0) ? creator.pointColor : creator.controlPointColor;
                    float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;
                    var fmh_248_70_638806728771526973 = Quaternion.identity; Vector2 newPos = Handles.FreeMoveHandle(Path[i], handleSize, Vector2.zero, Handles.SphereHandleCap);
                    if (Path[i] != newPos)
                    {
                        Undo.RecordObject(creator, "Move point");
                        Path.MovePoint(i, newPos);
                    }
                }
            }
        }
    }
}