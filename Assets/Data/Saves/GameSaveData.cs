using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public List<SpawnPotSaveData> spawnerPots = new();
    public PlayerSaveData player = new();
    public FragmentSaveData fragment;
}

[System.Serializable]
public class SpawnPotSaveData
{
    public string id;
    public bool isBroken;
}

[System.Serializable]
public class PlayerSaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
}

[System.Serializable]
public class FragmentSaveData
{
    public int fragmentBank;
    public int hasCollect;
}