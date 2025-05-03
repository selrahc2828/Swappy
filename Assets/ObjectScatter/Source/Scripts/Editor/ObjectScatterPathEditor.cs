using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectScatter
{
    public class ObjectScatterPathEditor : EditorInspector
    {
        ObjectScatterPath _path;
        ObjectScatter _parentScatter;

        Vector3 _previousPosition;
        Vector3 _previousRotation;

        protected virtual void OnEnable()
        {
            _path = target as ObjectScatterPath;
            _parentScatter = _path.gameObject.GetComponentInParent<ObjectScatter>();
            if(_parentScatter != null && _parentScatter.gameObject == _path.gameObject)
                _parentScatter = null;
        }

        protected virtual void OnSceneGUI()
        {
            if (_path.autoUpdateOnMoveTransform)
            {
                if (_previousPosition != _path.transform.position || _previousRotation != _path.transform.eulerAngles)
                {
                    RefreshScatter();
                }

                _previousPosition = _path.transform.position;
                _previousRotation = _path.transform.eulerAngles;
            }
        }

        protected void RefreshScatter()
        {
            if (_parentScatter != null)
            {
                _parentScatter.Refresh();
            }
        }
    }
}