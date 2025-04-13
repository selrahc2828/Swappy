using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TapeMenu : MonoBehaviour
{
    private TapeData selectedTape;
    [SerializeField] private TextMeshProUGUI nameSelectedTape;
    [SerializeField] private ScrollRect scrollRect;
    
    
    public event Action<string> onPlaySelected;
    public event Action<string> onStopSelected;
    public event Action<bool> onMute;
    public bool isMuted;
    public GameObject muteImage;
    public GameObject notMuteImage;

    
    public void SetSelectedTape(TapeData tape)
    {
        selectedTape = tape;
        nameSelectedTape.text = tape.itemName;
    }


    public void PlayTape()
    {
        if (selectedTape == null)
            return;
        
        Debug.Log($"FMOD MANAGER PLAY: {selectedTape.itemName}");
        onPlaySelected?.Invoke(selectedTape.itemName);// emet le nom du son pour FMOD
    }
    
    public void StopTape()
    {
        if (selectedTape == null)
            return;
        
        Debug.Log($"FMOD MANAGER STOP: {selectedTape.itemName}");
        onStopSelected?.Invoke(selectedTape.itemName);
    }

    public void MuteEnvironment()
    {
        Debug.Log($"FMOD MANAGER MUTE ENVIRO");

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
        
        Debug.Log($"CenterOnButton");

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

}
