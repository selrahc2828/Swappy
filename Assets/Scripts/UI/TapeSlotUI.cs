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
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    [SerializeField] private TapeMenu tapeMenu; 
    
    [SerializeField] ScrollRect scrollRect;
    public ScrollRect ScrollRect
    {
        get => scrollRect;
        set => scrollRect = value;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(() => {
            tapeMenu.CenterButton(transform as RectTransform);
        });
        
        
        if (musicData == null)
        {
            //button.interactable  = false;
            return;
        }
        
        if (musicData?.isUnlocked == false)
        {
            // nameText.text = $"???";
            //button.interactable  = false;
        }
        else
        {
            nameText.text = musicData?.itemName;
            button.interactable  = true;
            button.onClick.AddListener(() => {
                SendMusic();
                tapeMenu.SetSelectedTape(musicData);
                // tapeMenu.CenterButton(transform as RectTransform);
            });
            
        }
    }

    public void SendMusic()
    {
        Debug.Log($"Tape click music: {musicData.name}");
    }
}
