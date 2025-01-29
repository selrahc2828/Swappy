using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemData", menuName = "Scriptable/SystemData", order = 0)]
public class SystemData : ScriptableObject
{
    public float sensitivity = 1f;
    
    public float volumeMaster = 1f;
    public float volumePlayer = 1f;
    public float volumeSystem = 1f;
    public float volumeMusic = 1f;
    public float volumeMenu = 1f;
}
