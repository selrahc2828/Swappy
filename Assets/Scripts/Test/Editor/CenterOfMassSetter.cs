using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEditor.ShortcutManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

[EditorTool("Center of mass", typeof(Rigidbody))]
[Icon("AvatarPivot")]
public class CenterOfMassSetter : EditorTool
{
    #region Overlay

    // By default, Overlays added to the canvas are not shown. Setting the `defaultDisplay` property ensures that the
    // first time this Overlay is added to a canvas it will be visible.
    [Overlay(defaultDisplay = true)]
    class CenterOfMassToolOverlay : Overlay, ITransientOverlay
    {
        Rigidbody[] selection;

        public CenterOfMassToolOverlay(Rigidbody[] targets) => selection = targets;

        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();
            root.Add(new Button(() => ResetCenterOfMass()) { text = "Reset Center Of Mass" });
            return root;
        }

        void ResetCenterOfMass()
        {
            Undo.RecordObjects(selection, "Reset center of mass");
            foreach (var rb in selection)
            {
                rb.ResetCenterOfMass();
            }
        }

        // Use the visible property to hide or show this instance from within the class.
        public bool visible => true;
    }

    #endregion

    CenterOfMassToolOverlay m_Overlay;
    
    public override void OnActivated()
    {
        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Using Center of mass Tool"), .2f);
        SceneView.AddOverlayToActiveView(m_Overlay =
            new CenterOfMassToolOverlay(targets.Select(x => x as Rigidbody).ToArray()));
    }

    public override void OnWillBeDeactivated()
    {
        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Center of mass Tool"), .2f);
        SceneView.RemoveOverlayFromActiveView(m_Overlay);
    }
    
    [Shortcut("Toggle Center of Mass Tool", KeyCode.C)]
    private static void ToggleTool()
    {
        if (!SelectionContainsRigidbody()) return;
        
        // Get the current active tool
        var currentTool = ToolManager.activeToolType;

        // Check if the current tool is the CenterOfMassTool
        if (currentTool == typeof(CenterOfMassSetter))
        {
            // If it is, switch to the Move tool (or any default tool)
            //todo return to previous tool instead 
            Tools.current = Tool.Move;
        }
        else
        {
            // Otherwise, activate the CenterOfMassTool
            ToolManager.SetActiveTool<CenterOfMassSetter>();
        }
    }

    private static bool SelectionContainsRigidbody()
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (window is not SceneView)
            return;

        foreach (var target in targets)
        {
            if (target is not Rigidbody rb)
                continue;
            EditorGUI.BeginChangeCheck();
            Transform transform = rb.transform;
            Vector3 worldCenterOfMass = rb.worldCenterOfMass;
            worldCenterOfMass = Handles.PositionHandle(worldCenterOfMass, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(rb, "Center of mass");
                rb.centerOfMass = ConvertToLocalSpaceIgnoringScale(worldCenterOfMass, transform);
            }
        }
    }

    private static Vector3 ConvertToLocalSpaceIgnoringScale(Vector3 worldPoint, Transform tsfm)
    {
        Vector3 localPoint = worldPoint - tsfm.position;
        localPoint = Quaternion.Inverse(tsfm.rotation) * localPoint;
        return localPoint;
    }
}