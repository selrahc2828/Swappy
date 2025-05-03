using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDescription; 
    public Sprite itemSprite;
    public GameObject itemPrefab;
}
