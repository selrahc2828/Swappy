
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemDataManager : EditorWindow
{
    readonly string spritePath = "Assets/Texture/UI/Items";
    readonly string prefabPath = "Assets/Prefabs/Items";
    readonly string dataItemPath = "Assets/Data/Items/Collectibles";

    private Sprite itemSprite;
    private List<Sprite> listSpriteItem = new List<Sprite>(); //liste sprite
    private string[] spriteNames;//contient liste des noms de sprites items
    private Texture2D[] spriteTextures;
    private int selectedItemSpriteIndex;
    
    private GameObject itemPrefab;
    private List<GameObject> listPrefabItem = new List<GameObject>();
    private string[] prefabNames;
    private int selectedItemPrefabIndex;
    
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
        LoadPrefabsFromFolder(prefabPath);
    }

    private void OnGUI()
    {            
        GUILayout.Label("Sprite Path: " + spritePath);
        GUILayout.Label("Prefab Path: " + prefabPath);
        GUILayout.Label("Data Item Path: " + dataItemPath);
        GUILayout.Space(10);

        GUILayout.Label("Create Item", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
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
        
        #region Liste Nom Prefab

        if (listPrefabItem.Count > 0)
        {
            selectedItemPrefabIndex = EditorGUILayout.Popup("Sélectionner une Prefab", selectedItemPrefabIndex, prefabNames);
            itemPrefab = listPrefabItem[selectedItemPrefabIndex];
        }
        else
        {
            EditorGUILayout.HelpBox("Aucune Prefab trouvée.", MessageType.Warning);
        }

        #endregion
        
        
        GUILayout.EndVertical();

        //itemQuantity = EditorGUILayout.IntField("Quantité", itemQuantity);
        
        GUILayout.BeginVertical(GUILayout.Width(100)); 
        
        GUILayout.Label("Preview Sprite");
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
        
        
        GUILayout.BeginVertical(GUILayout.Width(100)); 
        
        GUILayout.Label("Preview Prefab");
        #region affiche la preview de la prefab

        if (itemPrefab != null)
        {
            //EditorGUILayout.ObjectField(itemPrefab, typeof(GameObject), false, GUILayout.Width(100), GUILayout.Height(100));
            Texture2D previewTexture = AssetPreview.GetAssetPreview(itemPrefab);
            if (previewTexture != null)
            {
                GUILayout.Label(previewTexture, GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                EditorGUILayout.HelpBox($"Aucune prefab trouvée pour {spriteNames[selectedItemSpriteIndex]}.", MessageType.Warning);
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
            // int maxItemId = 0;
        
            //cherche plus grand
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ItemData existingItem = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

                if (existingItem != null)
                {
                    // maxItemId = Mathf.Max(maxItemId, existingItem.itemId);
                    if (existingItem.itemName == itemName)
                    {
                        throw new Exception("Un item avec ce nom existe déjà.");
                    }
                }
            }
            // itemId = maxItemId + 1;

            #endregion
        
            ItemData newItemData = CreateInstance<ItemData>();
            //newItemData.itemId = itemId;
            newItemData.itemCategory = itemCategory;
            newItemData.itemName = itemName;
            newItemData.itemDescription = itemDescription;
            newItemData.itemSprite = itemSprite;
            newItemData.itemPrefab = itemPrefab;
        
            if (!Directory.Exists(dataItemPath))
            {
                Directory.CreateDirectory(dataItemPath);
            }
        
            string newDataPath =  $"{dataItemPath}/{itemName}.asset"; // _{itemId}
            AssetDatabase.CreateAsset(newItemData, newDataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            EditorUtility.DisplayDialog("Succès", $"ItemData '{itemName}' créé", "OK");
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Erreur", $"Une erreur est survenue lors de la création de l'item : {e.Message}", "OK");
        }
    }
   
    private void LoadSpritesFromFolder(string folderPath)
    {
        listSpriteItem.Clear();

        if (!Directory.Exists(folderPath))
        {
            return;
        }
        
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
    
    private void LoadPrefabsFromFolder(string folderPath)
    {
        listPrefabItem.Clear();
        
        if (!Directory.Exists(folderPath))
        {
            return;
        }
        
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null)
            {
                listPrefabItem.Add(prefab);
            }
        }
        prefabNames = listPrefabItem.ConvertAll(p => p.name).ToArray();
    }

}
