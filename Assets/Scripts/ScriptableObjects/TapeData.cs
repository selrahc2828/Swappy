using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTape", menuName = "ScriptableObjects/TapeData")]
public class TapeData : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite itemSprite;
    public bool isUnlocked = false;
    
}
