using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using Object = UnityEngine.Object;

namespace ObjectScatter
{
    [CanEditMultipleObjects, CustomEditor(typeof(ObjectScatter))]
    public class ObjectScatterEditor : EditorInspector
    {
        static class Styles
        {
            public static readonly GUIStyle sceneViewOverlayTransparentBackground = " ";
        }

        List<string> modesGroup = new List<string>()
        {
            "mode",
            "autoUpdateOnSelect",
            "autoUpdateOnMoveTransform"
            // "projection"
        };

        // List<string> contactCollisionInteractionGroup = new List<string>()
        // {
        //     "ignoreMask",
        //     "avoidMask",
        //     "followCollisionNormals",
        //     "useSlope",
        //     "slope"
        // };

        List<string> contactCollisionInteractionGridRandomGroup = new List<string>()
        {
            "ignoreMask",
            "avoidMask",
        };

        List<string> meshProjectionInteractionGroup = new List<string>()
        {
            "followCollisionNormals",
        };

        static bool foldoutGroupItems;
        static bool foldoutGroupPaths;

        bool needsRefresh = false;

        Texture2D bannerTex;
        string sourcePath;
        string editorGuiPath;

        ObjectScatter osp;

        GUIStyle padlockButtonStyle;
        Texture2D padlockLockTex,
                padlockUnlockTex,
                arrowUpTex,
                arrowDownTex;

        GUIStyle windowBackgroundStyle;

        bool shouldRefreshInspectorAfterPrefabRefresh = false;

        int currentModifierSelectedToAdd = 0;
        string[] modifiersOptionsDrop = new string[]
        {
            "Transform Offset"
        };

        List<int> modifierIndexesToBeRemoved = new List<int>();

        ObjectScatterModifier[] _cacheModifiers;

        private void OnEnable()
        {
            osp = (ObjectScatter)target;
            if (osp.inspectorFoldoutToggles.Length == 0)
                osp.inspectorFoldoutToggles = new bool[20];

            foldoutToggles = serializedObject.FindProperty("inspectorFoldoutToggles");

            modifierIndexesToBeRemoved = new List<int>();

            string scriptLocation = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            sourcePath = scriptLocation.Replace("/Scripts/Editor/ObjectScatterEditor.cs", "");
            editorGuiPath = sourcePath + "/Textures";

            padlockLockTex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorGuiPath + "/padlock_lock.png");
            padlockUnlockTex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorGuiPath + "/padlock_unlock.png");

