using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTape", menuName = "Scriptable/TapeData", order = 0)]
public class TapeData : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite itemSprite;
    public bool isUnlocked = false;
    
}
