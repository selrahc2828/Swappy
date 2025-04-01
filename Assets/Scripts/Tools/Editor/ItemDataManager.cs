
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemDataManager : EditorWindow
{
    string spritePath = "Assets/Texture/UI/Items";
    string dataItemPath = "Assets/Data/Items/Collectibles";

    private Sprite itemSprite;
    private List<Sprite> listSpriteItem = new List<Sprite>(); //liste sprite
    private string[] spriteNames;//contient liste des noms de sprites items
    private Texture2D[] spriteTextures;
    private int selectedItemSpriteIndex;
    
    [Header("Create item")]
    private string itemName = "New Item";
    private int itemId;
    private ItemData.Category itemCategory;
    private string itemDescription = "";
    
    private Sprite selectedSprite; //id sprite selectionné
    private List<Sprite> availableSprites = new List<Sprite>();
    
    
    [MenuItem("Tools/ItemDataManager")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataManager>("Item Data Editor");
    }

    private void OnEnable()
    {
        LoadSpritesFromFolder(spritePath);
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Item", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical(GUILayout.Width(250));
        
        itemName = EditorGUILayout.TextField("Nom de l'item", itemName);
        
        // itemId = EditorGUILayout.IntField("ID de l'item", itemId);
        
        itemCategory = (ItemData.Category)EditorGUILayout.EnumPopup("Catégorie", itemCategory);
        
        GUILayout.Label("Description de l'item");
        
        // itemDescription = EditorGUILayout.TextArea(itemDescription, GUILayout.Height(60));
        itemDescription = EditorGUILayout.TextArea(itemDescription, EditorStyles.textArea, GUILayout.Height(60));
        
        #region Liste Nom Sprite

        if (listSpriteItem.Count > 0)
        {
            selectedItemSpriteIndex = EditorGUILayout.Popup("Sélectionner un Sprite", selectedItemSpriteIndex, spriteNames);
            itemSprite = listSpriteItem[selectedItemSpriteIndex];
        }
        else
        {
            EditorGUILayout.HelpBox("Aucun Sprite trouvé dans le dossier spécifié.", MessageType.Warning);
        }

        #endregion
        
        GUILayout.EndVertical();

        //itemQuantity = EditorGUILayout.IntField("Quantité", itemQuantity);
        
        GUILayout.BeginVertical(GUILayout.Width(100)); 
        
        GUILayout.Label("PreviewSprite");
        #region affiche la preview du sprite

        if (itemSprite != null)
        {
            Texture2D texture2D = itemSprite.texture;
            if (texture2D != null)
            {
                GUILayout.Label(texture2D, GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                EditorGUILayout.HelpBox($"Aucun Sprite trouvé pour {spriteNames[selectedItemSpriteIndex]}.", MessageType.Warning);
            }
        }

        #endregion
        
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        
        
        if (GUILayout.Button("Create Pattern Data", GUILayout.Width(350)))
        {
            CreateItemData();
        }
        
    }

    private void CreateItemData()
    {
        try
        {
            // auto ID (plus grand +1)
            #region auto ID (plus grand +1)
        
            string[] guids = AssetDatabase.FindAssets("t:ItemData");
            int maxItemId = 0;
        
            //cherche plus grand
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ItemData existingItem = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

                if (existingItem != null)
                {
                    maxItemId = Mathf.Max(maxItemId, existingItem.itemId);
                }
            }
            itemId = maxItemId + 1;

            #endregion
        
            ItemData newItemData = ScriptableObject.CreateInstance<ItemData>();
            newItemData.itemId = itemId;
            newItemData.itemCategory = itemCategory;
            newItemData.itemName = itemName;
            newItemData.itemDescription = itemDescription;
            newItemData.itemSprite = itemSprite;
        
            // sauvegarde de itemData
            if (!Directory.Exists(dataItemPath))
            {
                Directory.CreateDirectory(dataItemPath);
            }
        
            string newDataPath =  $"{dataItemPath}/{itemName}_{itemId}.asset";
            AssetDatabase.CreateAsset(newItemData, newDataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            EditorUtility.DisplayDialog("Succès", $"ItemData '{itemName}' créé avec ID: {itemId}!", "OK");
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Erreur", $"Une erreur est survenue lors de la création de l'item : {e.Message}", "OK");
        }
    }
    private void LoadSprites()
    {
        availableSprites.Clear();

        if (Directory.Exists(spritePath))
        {
            string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { spritePath });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                if (sprite != null)
                    availableSprites.Add(sprite);
            }
        }
    }
    
    private void LoadSpritesFromFolder(string folderPath)
    {
        listSpriteItem.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                listSpriteItem.Add(sprite);
            }
        }
        
        // setUp de la liste de nom de sprite
        spriteNames = listSpriteItem.ConvertAll(s => s.name).ToArray();
        
        // Reinitialise la sélection si l'ancien sprite n'est plus la
        if (itemSprite != null)
        {
            int index = listSpriteItem.IndexOf(itemSprite);
            selectedItemSpriteIndex = index >= 0 ? index : 0;
        }
        else
        {
            selectedItemSpriteIndex = 0;
        }
    }
    
    private void LoadPuzzleData()
    {
        // string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { spritePath });
        //
        // puzzleDataList.Clear();
        // foreach (string guid in guids)
        // {
        //     string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        //     PuzzleData data = AssetDatabase.LoadAssetAtPath<PuzzleData>(assetPath);
        //     if (data != null)
        //     {
        //         puzzleDataList.Add(data);
        //     }
        // }
        //
        // puzzleDataNames = puzzleDataList.Select(data => data.name).ToArray();
        //
        // if (puzzleDataList.Count > 0)
        // {
        //     selectedPuzzleData = puzzleDataList[selectedDataIndex];
        // }
    }

}
