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
    }
    
    //emet event

    public void AddTape(TapeData newTape)
    {
        Debug.Log($"Adding tape {newTape}");

        tapeList.Add(newTape);
        GlobalEventManager.Instance.AddTape();
    }

    public void RemoveTape(TapeData tapeToRemove)
    {
        tapeList.Remove(tapeToRemove);
        GlobalEventManager.Instance.RemoveTape();

    }

    public void SetLockTape(TapeData tapeToSet, bool lockTape)
    {
        if (!tapeList.Contains(tapeToSet))
        {
            AddTape(tapeToSet);
        }
        
        int index = tapeList.IndexOf(tapeToSet);
        tapeList[index].isUnlocked = lockTape;
        
        GlobalEventManager.Instance.SetStateTape();
        GlobalEventManager.Instance.DisplayPopupPickUpTape(tapeToSet);
    }
}
