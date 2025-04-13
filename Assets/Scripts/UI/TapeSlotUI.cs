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
    
     // [SerializeField] private TapeData musicData;
    // public TapeData MusicData
    // {
    //     get => musicData;
    //     set => musicData = value;
    // }
    // [SerializeField] private TapeMenu tapeMenu; // pour test en inspecteur si on le set et pas ajout dynamique
    
    private void OnEnable()// pour test en inspecteur si on le set et pas ajout dynamique
    {
        #region pour test en inspecteur si on le set et pas ajout dynamique
        // button.onClick.AddListener(() => {
        //     tapeMenu.CenterButton(transform as RectTransform);
        // });
        //
        //
        // if (musicData == null)
        // {
        //     button.interactable  = false;
        //     nameText.text = $"??? - Pas de TapeData";
        //     return;
        // }
        //
        // if (musicData?.isUnlocked == false)
        // {
        //     nameText.text = $"???";
        //     button.interactable  = false;
        // }
        // else
        // {
        //     nameText.text = musicData?.itemName;
        //     button.interactable  = true;
        //     button.onClick.AddListener(() => {
        //         tapeMenu.SetSelectedTape(musicData);
        //         tapeMenu.CenterButton(transform as RectTransform);
        //     });
        //     
        // }
        

        #endregion
    }
    
    public void Initialize(TapeData tapeData, TapeMenu menu, Action<TapeData> onClickCallback, Action<RectTransform> onClickCallback2)
    {
        // button.onClick.AddListener(() => {
        //     onClickCallback2?.Invoke(transform as RectTransform);
        // });
        
        if (tapeData == null)
        {
            button.interactable  = false;
            nameText.text = $"??? - Pas de TapeData";
            return;
        }
        
        if (tapeData?.isUnlocked == false)
        {
            nameText.text = $"???";
            button.interactable  = false;
        }
        else
        {
            nameText.text = tapeData?.itemName;
            button.interactable  = true;
            button.onClick.AddListener(() =>
            {
                onClickCallback?.Invoke(tapeData); //SetSelectedTape
                onClickCallback2?.Invoke(transform as RectTransform); //CenterButton
            });  
            
        }
    }

}
