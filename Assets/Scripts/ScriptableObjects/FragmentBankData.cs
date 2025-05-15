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
}
