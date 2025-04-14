using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopupTapeGroup : MonoBehaviour
{
    public float duration = 1.5f; // durée de l'animation
    public CanvasGroup canvasGroup;
    public AnimationCurve fadeCurve; // controle du fade
    
    public TextMeshProUGUI textName;
    public Image icon;

    private void Awake()
    {
    }

    void Start()
    {
        StartCoroutine(Pop());
    }

    IEnumerator Pop()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            // Fade out
            canvasGroup.alpha = 1f - fadeCurve.Evaluate(t);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }
}
