using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "FragmentBankData", menuName = "Scriptable/FragmentBankData", order = 0)]
public class FragmentBankData : ScriptableObject
{
    public string fragmentBankName;
    public Sprite fragmentBankIcon;
    public int bankInventoryFragmentQuantity; // incremente avec la quantity
    public int hasCollect; // stock les frgaments non recupere => recupere cette valeur de la sauvegarde au chargement
}
