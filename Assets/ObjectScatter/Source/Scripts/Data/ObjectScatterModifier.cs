using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    //[System.Serializable]
    public class ObjectScatterModifier : MonoBehaviour
    {
#if UNITY_EDITOR  
        [SerializeField, HideInInspector]
        ObjectScatterModifierEditor _modifierEditorInspector;
        [SerializeField, HideInInspector]
        bool _foldout;
        [HideInInspector]
        public string _displayName;
        [HideInInspector]
        public int _order;
#endif 

        [HideInInspector]
        public bool enable = true;

        public virtual void ApplyModifier(ObjectScatter scatter, ref List<ObjectScatter.PointScatter> instances)
        {

        }

#if UNITY_EDITOR 
        private void OnValidate()
        {
            if (!gameObject.TryGetComponent<ObjectScatter>(out var os))
            {
                Debug.LogError("Modifiers are added automatically through Object Scatter component. Don't use modifiers as raw components.", gameObject);
                UnityEditor.Selection.activeGameObject = gameObject;
            }
        }

        public void ValidateScatter()
        {
            if (!gameObject.TryGetComponent<ObjectScatter>(out var os))
            {
                DestroyImmediate(this);
            }
        }

#endif

    }

#if UNITY_EDITOR 
    [UnityEditor.CustomEditor(typeof(ObjectScatterModifier), true)]
    public class ObjectScatterModifierEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var e = target as ObjectScatterModifier;
            if (serializedObject != null)
            {
                serializedObject.Update();

                UnityEditor.EditorGUI.BeginChangeCheck();
                DrawPropertiesExcluding(serializedObject, "m_Script");
                if (UnityEditor.EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
            }

        }
    }
#endif 
}