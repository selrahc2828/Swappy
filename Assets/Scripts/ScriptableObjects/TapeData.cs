using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "newTape", menuName = "Scriptable/TapeData", order = 0)]
public class TapeData : ItemData
{
    public EventReference musicEvent;
    public string musicFmodName;
    public bool isUnlocked = false;
}
