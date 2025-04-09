using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TapeSystem : MonoBehaviour
{

    [SerializeField] private int nbTapes = 10;

    public int NbTapes => nbTapes;

    [SerializeField] private List<TapeData> tapeList = new List<TapeData>();

    public List<TapeData> TapeList
    {
        get => tapeList;
        //set => inventoryItems = value;
    }
    
    //emet event
    public event Action OnAddTapeList;
    public event Action OnRemoveTapeList;
    public event Action OnTapeUnlockStatusChanged;

    public void AddTape(TapeData newTape)
    {
        Debug.Log($"Adding tape {newTape}");

        tapeList.Add(newTape);
        OnAddTapeList?.Invoke();
    }

    public void RemoveTape(TapeData tapeToRemove)
    {
        tapeList.Remove(tapeToRemove);
        OnRemoveTapeList?.Invoke();
    }

    public void SetLockTape(TapeData tapeToSet, bool lockTape)
    {
        if (!tapeList.Contains(tapeToSet))
        {
            AddTape(tapeToSet);
        }
        
        int index = tapeList.IndexOf(tapeToSet);
        tapeList[index].isUnlocked = lockTape;
        
        OnTapeUnlockStatusChanged?.Invoke();
    }
}
