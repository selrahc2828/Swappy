using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum Category
    {
        Ressource,
        Souvenir,
        Aventure
    }

    public int itemId;
    public string itemName;
    public Sprite itemSprite;

    public Category itemCategory;
    public string itemDescription;   
    
}
