using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopupFragmentGroup : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI textQuantity;
    public Image icon;
    public CanvasGroup canvasGroup;
    
    [Header("Animation")]
    public AnimationCurve fadeInCurve; // quand le popup apparait
    public AnimationCurve fadeOutCurve;// quand le popup disparait
    public float fadeInDuration = 0.3f;// duree de la transition du fade
    
    public float duration = 1.5f; // durée avant de disparaitre
    // public float distance = 50f; // distance à parcourir

    // Coroutine
    private float _elapsedTime;
    private Coroutine _coroutine;
    
    private int _quantityAddAmount;
    
    public Action OnEndDisplay; // pour reset _activeFragmentPopup du canvasManager

    bool _hasFadedIn = false;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    IEnumerator Display()
    {
        _hasFadedIn = false;
        
        while (_elapsedTime < duration)
        {
            // n'a pas encore fait de fade in
            if (!_hasFadedIn & _elapsedTime < fadeInDuration)
            {
                float t = _elapsedTime / fadeInDuration;
                canvasGroup.alpha = fadeInCurve.Evaluate(t);
            }
            else
            {
                if (!_hasFadedIn)
                {
                    // fade in fini, on s'assure que les valeur son bonne
                    _hasFadedIn = true;
                    canvasGroup.alpha = 1f;
                }
                
                float t = (_elapsedTime - fadeInDuration) / (duration - fadeInDuration);
                canvasGroup.alpha = fadeOutCurve.Evaluate(1f - t);
            }

            _elapsedTime += Time.deltaTime;

            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        OnEndDisplay?.Invoke();
        Destroy(gameObject);
    }

    public void IncreaseQuantityAmount()
    {
        _quantityAddAmount++;
        textQuantity.text = $"+ " + _quantityAddAmount;

        _elapsedTime = 0;
        
        if (_coroutine == null)
            _coroutine = StartCoroutine(Display());
    }
}
