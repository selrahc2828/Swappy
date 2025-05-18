using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // permet de retranscrire pour format json
public class GameSaveData
{
    public List<SpawnPotSaveData> spawnerPots = new();//liste des spawner de pots de la scène qui sont cassé
    public PlayerSaveData player = new();
    public FragmentSaveData fragment;
}

[System.Serializable]
public class SpawnPotSaveData
{
    public string id;// identifiant unique du spawner
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