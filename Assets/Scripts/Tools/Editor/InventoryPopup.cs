using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class InventoryPopup : EditorWindow {
    
    private InventorySystem inventoryPlayer;

    private List<ItemData> itemDatas = new List<ItemData>();
    readonly string dataItemPath = "Assets/Data/Items/Collectibles";
    
    // Méthode pour afficher le pop-up
    public static void Show(InventorySystem inventory) {
        InventoryPopup window = GetWindow<InventoryPopup>("Inventaire");
        window.inventoryPlayer = inventory;
        window.Show();
    }

    private void OnEnable()
    {
        LoadAllItemData(dataItemPath);
        ChargeItemInventory();
    }

    private void OnGUI() {
        if (inventoryPlayer == null) {
            GUILayout.Label("Aucun inventaire trouvé.");
            return;
        }

        GUILayout.Label("Dictionnaire de l'inventaire", EditorStyles.boldLabel);

        // Afficher chaque item du dictionnaire
        foreach ( ItemData item in itemDatas ) {
            
            GUILayout.BeginHorizontal();

            #region Sprite

            if (item.itemSprite != null)
            {
                Texture2D texture2D = item.itemSprite.texture;
                if (texture2D != null)
                {
                    GUILayout.Label(texture2D, GUILayout.Width(50), GUILayout.Height(50));
                }
                else
                {
                    EditorGUILayout.HelpBox($"Aucun Sprite trouvé pour {item.itemName}.", MessageType.Warning);
                } 
            }
            else
            {
                EditorGUILayout.HelpBox($"Aucun Sprite défini pour l'item {item.itemName}.", MessageType.Warning);
            }
            
            #endregion
            
            GUILayout.Label(item.itemName, GUILayout.Width(100));

            #region Quantity
            GUILayout.BeginHorizontal();

            // recupere quantite actuelle (inventoryPlayer) ou met 0 si existe pas
            int quantity = inventoryPlayer.InventoryItems.ContainsKey(item)
                ? inventoryPlayer.InventoryItems[item].quantity
                : 0;
            
            if (GUILayout.Button("-", GUILayout.Width(30))) {
                inventoryPlayer.RemoveItem(item);
            }
            
            GUILayout.Label(quantity.ToString(), GUILayout.Width(100));
            if (GUILayout.Button("+", GUILayout.Width(30))) { 
                inventoryPlayer.AddItem(item);
            }
            
            GUILayout.EndHorizontal();
            #endregion

           
            GUILayout.EndHorizontal();

        }

        // Ajouter un bouton pour fermer la fenêtre
        if (GUILayout.Button("Fermer")) {
            Close();
        }
    }
    
    private void LoadAllItemData(string folderPath)
    {
        itemDatas.Clear();

        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Le dossier spécifié n'existe pas : " + folderPath);
        }

        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { folderPath });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
            if (itemData != null)
            {
                itemDatas.Add(itemData);
            }
        }
    }


    private void ChargeItemInventory()
    {
        //on verra
    }
}