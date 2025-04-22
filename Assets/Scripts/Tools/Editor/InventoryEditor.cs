using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventorySystem))]
public class InventoryEditor : Editor {
    private bool showPopup = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        InventorySystem inventory = (InventorySystem)target;

        // Ajouter un bouton dans l'inspecteur
        if (GUILayout.Button("Afficher Inventaire")) {
            // Ouvrir le pop-up (Editor Window) lorsque le bouton est pressé
            InventoryPopup.Show(inventory);
        }
    }
}