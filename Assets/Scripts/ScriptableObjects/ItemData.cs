using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "newItem", menuName = "Scriptable/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    
    public string itemName;
    public string itemDescription; 
    public Sprite itemSprite;
    public GameObject itemPrefab;
    
}
