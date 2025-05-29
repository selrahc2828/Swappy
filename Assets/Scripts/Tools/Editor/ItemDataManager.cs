
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemDataManager : EditorWindow
{
    readonly string spritePath = "Assets/Texture/UI/Items";
    readonly string prefabPath = "Assets/Prefabs/Interaction/Items";
    readonly string dataItemPath = "Assets/Data/Items/Collectibles";
    readonly string dataTapePath = "Assets/Data/Items/Tapes";
    readonly string tapeMusicPath = "Assets/Sound/TapeMusic";

    private enum Onglet
    {
        Collectible,
        Tape
    }
    private Onglet _currentOnglet = Onglet.Collectible;
    
    //Commun 
    private Sprite itemSprite;
    private List<Sprite> listSpriteItem = new List<Sprite>(); //liste sprite
    private string[] spriteNames;//contient liste des noms de sprites items
    private Texture2D[] spriteTextures;
    private int selectedItemSpriteIndex;
    
    private GameObject itemPrefab;
    private List<GameObject> listPrefabItem = new List<GameObject>();
    private string[] prefabNames;
    private int selectedItemPrefabIndex;
    
    [Header("Create Collectible item")]
    private string itemName = "New Item";
    private CollectibleData.Category itemCategory;
    private string itemDescription = "";
    
    private Sprite selectedSprite; //id sprite selectionné
    private List<Sprite> availableSprites = new List<Sprite>();
    
    private string tapeMusicName;
    private string[] tapeMusicNames;
    private int selectedTapeMusicIndex;
    
    [MenuItem("Tools/ItemDataManager")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataManager>("Item Data Editor");
    }

    private void OnEnable()
    {
        LoadSpritesFromFolder(spritePath);
        LoadPrefabsFromFolder(prefabPath);
        LoadTapeMusicNames(tapeMusicPath);
    }

    private void OnGUI()
    {         
        GUILayout.Label("Sprite Path: " + spritePath);
        GUILayout.Label("Prefab Path: " + prefabPath);
        GUILayout.Label("CollectibleData Item Path: " + dataItemPath);
        GUILayout.Label("TapeData Item Path: " + dataTapePath);
        GUILayout.Label("Tape Name Path: " + tapeMusicPath);
        
        _currentOnglet = (Onglet)GUILayout.Toolbar((int)_currentOnglet, new[] { "Collectible", "Tape" });
        GUILayout.Space(10);

        switch (_currentOnglet)
        {
            case Onglet.Collectible:
                CollectibleDataCreator();
                break;
            case Onglet.Tape:
                TapeDataCreator();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        GUILayout.Space(10);

        
    }
    private void CollectibleDataCreator()
    {
        GUILayout.Label("Create Item Collectible", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(250));
        
        itemName = EditorGUILayout.TextField("Nom de l'item", itemName);
        itemCategory = (CollectibleData.Category)EditorGUILayout.EnumPopup("Catégorie", itemCategory);
        
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
        
        if (GUILayout.Button("Create Collectable Data", GUILayout.Width(350)))
        {
            CreateItemData();
        }
        
    }

    private void TapeDataCreator()
    {
        GUILayout.Label("Create Tape Data", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical(GUILayout.Width(250));

        // Tape info
        itemName = EditorGUILayout.TextField("Tape Name", itemName);
        GUILayout.Label("Description");
        itemDescription = EditorGUILayout.TextArea(itemDescription, GUILayout.Height(60));

        // Music Dropdown
        if (tapeMusicNames.Length > 0)
        {
            selectedTapeMusicIndex = EditorGUILayout.Popup("FMOD Music Name", selectedTapeMusicIndex, tapeMusicNames);
            tapeMusicName = tapeMusicNames[selectedTapeMusicIndex];
        }

        // Sprite
        if (listSpriteItem.Count > 0)
        {
            selectedItemSpriteIndex = EditorGUILayout.Popup("Select Sprite", selectedItemSpriteIndex, spriteNames);
            itemSprite = listSpriteItem[selectedItemSpriteIndex];
        }

        // Prefab
        if (listPrefabItem.Count > 0)
        {
            selectedItemPrefabIndex = EditorGUILayout.Popup("Select Prefab", selectedItemPrefabIndex, prefabNames);
            itemPrefab = listPrefabItem[selectedItemPrefabIndex];
        }

        GUILayout.EndVertical();

        // Sprite Preview
        GUILayout.BeginVertical(GUILayout.Width(100));
        GUILayout.Label("Preview Sprite");

        if (itemSprite != null)
        {
            Texture2D texture2D = itemSprite.texture;
            if (texture2D != null)
            {
                GUILayout.Label(texture2D, GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                EditorGUILayout.HelpBox($"No sprite preview for {spriteNames[selectedItemSpriteIndex]}.", MessageType.Warning);
            }
        }
        GUILayout.EndVertical();

        // Prefab Preview
        GUILayout.BeginVertical(GUILayout.Width(100));
        GUILayout.Label("Preview Prefab");

        if (itemPrefab != null)
        {
            Texture2D previewTexture = AssetPreview.GetAssetPreview(itemPrefab);
            if (previewTexture != null)
            {
                GUILayout.Label(previewTexture, GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                EditorGUILayout.HelpBox($"No prefab preview for {prefabNames[selectedItemPrefabIndex]}.", MessageType.Warning);
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("Create Tape Data", GUILayout.Width(350)))
        {
            CreateTapeData();
        }
    }
    
    private void CreateItemData()
    {
        try
        {
            CheckExisting(itemName);
            
            CollectibleData newItemData = CreateInstance<CollectibleData>();
            newItemData.itemCategory = itemCategory;
            newItemData.itemName = itemName;
            newItemData.itemDescription = itemDescription;
            newItemData.itemSprite = itemSprite;
            newItemData.itemPrefab = itemPrefab;
            
            SaveAsset(dataItemPath, itemName, newItemData);
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Erreur", $"Une erreur est survenue lors de la création de l'item : {e.Message}", "OK");
        }
    }
    
    private void CreateTapeData()
    {
        try
        {
            CheckExisting(itemName);
            
            TapeData data = CreateInstance<TapeData>();
            data.itemName = itemName;
            data.itemDescription = itemDescription;
            data.itemSprite = itemSprite;
            data.itemPrefab = itemPrefab;
            data.musicFmodName = tapeMusicName;

            SaveAsset(dataTapePath, itemName, data);
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Erreur", $"Une erreur est survenue lors de la création de la cassette : {e.Message}", "OK");
        }
    }
    
    private void CheckExisting(string name)
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        
        //cherche plus grand
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ItemData existingItem = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

            if (existingItem != null)
            {
                if (existingItem.itemName == name)
                {
                    throw new Exception("Un item avec ce nom existe déjà.");
                }
            }
        }
    }

    private void SaveAsset(string path, string fileName, ScriptableObject data)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string newDataPath =  $"{path}/{fileName}.asset";
        AssetDatabase.CreateAsset(data, newDataPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"'{fileName}' created successfully!", "OK");
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
    
    private void LoadTapeMusicNames(string folderPath)
    {
        List<string> musicList = new List<string>();
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath, "*.mp3", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                musicList.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        tapeMusicNames = musicList.ToArray();
    }

}
