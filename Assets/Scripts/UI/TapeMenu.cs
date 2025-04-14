using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TapeMenu : MonoBehaviour
{
    private TapeData selectedTape;
    [SerializeField] private TextMeshProUGUI nameSelectedTape;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform tapesContent;
    [SerializeField] private GameObject tapeSlotPrefab;
    [SerializeField] private List<TapeData> tapeList = new List<TapeData>();
    public List<TapeData> TapeList
    {
        get => tapeList;
        set => tapeList = value;
    }

    
    public event Action<string> onPlaySelected;
    public event Action<string> onStopSelected;
    public event Action<bool> onMute;
    public bool isMuted;
    public GameObject muteImage;
    public GameObject notMuteImage;
    private VerticalLayoutGroup layout;


    void OnEnable()
    {
        layout = scrollRect.content.GetComponent<VerticalLayoutGroup>();

        SetTapeButtons();
        CenterMiddleButtonIfNoneSelected();
        AdjustScrollPadding();
    }

    public void SetTapeButtons()
    {
        // On supprime tous les slots existants
        foreach (Transform child in tapesContent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (TapeData tape in tapeList)
        {
            GameObject tapeButton = Instantiate(tapeSlotPrefab, tapesContent);
            TapeSlotUI tapeSlotUI = tapeButton.GetComponent<TapeSlotUI>();
            tapeSlotUI.Initialize(tape, this,SetSelectedTape, CenterButton);
        } 
    }

    public void AdjustScrollPadding()
    {
        // ajoute padding en haut et en bas pour pouvoir placer tous les boutons au centre si on clique dessus
        // (sinon les boutons les plus en haut et en bas ne le font pas)
        
        float viewportHalfHeight = scrollRect.viewport.rect.height / 2f; // centre du conten

        // On concidere que tous les enfants seront la meme prefab alors on récupere la hauteur de cette prefab
        float maxItemHeight = tapeSlotPrefab.GetComponent<RectTransform>().rect.height ;

        // Calcul du padding haut et bas
        int idealPadding = Mathf.RoundToInt(viewportHalfHeight - (maxItemHeight / 2f));//car bouton centré au milieux
        idealPadding = Mathf.Max(0, idealPadding);// au cas ou le bouton est trop gros et donne une valeur négative

        layout.padding.top = idealPadding;
        layout.padding.bottom = idealPadding;

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
    }
    
    public void SetSelectedTape(TapeData tape)
    {
        selectedTape = tape;
        nameSelectedTape.text = tape.itemName;
    }

    public void PlayTape()
    {
        if (selectedTape == null)
            return;
        
        onPlaySelected?.Invoke(selectedTape.itemName);// emet le nom du son pour FMOD
    }
    
    public void StopTape()
    {
        if (selectedTape == null)
            return;
        
        onStopSelected?.Invoke(selectedTape.itemName);
    }

    public void MuteEnvironment()
    {
        isMuted = !isMuted;
        
        onMute?.Invoke(isMuted);

        if (isMuted)
        {
            muteImage.SetActive(true);
            notMuteImage.SetActive(false);
        }
        else
        {
            muteImage.SetActive(false);
            notMuteImage.SetActive(true);
        }
    }
    
    public void CenterButton(RectTransform targetToMove)
    {
        // prefab
        // content pivot et boutons pivot X = 0.5, Y = 1
        // enfant ajouté de bas en haut, on place tout à l'origine en haut
        // pour simplifier les calculs de localPosition.y et normalizedPosition.y
        
        Canvas.ForceUpdateCanvases();
        
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;
        
        float targetCenterY = Mathf.Abs(targetToMove.localPosition.y) + (targetToMove.rect.height / 2);

        float normalizedY = (targetCenterY - viewportHeight / 2) / (contentHeight - viewportHeight);
        normalizedY = Mathf.Clamp01(normalizedY);

        scrollRect.normalizedPosition = new Vector2(0f, 1f - normalizedY);
    }

    public void CenterMiddleButtonIfNoneSelected()
    {
        if (selectedTape != null) return;
        if (tapeList == null || tapeList.Count == 0) return;
        
        int middleIndex = tapeList.Count / 2;
        if (middleIndex < scrollRect.content.childCount) // securite
        {
            RectTransform middleButton = scrollRect.content.GetChild(middleIndex) as RectTransform;
            if (middleButton != null)
            {
                CenterButton(middleButton);
            }
        }
    }
    
}
