using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupItemGroup : MonoBehaviour
{
    public float duration = 1.5f; // durée de l'animation
    public float distance = 50f; // distance à parcourir
    public CanvasGroup canvasGroup;
    public AnimationCurve fadeCurve; // controle du fade
    private RectTransform rectTransform;
    private Vector2 startPos;
    
    public TextMeshProUGUI textQuantity;
    public Image icon;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;

    }

    void Start()
    {
        StartCoroutine(MovePickItem());
    }

    IEnumerator MovePickItem()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            
            // Fade out
            canvasGroup.alpha = 1f - fadeCurve.Evaluate(t);

            // Move upward
            rectTransform.anchoredPosition = startPos + Vector2.up * (distance * t);

            elapsed += Time.deltaTime;
            yield return null;
            
        }
        
        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }
}