            arrowUpTex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorGuiPath + "/arrow_up.png");
            arrowDownTex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorGuiPath + "/arrow_down.png");
            //EditorGUIUtility.isProSkin.

            GatherProperties(typeof(ObjectScatter));

            // Refresh modifiers
            osp.modifiers.Clear();
            _cacheModifiers = osp.gameObject.GetComponents<ObjectScatterModifier>();
            foreach (var mod in _cacheModifiers)
                mod.hideFlags = HideFlags.HideInInspector;

            _cacheModifiers = _cacheModifiers.OrderBy(it => it._order).ToArray();

            osp.modifiers.AddRange(_cacheModifiers);

            CheckModifiers();

            if (osp.autoUpdateOnSelect || (osp.Path != null && osp.Path.dirty))
                osp.Refresh();
        }

        private void OnDestroy()
        {
            foreach (var mod in _cacheModifiers)
            {
                if (mod != null)
                {
                    mod.hideFlags = HideFlags.None;
                    mod.ValidateScatter();
                }
            }
        }

        void CheckModifiers()
        {
            //Local modifiers check
            ObjectScatterPath[] modifierPaths = osp.gameObject.GetComponentsInChildren<ObjectScatterPath>(true);
            for (int i = osp.modifierPaths.Count - 1; i >= 0; i--)
            {
                if (!modifierPaths.Contains(osp.modifierPaths[i]))
                {
                    osp.modifierPaths.RemoveAt(i);
                    needsRefresh = true;
                }
            }

            for (int i = 0; i < modifierPaths.Length; i++)
            {
                if (modifierPaths[i] == osp.Path) continue;

                if (!osp.modifierPaths.Contains(modifierPaths[i]))
                {
                    osp.modifierPaths.Add(modifierPaths[i]);
                    needsRefresh = true;
                }
            }

            //Global modifiers check
            ObjectScatterPath[] global_ModifierPaths = FindObjectsOfType<ObjectScatterPath>();
            for (int i = 0; i < global_ModifierPaths.Length; i++)
            {
                if (!global_ModifierPaths[i].isGlobal) continue;

                if (global_ModifierPaths[i] == osp.Path) continue;

                if (!osp.modifierPaths.Contains(global_ModifierPaths[i]))
                {
                    osp.modifierPaths.Add(global_ModifierPaths[i]);
                    needsRefresh = true;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            osp = (ObjectScatter)target;

            serializedObject.Update();

            if (osp.Path != null && osp.Path.dirty)
                needsRefresh = true;

            if (needsRefresh)
            {
                osp.Refresh();
                needsRefresh = false;
            }

            if (osp.updateQueuedForPrefabInstance)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("ObjectScatter won't update as a Prefab Instance in a scene. Edit it in Prefab Mode or click on on the button below", MessageType.Warning);
                if (GUILayout.Button("Apply changes to Prefab Root"))
                {
                    shouldRefreshInspectorAfterPrefabRefresh = true;
                }
            }

            DrawArea(0, "Tools", () =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("drawInstancesGizmos"), new GUIContent("Draw Gizmos"));
                if (osp.drawInstancesGizmos)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmosColor"));
                EditorGUILayout.Space(3);

                //EditorGUI.BeginDisabledGroup(osp.spread == SpreadMode.PoissonDiscSampling);

                if (GUILayout.Button("Clear Items"))
                {

                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();

                    osp.ResetAndClear();
                }
                //EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Build"))
                {
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();

                    osp.Refresh(true);

                    GUILayout.Space(15);
                }

            });

           // EditorGUI.BeginChangeCheck();
            DrawGroup(1, "Setup", modesGroup,
            (string key, Action<bool> skipIt) =>
            {
            },
            (string key) =>
            {
            },
            () =>
            {

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("bakeToMesh"));
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    osp.Refresh(true);
                }
            });

            if (shouldRefreshInspectorAfterPrefabRefresh)
            {
                osp.ReloadFromPrefabInstance();

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();

                shouldRefreshInspectorAfterPrefabRefresh = false;
                return;
            }

            // if (osp.projection == ProjectionMode.Colliders)
            //     DrawGroup(2, "Collision Contact Interaction", contactCollisionInteractionGroup);

           // if (EditorGUI.EndChangeCheck()) needsRefresh = true;

            DrawArea(6, "Items", () =>
            {
                Event evt = Event.current;


                GUILayout.Space(5);
                DrawListPrefabs("Prefabs List", "prefabs", "prefabChances", ref foldoutGroupItems, true, true, () =>
                {
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    osp.Refresh(true);
                }, null);


                Rect drop_area = GUILayoutUtility.GetLastRect();

                switch (evt.type)
                {
                    case EventType.DragUpdated:
                    case EventType.DragPerform:
                        if (!drop_area.Contains(evt.mousePosition))
                            return;

                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (evt.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();

                            bool addedItem = false;
                            foreach (GameObject dragged_object in DragAndDrop.objectReferences)
                            {
                                osp.prefabs.Add(new ObjectScatter.ItemScatter()
                                {
                                    gameObject = dragged_object,
                                    chance = 1.0f
                                });

                                serializedObject.ApplyModifiedProperties();
                                serializedObject.Update();


                                addedItem = true;
                            }

                            if (addedItem) osp.Refresh(true);
                        }
                        break;
                }
            });


            DrawArea(8, "Modifiers", () =>
            {
                // currentModifierSelectedToAdd = EditorGUILayout.Popup("Add Modifier", currentModifierSelectedToAdd, modifiersOptionsDrop); 
                if (GUILayout.Button("Add Modifier"))
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Even Spreading"), false, () =>
                    {
                        var to = osp.gameObject.AddComponent<EvenSpreading>();
                        to.hideFlags = HideFlags.HideInInspector;
                        to._displayName = "Even Spreading";
                        to._order = osp.modifiers.Count;

                        osp.modifiers.Add(to);
                        needsRefresh = true;
                    });

                    menu.AddItem(new GUIContent("Random Spreading"), false, () =>
                    {
                        var to = osp.gameObject.AddComponent<RandomSpreading>();
                        to.hideFlags = HideFlags.HideInInspector;
                        to._displayName = "Random Spreading";
                        to._order = osp.modifiers.Count;

                        osp.modifiers.Add(to);
                        needsRefresh = true;
                    });

                    menu.AddItem(new GUIContent("PoissonDisc Sampling Spreading"), false, () =>
                   {
                       var to = osp.gameObject.AddComponent<PoissonDiscSamplingSpreading>();
                       to.hideFlags = HideFlags.HideInInspector;
                       to._displayName = "PoissonDisc Sampling Spreading";
                       to._order = osp.modifiers.Count;

                       osp.modifiers.Add(to);
                       needsRefresh = true;
                   });

                    menu.AddItem(new GUIContent("Transform Offset"), false, () =>
                    {
                        var to = osp.gameObject.AddComponent<TransformOffset>();
                        to.hideFlags = HideFlags.HideInInspector;
                        to._displayName = "Transform Offset";
                        to._order = osp.modifiers.Count;

                        osp.modifiers.Add(to);
                        needsRefresh = true;
                    });

                    menu.AddItem(new GUIContent("Transform Randomization"), false, () =>
                    {
                        var to = osp.gameObject.AddComponent<TransformRandomization>();
                        to.hideFlags = HideFlags.HideInInspector;
                        to._displayName = "Transform Randomization";
                        to._order = osp.modifiers.Count;

                        osp.modifiers.Add(to);
                        needsRefresh = true;
                    });

                    menu.AddItem(new GUIContent("Rotate Towards Next Point"), false, () =>
                    {
                        var to = osp.gameObject.AddComponent<RotateTowardsNextPoint>();
                        to.hideFlags = HideFlags.HideInInspector;
                        to._displayName = "Rotate Towards Next Point";
                        to._order = osp.modifiers.Count;

                        osp.modifiers.Add(to);
                        needsRefresh = true;
                    });

                    menu.AddItem(new GUIContent("Project On Colliders"), false, () =>
                    {
                        var to = osp.gameObject.AddComponent<ProjectOnColliders>();
                        to.hideFlags = HideFlags.HideInInspector;
                        to._displayName = "Project On Colliders";
                        to._order = osp.modifiers.Count;

                        osp.modifiers.Add(to);
                        needsRefresh = true;
                    });

                    menu.AddItem(new GUIContent("Project On Mesh"), false, () =>
                   {
                       var to = osp.gameObject.AddComponent<ProjectOnMesh>();
                       to.hideFlags = HideFlags.HideInInspector;
                       to._displayName = "Project On Mesh";
                       to._order = osp.modifiers.Count;

                       osp.modifiers.Add(to);
                       needsRefresh = true;
                   });


                    menu.ShowAsContext();
                }

                var modifiersSO = serializedObject.FindProperty("modifiers");
                for (int i = 0; i < modifiersSO.arraySize; i++)
                {
                    var modifierSP = modifiersSO.GetArrayElementAtIndex(i);
                    using (SerializedObject modifierSO = new SerializedObject(modifierSP.objectReferenceValue))
                    {
                        modifierSO.Update();
                        EditorGUI.BeginChangeCheck();

                        Editor modifierEditor = null;

                        var customEditorSO = modifierSO.FindProperty("_modifierEditorInspector");
                        var customEditor = customEditorSO.objectReferenceValue;
                        //Debug.Log((Editor)customEditorSO);
                        void CreateCustomEditorIfNotExists()
                        {
                            if (((Editor)customEditor) == null)
                            {
                                customEditor = modifierEditor = Editor.CreateEditor(modifierSP.objectReferenceValue);
                                customEditorSO.objectReferenceValue = customEditor;
                            }
                            else
                            {
                                modifierEditor = (Editor)customEditor;
                            }
                        }
                        CreateCustomEditorIfNotExists();

                        var displayName = modifierSO.FindProperty("_displayName");
                        var modifierFoldoutSO = modifierSO.FindProperty("_foldout");
                        var enabledSO = modifierSO.FindProperty("enable");

                        bool foldout = modifierFoldoutSO.boolValue;

                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUI.indentLevel++;

                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel--;
                        enabledSO.boolValue = EditorGUILayout.Toggle(enabledSO.boolValue, GUILayout.Width(10f));
                        EditorGUI.indentLevel++;

                        if (EditorGUI.EndChangeCheck()) needsRefresh = true;

                        foldout = EditorGUILayout.Foldout(foldout, displayName.stringValue, true);

                        EditorGUI.BeginChangeCheck();

                        const float arrowIconSize = 16f;
                        if (GUILayout.Button(arrowUpTex, EditorStyles.label, GUILayout.Width(arrowIconSize), GUILayout.Height(arrowIconSize)))
                        {
                            var goingToIndex = i - 1;
                            if (goingToIndex < 0)
                                goingToIndex = modifiersSO.arraySize - 1;

                            //modifiersSO.MoveArrayElement(i, goingToIndex);
                            var itemTomove = osp.modifiers[i];
                            
                            osp.modifiers.RemoveAt(i);
                            osp.modifiers.Insert(goingToIndex, itemTomove);

                            RefreshModifiersOrders();
                        }
                        if (GUILayout.Button(arrowDownTex, EditorStyles.label, GUILayout.Width(arrowIconSize), GUILayout.Height(arrowIconSize)))
                        {
                            var goingToIndex = i + 1;
                            if (goingToIndex >= modifiersSO.arraySize)
                                goingToIndex = 0;

                            //modifiersSO.MoveArrayElement(i, goingToIndex);
                            var itemTomove = osp.modifiers[i];
                            osp.modifiers.RemoveAt(i);
                            osp.modifiers.Insert(goingToIndex, itemTomove);

                            RefreshModifiersOrders();
                        }
                        if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("_Menu").image, "Options"), EditorStyles.label, GUILayout.Width(arrowIconSize), GUILayout.Height(arrowIconSize)))
                        {
                            var local_i = i;
                            GenericMenu menu = new GenericMenu();

                            menu.AddItem(new GUIContent("Remove"), false, (object data) =>
                            {
                                var d = (int)data;
                                modifierIndexesToBeRemoved.Add(d);

                            }, local_i);

                            // menu.AddItem(new GUIContent("Reset"), false, (object data) =>
                            // {
                            //     var me = (Editor)data;
                            //     me.ResetTarget();

                            //     needsRefresh = true;

                            // }, modifierEditor);
                            menu.ShowAsContext();
                        }

                        EditorGUILayout.EndHorizontal();

                        if (foldout)
                        {
                            EditorGUI.BeginDisabledGroup(!enabledSO.boolValue);

                            if (modifierEditor.serializedObject == null)
                                CreateCustomEditorIfNotExists();

                            modifierEditor?.OnInspectorGUI();
                            EditorGUI.EndDisabledGroup();
                        }

                        modifierFoldoutSO.boolValue = foldout;

                        EditorGUI.indentLevel--;
                        EditorGUILayout.EndVertical();

                        if (EditorGUI.EndChangeCheck()) needsRefresh = true;
                        modifierSO.ApplyModifiedProperties();
                    }
                }

            });

            var modifiersSO = serializedObject.FindProperty("modifiers");
            foreach (var modifierIndex in modifierIndexesToBeRemoved)
            {
                DestroyImmediate(osp.modifiers[modifierIndex]);

                modifiersSO.DeleteArrayElementAtIndex(modifierIndex);
                needsRefresh = true;

            }
            modifierIndexesToBeRemoved.Clear();

            DrawArea(7, "Exclusions", () =>
            {
                DrawList("Paths", "modifierPaths", ref foldoutGroupPaths, false, false, null, () =>
                {
                    GUILayout.Label("These items are automatically added to this list");
                });
            });

            if (osp.modifierPaths.Any(it => it == null))
            {
                Undo.RecordObject(osp, "Refreshing Object Scatter");
                osp.modifierPaths.RemoveAll(it => it == null);
                osp.Refresh();
            }

            serializedObject.ApplyModifiedProperties();
        }

        void RefreshModifiersOrders()
        {
            for (int i = 0; i < osp.modifiers.Count; i++)
            {
                osp.modifiers[i]._order = i;
            }

        }

        protected virtual void OnSceneGUI()
        {
            osp = (ObjectScatter)target;

            //Handles.color = new Color(0, 0, 0.7f, 0.5f);
            //Handles.CubeHandleCap(0, osp.transform.position, osp.transform.rotation, 2f, EventType.Repaint);

            if (osp.drawInstancesGizmos)
            {
                Handles.color = osp.gizmosColor;
                var points = osp.Instances;
                float sphereSize = 0.4f;
                for (int i = 0; i < points.Count; i++)
                {
                    var p = points[i];
                    if (p.obj == null || !p.obj.activeInHierarchy) continue;
                    var itemPos = p.obj.transform.position;

                    Vector3 p3 = itemPos;

                    if (Camera.current != null)
                    {
                        float magDist = (p3 - Camera.current.transform.position).magnitude;
                        sphereSize = (p3 - Camera.current.transform.position).magnitude / 90f;

                        Color currentColor = Handles.color;
                        currentColor.a = 1f - Mathf.Clamp(Utils.Map(magDist, 0.0f, 70f, 0.3f, 0.7f), 0.0f, 1f);
                        Handles.color = currentColor;
                    }

                    Handles.SphereHandleCap(0, p3, Quaternion.identity, sphereSize, EventType.Repaint);

                    // if (osp.spread == SpreadMode.PoissonDiscSampling)
                    // {
                    //     Color currentColor = Handles.color;
                    //     currentColor.a = 0.65f;
                    //     Handles.color = currentColor;
                    //     Handles.CircleHandleCap(0, p3, Quaternion.LookRotation((p3 + Vector3.up) - p3, Vector3.up), osp.radius * .5f, EventType.Repaint);
                    // }
                }
            }
            // var origin = osp.transform.position;
            // Vector3[] myCorners = new Vector3[4];
            // myCorners[0] = osp.Boundaries.CornersMidPoint + new Vector2(osp.Boundaries.MinCorner.x, osp.Boundaries.MinCorner.y);
            // myCorners[1] = osp.Boundaries.CornersMidPoint + new Vector2(osp.Boundaries.MinCorner.x, osp.Boundaries.MaxCorner.y);
            // myCorners[2] = osp.Boundaries.CornersMidPoint + new Vector2(osp.Boundaries.MaxCorner.x, osp.Boundaries.MaxCorner.y);
            // myCorners[3] = osp.Boundaries.CornersMidPoint + new Vector2(osp.Boundaries.MaxCorner.x, osp.Boundaries.MinCorner.y);

            // myCorners[0] = osp.transform.TransformPoint(new Vector3(myCorners[0].x, 0f, myCorners[0].y));
            // myCorners[1] = osp.transform.TransformPoint(new Vector3(myCorners[1].x, 0f, myCorners[1].y));
            // myCorners[2] = osp.transform.TransformPoint(new Vector3(myCorners[2].x, 0f, myCorners[2].y));
            // myCorners[3] = osp.transform.TransformPoint(new Vector3(myCorners[3].x, 0f, myCorners[3].y));

            // for (int i = 0; i < 4; i++)
            // {
            //     if (i == 3)
            //     {
            //         Handles.DrawLine(myCorners[i], myCorners[0]);
            //     }
            //     else
            //     {
            //         Handles.DrawLine(myCorners[i], myCorners[i + 1]);
            //     }
            // }
        }

        List<int> indexesToRemove = new List<int>();
        void DrawList(string listTitle, string propertyName, ref bool foldoutFlag, bool hasDragAndDrop, bool canRemoveItem,
            UnityAction onRemoveItem = null,
            UnityAction postRenderList = null,
            UnityAction<int> onRemoveItemIndividual = null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);
            foldoutFlag = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutFlag, listTitle);
            EditorGUILayout.EndHorizontal();
            indexesToRemove.Clear();

            var modPathsSO = serializedObject.FindProperty(propertyName);
            if (foldoutFlag)
            {
                EditorGUILayout.BeginVertical();

                if (modPathsSO.arraySize > 0)
                {
                    for (int i = 0; i < modPathsSO.arraySize; i++)
                    {
                        SerializedProperty modPathItemSO = modPathsSO.GetArrayElementAtIndex(i);

                        EditorGUILayout.BeginHorizontal();
                        // GUILayout.Space(12);
                        EditorGUILayout.LabelField(i + ":", GUILayout.Width(12));

                        EditorGUILayout.PropertyField(modPathItemSO, new GUIContent(""));

                        if (canRemoveItem)
                        {
                            Color prevCol = GUI.backgroundColor;
                            GUI.backgroundColor = new Color(0.85f, 0.2f, 0.2f, 1f);
                            if (GUILayout.Button("X", GUILayout.Width(27)))
                            {
                                indexesToRemove.Add(i);
                            }
                            GUI.backgroundColor = prevCol;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                }

                if (hasDragAndDrop)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Drag and Drop prefabs here");

                    padlockButtonStyle = new GUIStyle(EditorStyles.label);
                    padlockButtonStyle.alignment = TextAnchor.MiddleLeft;
                    padlockButtonStyle.fontSize = 8;

                    if (GUILayout.Button(ActiveEditorTracker.sharedTracker.isLocked ? "Inspector Locked" : "Inspector Unlocked",
                    new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleRight,
                        fontSize = 10
                    }))
                    {
                        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                    }
                    if (GUILayout.Button(ActiveEditorTracker.sharedTracker.isLocked ? padlockLockTex : padlockUnlockTex, padlockButtonStyle, GUILayout.Width(14), GUILayout.Height(14)))
                    {
                        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                    }
                    GUILayout.EndHorizontal();
                }

                postRenderList?.Invoke();

                GUILayout.Space(10);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            var indexesRemoved = indexesToRemove.Count;
            foreach (var indexRemove in indexesToRemove)
            {
                onRemoveItemIndividual?.Invoke(indexRemove);

                modPathsSO.DeleteArrayElementAtIndex(indexRemove);
                modPathsSO.DeleteArrayElementAtIndex(indexRemove);
            }

            if (indexesRemoved > 0)
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                onRemoveItem?.Invoke();

            }
        }

        void DrawListPrefabs(string listTitle, string propertyName, string chancePropName, ref bool foldoutFlag, bool hasDragAndDrop, bool canRemoveItem,
            UnityAction onRemoveItem = null,
            UnityAction postRenderList = null,
            UnityAction<int> onRemoveItemIndividual = null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);
            foldoutFlag = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutFlag, listTitle);
            EditorGUILayout.EndHorizontal();
            indexesToRemove.Clear();

            var serializedList = serializedObject.FindProperty(propertyName);
            //var pathChancesSO = serializedObject.FindProperty(chancePropName);

            if (foldoutFlag)
            {
                EditorGUILayout.BeginVertical();

                if (serializedList.arraySize > 0)
                {
                    for (int i = 0; i < serializedList.arraySize; i++)
                    {
                        SerializedProperty itemScatterSO = serializedList.GetArrayElementAtIndex(i);
                        //SerializedProperty pathItemChanceSO = pathChancesSO.GetArrayElementAtIndex(i);

                        EditorGUILayout.BeginHorizontal();
                        // GUILayout.Space(12);
                        EditorGUILayout.LabelField(i + ":", GUILayout.Width(12));

                        EditorGUI.BeginChangeCheck();

                        EditorGUILayout.PropertyField(itemScatterSO.FindPropertyRelative("gameObject"), new GUIContent(""));

                        // if (osp.useSpawnChance)
                        // {
                        //     var itemScatterChanceSP = itemScatterSO.FindPropertyRelative("chance");
                        //     itemScatterChanceSP.floatValue = EditorGUILayout.Slider(itemScatterChanceSP.floatValue, 0f, 1f);
                        // }

                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedObject.ApplyModifiedProperties();
                            serializedObject.Update();
                            osp.Refresh(true);
                        }

                        if (canRemoveItem)
                        {
                            Color prevCol = GUI.backgroundColor;
                            GUI.backgroundColor = new Color(0.85f, 0.2f, 0.2f, 1f);
                            if (GUILayout.Button("X", GUILayout.Width(27)))
                            {
                                indexesToRemove.Add(i);
                            }
                            GUI.backgroundColor = prevCol;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                }

                if (hasDragAndDrop)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Drag and Drop prefabs here");

                    padlockButtonStyle = new GUIStyle(EditorStyles.label);
                    padlockButtonStyle.alignment = TextAnchor.MiddleLeft;
                    padlockButtonStyle.fontSize = 8;

                    if (GUILayout.Button(ActiveEditorTracker.sharedTracker.isLocked ? "Inspector Locked" : "Inspector Unlocked",
                    new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleRight,
                        fontSize = 10
                    }))
                    {
                        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                    }
                    if (GUILayout.Button(ActiveEditorTracker.sharedTracker.isLocked ? padlockLockTex : padlockUnlockTex, padlockButtonStyle, GUILayout.Width(14), GUILayout.Height(14)))
                    {
                        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
                    }
                    GUILayout.EndHorizontal();
                }

                postRenderList?.Invoke();

                GUILayout.Space(10);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            var indexesRemoved = indexesToRemove.Count;
            foreach (var indexRemove in indexesToRemove)
            {
                onRemoveItemIndividual?.Invoke(indexRemove);

                serializedList.DeleteArrayElementAtIndex(indexRemove);
            }

            if (indexesRemoved > 0)
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                onRemoveItem?.Invoke();

            }
        }
    }
}