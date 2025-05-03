using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor;
#endif 

namespace ObjectScatter
{
    public class TransformRandomization : ObjectScatterModifier
    {
        public MinMaxVector3 randomPosition;
        public MinMaxVector3 randomRotation;
        public MinMaxVector3 randomScale;
        public bool _randomPositionSingle;
        public bool _randomRotationSingle;
        public bool _randomScaleSingle = true;

        public override void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                var point = instances[i];

                Vector3 posOffset = point.worldPosition;
                var scale = point.scale;
                var angles = Vector3.zero; // point.rotationAngle;

                if (randomScale.min.magnitude > 0 || randomScale.max.magnitude > 0)
                {
                    if (!_randomScaleSingle)
                    {
                        scale.x = Random.Range(randomScale.min.x, randomScale.max.x);
                        scale.y = Random.Range(randomScale.min.y, randomScale.max.y);
                        scale.z = Random.Range(randomScale.min.z, randomScale.max.z);
                    }
                    else
                    {
                        float v = Random.Range(randomScale.min.x, randomScale.max.x);
                        scale.x = scale.y = scale.z = v;
                    }
                }

                if (randomRotation.min.magnitude > 0 || randomRotation.max.magnitude > 0)
                {
                    if (!_randomRotationSingle)
                    {
                        angles.x = Random.Range(randomRotation.min.x, randomRotation.max.x);
                        angles.y = Random.Range(randomRotation.min.y, randomRotation.max.y);
                        angles.z = Random.Range(randomRotation.min.z, randomRotation.max.z);
                    }
                    else
                    {
                        float v = Random.Range(randomRotation.min.x, randomRotation.max.x);
                        angles.x = angles.y = angles.z = v;
                    }
                }

                if (randomPosition.min.magnitude > 0 || randomPosition.max.magnitude > 0)
                {
                    if (!_randomPositionSingle)
                    {
                        posOffset.x += Random.Range(randomPosition.min.x, randomPosition.max.x);
                        posOffset.y += Random.Range(randomPosition.min.y, randomPosition.max.y);
                        posOffset.z += Random.Range(randomPosition.min.z, randomPosition.max.z);
                    }
                    else
                    {
                        float v = Random.Range(randomPosition.min.x, randomPosition.max.x);
                        posOffset.x += v;
                        posOffset.y += v;
                        posOffset.z += v;
                    }
                }

                point.worldPosition = posOffset;
                point.scale = scale;
                point.rotationAngle += angles;

                 instances[i] = point;
            }
        }

    }

#if UNITY_EDITOR 
    [CustomEditor(typeof(TransformRandomization), true)]
    public class TransformRandomizationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var osp = target as TransformRandomization;

            serializedObject.Update();

            UnityEditor.EditorGUI.BeginChangeCheck();

            //float prevV = EditorGUIUtility.labelWidth;
            //  EditorGUIUtility.labelWidth = prevV * .5f;

            EditorGUILayout.BeginVertical(GUI.skin.box);
            osp.randomPosition = HandleRandomVar("Position", osp.randomPosition, ref osp._randomPositionSingle);
            // osp._randomPositionSingle = EditorGUILayout.Toggle("", osp._randomPositionSingle, GUILayout.Width(20));
            EditorGUILayout.EndVertical();

            GUILayout.Space(6);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            osp.randomRotation = HandleRandomVar("Rotation", osp.randomRotation, ref osp._randomRotationSingle);
            // osp._randomRotationSingle = EditorGUILayout.Toggle("", osp._randomRotationSingle, GUILayout.Width(20));
            EditorGUILayout.EndVertical();

            GUILayout.Space(6);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            osp.randomScale = HandleRandomVar("Scale", osp.randomScale, ref osp._randomScaleSingle);
            //osp._randomScaleSingle = EditorGUILayout.Toggle("", osp._randomScaleSingle, GUILayout.Width(20));
            EditorGUILayout.EndVertical();

            // EditorGUIUtility.labelWidth = prevV;

            if (UnityEditor.EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

        }

        MinMaxVector3 HandleRandomVar(string title, MinMaxVector3 val, ref bool single)
        {
            //   EditorGUILayout.BeginHorizontal();
            //  EditorGUILayout.LabelField(title, GUILayout.Width(70));
            // single = EditorGUILayout.Toggle("", single, GUILayout.Width(20));
            single = EditorGUILayout.Toggle(title, single, GUILayout.MinWidth(60));
            //EditorGUILayout.EndHorizontal();

            if (!single)
            {

                //EditorGUILayout.BeginVertical();
                val.min = EditorGUILayout.Vector3Field("Min", val.min);
                val.max = EditorGUILayout.Vector3Field("Max", val.max);
                // EditorGUILayout.EndVertical();

                return val;
            }
            else
            {

                //EditorGUILayout.BeginVertical();
                float vMin = EditorGUILayout.FloatField("Min", val.min.x);
                float vMax = EditorGUILayout.FloatField("Max", val.max.x);
                // EditorGUILayout.EndVertical();

                val.min.x = val.min.y = val.min.z = vMin;
                val.max.x = val.max.y = val.max.z = vMax;

                return val;
            }
        }
    }
#endif

}
