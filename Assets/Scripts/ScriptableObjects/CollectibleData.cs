using UnityEngine;

[CreateAssetMenu(fileName = "newCollectible", menuName = "Scriptable/CollectibleData", order = 0)]
public class CollectibleData : ItemData
{
    public enum Category
    {
        Ressource,
        Souvenir,
        Aventure
    }
    public Category itemCategory;
}
