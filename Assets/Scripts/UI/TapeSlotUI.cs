using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapeSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
     [SerializeField] private Button button;

     private void OnEnable()// pour test en inspecteur si on le set et pas ajout dynamique
    {
       
    }
    
    public void Initialize(TapeData tapeData, TapeMenu menu, Action<TapeData> onClickCallback, Action<RectTransform> onClickCallback2)
    {
        if (tapeData == null)
        {
            button.interactable  = false;
            nameText.text = $"??? - Pas de TapeData";
            return;
        }
        
        nameText.text = tapeData.isUnlocked ? tapeData.itemName : "???";
        button.interactable = tapeData.isUnlocked;
        
        if (tapeData.isUnlocked)
        {
            button.onClick.RemoveAllListeners();
            RectTransform rectTransform = transform as RectTransform;
            button.onClick.AddListener(() =>
            {
                onClickCallback?.Invoke(tapeData); // SetSelectedTape
                onClickCallback2?.Invoke(rectTransform); // CenterButton
            });  
        }
    }
}
