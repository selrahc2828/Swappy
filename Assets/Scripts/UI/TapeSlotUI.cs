using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapeSlotUI : MonoBehaviour
{
    [SerializeField] private TapeData musicData;
    public TapeData MusicData
    {
        get => musicData;
        set => musicData = value;
    }
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    [SerializeField] private TapeMenu tapeMenu;

    public TapeMenu TapeMenu
    {
        get => tapeMenu;
        set => tapeMenu = value;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(() => {
            tapeMenu.CenterButton(transform as RectTransform);
        });
        
        
        if (musicData == null)
        {
            button.interactable  = false;
            nameText.text = $"??? - Pas de TapeData";
            return;
        }
        
        if (musicData?.isUnlocked == false)
        {
            nameText.text = $"???";
            button.interactable  = false;
        }
        else
        {
            nameText.text = musicData?.itemName;
            button.interactable  = true;
            button.onClick.AddListener(() => {
                tapeMenu.SetSelectedTape(musicData);
                tapeMenu.CenterButton(transform as RectTransform);
            });
            
        }
    }

}
