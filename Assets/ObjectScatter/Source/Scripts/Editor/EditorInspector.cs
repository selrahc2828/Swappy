using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Reflection;

namespace ObjectScatter
{
    public class EditorInspector : Editor
    {
        protected SerializedProperty foldoutToggles;

        protected Dictionary<string, SerializedProperty> properties = new Dictionary<string, SerializedProperty>();

        protected ObjectScatterConfig _config;

        protected void GatherProperties(Type osType, List<string> extraFields = null)
        {
            if(extraFields != null)
            {
                foreach(var ef in extraFields)
                {
                    var so = serializedObject.FindProperty(ef);
                    if(so != null)
                    {
                        properties.Add(ef, so);
                    }
                }
            }
            
            FieldInfo[] osFields = osType.GetFields();
            for (int i = 0; i < osFields.Length; i++)
            {
                FieldInfo fieldInfo = osFields[i];
                properties.Add(fieldInfo.Name, serializedObject.FindProperty(fieldInfo.Name));
            }

        }

        protected void DrawGroup(int controlId, string title, List<string> fields,
            UnityAction<string, Action<bool>> onBeforeDrawField = null,
            UnityAction<string> onAfterDrawField = null,
            UnityAction onAfterDrawFields = null)
        {
            
            DrawArea(controlId, title, () =>
            {
                var props = properties.Where(it => fields.Contains(it.Key));

                for (int i = 0; i < fields.Count; i++)
                {
                    if (i < props.Count())
                    {
                        var elmnt = props.ElementAt(i);
                        bool skipIt = false;
                        onBeforeDrawField?.Invoke(elmnt.Key, (value) => skipIt = value);

                        if (!skipIt)
                            EditorGUILayout.PropertyField(elmnt.Value);

                        onAfterDrawField?.Invoke(elmnt.Key);
                    }
                }

                onAfterDrawFields?.Invoke();
            });
        }

        protected void DrawArea(int controlId, string title, UnityAction onDrawArea = null)
        {
            Color _inspectorColor = Color.cyan;
            if(_config == null)
            {
                _config = ObjectScatterConfig.GetInstance();
                if(_config != null)
                    _inspectorColor = _config.inspectorFoldoutColor;
            }

            var serializedPropItem = foldoutToggles.GetArrayElementAtIndex(controlId);
            var foldoutValue = serializedPropItem.boolValue;
            

            GUIStyle gsTest = new GUIStyle();

            var prevCol = GUI.backgroundColor;

            EditorGUILayout.BeginVertical();

            GUI.backgroundColor = _inspectorColor;

            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.Space(3);

            EditorGUILayout.BeginHorizontal();

            // GUILayout.FlexibleSpace();
            GUILayout.Space(15);

            //GUILayout.Label(title);
            foldoutValue = EditorGUILayout.Foldout(foldoutValue, title);

            //GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3);

            EditorGUILayout.EndVertical();
            GUI.backgroundColor = prevCol;

            if (foldoutValue)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                onDrawArea?.Invoke();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space(5);

            EditorGUILayout.EndVertical();

            serializedPropItem.boolValue = foldoutValue;
        }
    }
}