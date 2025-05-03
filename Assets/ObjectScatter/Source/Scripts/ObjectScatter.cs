using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ObjectScatter
{
    [ExecuteInEditMode]
    [AddComponentMenu("Object Scatter/Object Scatter")]
    public class ObjectScatter : ObjectScatterComponent
    {
        [System.Serializable]
        public struct PointScatter
        {
            public Vector3 worldPosition;

            public Vector2 pathPoint;
            public Vector3 rotationOffset;
            public Vector3 rotationAngle;
            public Vector3 scale;

            public bool isActive;

            public GameObject obj;
        }

        [System.Serializable]
        public struct ItemScatter
        {
            public GameObject gameObject;
            public float chance;
        }

        //[Header("Setup")]
        public ScatterMode mode = ScatterMode.Editor_And_Runtime;
        public bool autoUpdateOnSelect = false;
        public bool autoUpdateOnMoveTransform = true;
        //public ProjectionMode projection = ProjectionMode.Grid;

        public List<ObjectScatterModifier> modifiers = new List<ObjectScatterModifier>();

        //

        //Prototype baking
        // ("Baking")
        [SerializeField]
        bool bakeToMesh;
        [SerializeField]
        List<BakedInstance> bakedInstances = new List<BakedInstance>();
        Dictionary<Material, BakedInstance> bakedInstancesByMaterial = new Dictionary<Material, BakedInstance>();

        //[Header("Debug")]
        public Color gizmosColor = new Color(0, 0, 0.7f, 0.9f);
        public bool drawInstancesGizmos = false;

        //Noise (Amplitude, Frequency, etc) ?

        //Disabled for now
        //public bool useSpawnChance;

        [Header("Other")]
        public List<ItemScatter> prefabs = new List<ItemScatter>();
        public List<ObjectScatterPath> modifierPaths = new List<ObjectScatterPath>();

        public ObjectScatterPath Path
        {
            get
            {
                if (path == null) path = GetComponent<ObjectScatterPath>();
                return path;
            }
        }

        [SerializeField] ObjectScatterPath path;
        [SerializeField, HideInInspector] List<PointScatter> instances = new List<PointScatter>();
        [SerializeField, HideInInspector] List<Transform> instancesCache = new List<Transform>();

        [HideInInspector]
        public bool updateQueuedForPrefabInstance = false;

        ObjectScatterBoundary boundaries = new ObjectScatterBoundary();
        public ObjectScatterBoundary Boundaries => boundaries;

        public List<PointScatter> Instances => instances;

        int previousIntensity;
        bool fullRestart;

        [SerializeField, HideInInspector]
        Transform itemsRoot;

#if UNITY_EDITOR
        [HideInInspector]
        public ObjectScatterConfig _config;

        [SerializeField, HideInInspector]
        Vector3 _previousPosition;
        [SerializeField, HideInInspector]
        Vector3 _previousRotation;

        float _transformRefreshElapsed;
#endif

        void Awake()
        {
            if (mode == ScatterMode.Runtime)
                Refresh(true);

            GuaranteeRoot();
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (_config == null)
            {
                _config = ObjectScatterConfig.GetInstance();
            }

            if (autoUpdateOnMoveTransform && _transformRefreshElapsed < Time.time)
            {
                if (_previousPosition != transform.position || _previousRotation != transform.eulerAngles)
                    Refresh();

                _previousPosition = transform.position;
                _previousRotation = transform.eulerAngles;

                _transformRefreshElapsed = Time.time + 0.2f;
            }
#endif
        }

        void GuaranteeRoot()
        {
            if (itemsRoot == null)
            {
                itemsRoot = new GameObject("Root").transform;
                itemsRoot.SetParent(transform);
                //itemsRoot.gameObject.hideFlags = HideFlags.HideInInspector;
            }

            itemsRoot.localPosition = Vector3.zero;
            itemsRoot.localScale = Vector3.one;
            itemsRoot.localRotation = Quaternion.identity;
        }

        public void ResetAndClear()
        {
            fullRestart = true;

            Clear();

            //reset baking
            fullRestart = false;
        }

        void Clear()
        {
            if (fullRestart)
            {
                if (instancesCache != null)
                {
                    foreach (Transform _child in instancesCache)
                    {
                        if (_child != null)
                        {
                            if (!Application.isPlaying)
                                DestroyImmediate(_child.gameObject);
                            else
                                Destroy(_child.gameObject);
                        }
                    }

                    instancesCache.Clear();
                }
                else
                {
                    instancesCache.Clear();
                }
            }

            instances.Clear();
        }


        public void Refresh(bool forceFullRefresh = false)
        {
            if (Path == null)
            {
                Clear();
                return;
            }

            Path?.ComputePoints();

            GuaranteeRoot();

            if (mode == ScatterMode.Editor && Application.isPlaying)
                return;

#if UNITY_EDITOR
            //string prefabPath = "";
            //GameObject prefabRoot = null;
            if (CheckIfItsPartOfPrefab())
            {
                updateQueuedForPrefabInstance = true;
                return;
            }
            updateQueuedForPrefabInstance = false;
#endif

            instancesCache.RemoveAll(it => it == null);

            if ((prefabs != null && prefabs.Count <= 0) || forceFullRefresh)
                fullRestart = true;

            Clear();

            if (Path.Points.Count <= 0) return;

            //Update boundaries
            boundaries.CalculateBoundaries(Path.Points);

            instances.Clear();

            foreach (var mod in modifiers)
            {
                if (mod.enabled)
                {
                    if (mod.enable)
                        mod.ApplyModifier(this, ref instances);
                }
            }

            if (mode == ScatterMode.Runtime && !Application.isPlaying)
                return;

            if (fullRestart)
            {
                if (instances == null) return;
                instances.Shuffle();
                // Manage instances
                {
                    for (int i = 0; i < instances.Count; i++)
                    {
                        var instance = instances[i];
                        if (!CreateItem(instance, i))
                            break;
                    }
                }
            }

            // Validate instances
            if (!fullRestart) ValidateItems();
            UpdatePositions();

            //

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
            fullRestart = false;

            Path.dirty = false;
        }

        public PointScatter CreatePoint(Vector2 point)
        {
            var instance = new ObjectScatter.PointScatter()
            {
                pathPoint = point
            };
            var originPosition = transform.position;

            Vector3 posOffset = Vector3.zero;
            Vector3 scale = Vector3.one;
            Vector3 angles = Vector3.zero;

            instance.worldPosition = originPosition + new Vector3(instance.pathPoint.x, 0f, instance.pathPoint.y);
            instance.scale = scale;
            instance.rotationAngle = angles;

            instances.Add(instance);

            return instance;
        }

        public PointScatter CreatePoint(Vector2 point, out int index)
        {
            index = instances.Count;
            return CreatePoint(point);
        }

        public void UpdatePositions()
        {
#if UNITY_EDITOR
            //string prefabPath = "";
            //GameObject prefabRoot = null;
            if (CheckIfItsPartOfPrefab())
            {
                updateQueuedForPrefabInstance = true;
                return;
            }
            updateQueuedForPrefabInstance = false;
#endif

            //if(projection == ProjectionMode.Colliders)
            {
                // Disable objects
                for (int i = 0; i < instances.Count; i++)
                {
                    var instance = instances[i];
                    if (instance.obj != null)
                    {
                        instance.obj.SetActive(false);
#if UNITY_EDITOR
                        EditorUtility.SetDirty(instance.obj);
#endif
                    }
                }
            }

            var originPosition = transform.position;
            var toEnable = new List<GameObject>();

            for (int i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                if (instance.obj == null) continue;

                var position = instance.worldPosition;
                var t = instance.obj.transform;

                t.localScale = instance.scale;
               // t.localEulerAngles = instance.rotationAngle;
               
                t.rotation = Quaternion.AngleAxis(instance.rotationAngle.z, Vector3.forward) * Quaternion.AngleAxis(instance.rotationAngle.x, Vector3.right)
                    * Quaternion.AngleAxis(instance.rotationAngle.y, Vector3.up);

                //                Quaternion.LookRotation(instance.rotationAngle, Vector3.forward);

                if (!ValidatePositionBasedOnModifiers(instance.worldPosition)) continue;

                //if (projection == ProjectionMode.Grid)
                {
                    t.position = instance.worldPosition; //(transform.right * position.x + transform.forward * position.z + transform.up * position.y);
                    toEnable.Add(instance.obj);
                }
            }

            ResetBakedInstances();

            for (int i = 0; i < toEnable.Count; i++)
            {
                var obj = toEnable[i];
                if (obj != null)
                {
                    obj.SetActive(true);

                    if (bakeToMesh)
                    {
                        var instanceMaterial = obj.GetComponent<MeshRenderer>()?.sharedMaterial;
                        if (instanceMaterial)
                            AddBakedInstance(obj, instanceMaterial);

                    }

                }

#if UNITY_EDITOR
                EditorUtility.SetDirty(obj);
#endif
            }

            if (bakeToMesh)
            {
                CreateBakedInstances();
            }
            else
            {
                DestroyBakedInstances();
            }
        }

        void AddBakedInstance(GameObject obj, Material material)
        {
            if (!bakedInstancesByMaterial.ContainsKey(material))
                bakedInstancesByMaterial.Add(material, new BakedInstance(transform, material, bakedInstances.Count + System.DateTime.Now.ToString("yyyyMMddHHmmss")));

            var bi = bakedInstancesByMaterial[material];

            if (!bakedInstances.Contains(bi))
                bakedInstances.Add(bi);

            if (bi.AddInstance(obj))
            {
                if (obj.TryGetComponent<MeshRenderer>(out var mr))
                {
                    mr.enabled = false;
                }
            }
        }

        Queue<BakedInstance> queuedToDestroy = new Queue<BakedInstance>();
        void CreateBakedInstances()
        {
            if (bakedInstances.Count > 0)
            {
                queuedToDestroy.Clear();

                foreach (var bakedInstance in bakedInstances)
                {
                    if (bakedInstance.Size > 0)
                        bakedInstance.Bake();
                    else
                    {
                        bakedInstance.Destroy();
                        queuedToDestroy.Enqueue(bakedInstance);
                    }
                }

                while (queuedToDestroy.Count > 0)
                {
                    var bakedInstanceToDestroy = queuedToDestroy.Dequeue();

                    bakedInstancesByMaterial.Remove(bakedInstanceToDestroy.material);
                    bakedInstances.Remove(bakedInstanceToDestroy);
                }
            }
        }

        void ResetBakedInstances()
        {
            if (bakeToMesh)
            {
                CheckBakedInstances();

                foreach (var bakedInstance in bakedInstances)
                {
                    bakedInstance.Reset();
                }
            }
        }

        void CheckBakedInstances()
        {
            if (bakedInstancesByMaterial.Count <= 0 && bakedInstances.Count > 0)
            {
                foreach (var bakedInstance in bakedInstances)
                    bakedInstancesByMaterial.Add(bakedInstance.material, bakedInstance);
            }
        }

        void DestroyBakedInstances()
        {
            foreach (var bakedInstance in bakedInstances)
            {
                bakedInstance.Destroy();
            }

            bakedInstancesByMaterial.Clear();
            bakedInstances.Clear();
        }

        //List<ItemScatter> localCacheChosenPrefabs = null;
        bool CreateItem(PointScatter pointScatter, int at)
        {
            if (prefabs.Count == 0) return false;

            //localCacheChosenPrefabs = prefabs.OrderBy(it => it.chance).ToList();

            GameObject prefab = null;

            #region disabled WIP
            // if (useSpawnChance)
            // {
            //     for (int i = 0; i < localCacheChosenPrefabs.Count; i++)
            //     {
            //         var itemScatter = localCacheChosenPrefabs[i];
            //         var prefabChance = itemScatter.chance;
            //         if (Random.value < prefabChance)
            //         {
            //             prefab = itemScatter.gameObject;
            //             break;
            //         }
            //     }
            // }
            // else
            #endregion
            {
                prefab = prefabs[Random.Range(0, prefabs.Count)].gameObject;
            }


            if (prefab == null) return false;

            if (Application.isEditor)
            {
#if UNITY_EDITOR 
                var item = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
#else
                var item = Instantiate(prefab);
#endif
                item.transform.SetParent(itemsRoot);

                instancesCache.Add(item.transform);

                pointScatter.obj = item;

#if UNITY_EDITOR
                EditorUtility.SetDirty(item);
#endif
                instances[at] = pointScatter;
            }

            return true;
        }

        public void ValidateItems()
        {
            int childCount = instancesCache.Count;
            int instancesCount = instances.Count;
            bool detectedMissingInstances = false;

            for (int i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                if (instance.obj == null)
                {
                    detectedMissingInstances = true;
                    break;
                }
            }

            if (childCount == instancesCount && !detectedMissingInstances) return;

            var objectPool = new List<GameObject>();
            foreach (Transform child in instancesCache)
                objectPool.Add(child.gameObject);

            for (int i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                if (objectPool.Count > 0)
                {
                    if (instance.obj != null)
                        objectPool.Remove(instance.obj);
                    else
                    {
                        var obj = objectPool[0];
                        instance.obj = obj;
                        objectPool.RemoveAt(0);

                        instances[i] = instance;
                    }
                }
                else
                {
                    if (!CreateItem(instance, i))
                        break;
                }


            }

            for (int i = 0; i < objectPool.Count; i++)
            {
                var obj = objectPool[i];
                instancesCache.Remove(obj.transform);

                if (!Application.isPlaying) DestroyImmediate(obj);
                else Destroy(obj);

            }
        }

        bool ValidatePositionBasedOnModifiers(Vector3 globalPoint)
        {
            for (int i = 0; i < modifierPaths.Count; i++)
            {
                ObjectScatterPath modifierPath = modifierPaths[i];
                if (modifierPath == null) continue;

                modifierPath.ComputePoints();

                if (modifierPath.distribution == DistributionMode.Inside)
                {
                    //var globalPoint = transform.TransformPoint(new Vector3(point.x, 0f, point.y));
                    var localPointRelativeToPath = modifierPath.transform.InverseTransformPoint(globalPoint);

                    if (ObjectScatterHelper.ContainsPoint(modifierPath.Points, new Vector2(localPointRelativeToPath.x, localPointRelativeToPath.z)))
                        return false;
                }
                else if (modifierPath.distribution == DistributionMode.Alongside)
                {
                    // var p = transform.TransformPoint(new Vector3(point.x, 0f, point.y));

                    if (ObjectScatterHelper.IsCloseToASegment(modifierPath.transform, modifierPath.Points,
                            new Vector2(globalPoint.x, globalPoint.z), modifierPath.width, modifierPath.Loop, (int)modifierPath.distribution))
                        return false;
                }
                else if (modifierPath.distribution == DistributionMode.AroundPoints)
                {
                    if (ObjectScatterHelper.IsCloseToPoint(modifierPath.transform, modifierPath.Points, new Vector2(globalPoint.x, globalPoint.z), modifierPath.width))
                        return false;
                }
            }

            return true;
        }

        bool CheckIfItsPartOfPrefab()
        {
#if UNITY_EDITOR
            //GameObject prefabRoot = null;
            string prefabPath = "";
            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(transform))
            {
                prefabPath = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);

                //if (UnityEditor.PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
                //    prefabRoot = UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(gameObject);
                //else
                //    prefabRoot = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);

                //UnityEditor.PrefabUtility.UnpackPrefabInstance(prefabRoot,
                //    UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.UserAction);
                //Debug.Log("[OBJECT SCATTER] ObjectScatter won't update if it's a part of a Prefab Instance! Edit it in Prefab Mode or click on <b>Apply changes to Prefab Root!</b>");

                return true;
            }

#endif
            return false;
        }

#if UNITY_EDITOR
        public void ReloadFromPrefabInstance()
        {
            GameObject prefabRoot = null;
            var prefabPath = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            if (UnityEditor.PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
                prefabRoot = UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(gameObject);
            else
                prefabRoot = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);


            UnityEditor.PrefabUtility.UnpackPrefabInstance(prefabRoot,
                            UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);

            Refresh(true);

            UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(prefabRoot, prefabPath, InteractionMode.AutomatedAction);
        }

#endif
    }

}
