using UnityEngine;

public enum Tier
{
    Low = 5,
    Medium = 10,
    High = 20
}

[CreateAssetMenu(fileName = "NewPotData", menuName = "Scriptable/PotData")]
public class PotData : ScriptableObject
{
    public GameObject normalPrefab;
    public GameObject brokenPrefab;
    public Tier tier;
}
