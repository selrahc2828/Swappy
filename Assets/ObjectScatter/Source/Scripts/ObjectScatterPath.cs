using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectScatter
{
    public class ObjectScatterPath : ObjectScatterComponent
    {
        [SerializeField, HideInInspector]
        protected List<Vector2> _computedPoints = new List<Vector2>();
        protected bool _loop = true;

        //[Header("Setup")]
        public bool isGlobal = false;
        public DistributionMode distribution = DistributionMode.Inside;
        public float width = 1f;

        public bool autoUpdateOnMoveTransform = true;

        [HideInInspector]
        public bool dirty;

        public List<Vector2> Points => _computedPoints;
        public virtual bool Loop => _loop;

        public virtual void ComputePoints()
        {
            _computedPoints.Clear();

        }
    }
}