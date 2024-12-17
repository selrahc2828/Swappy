using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowToRadius : MonoBehaviour
{
    // � fournit � l'instantiate
    [HideInInspector]
    public float targetRadius;

    public float durationScaling;
    private float elapsedTime = 0f;
    private Vector3 _initialScale;
    private Vector3 _targetScale;
    private bool isScaling;

    // Start is called before the first frame update
    void Start()
    {
        _initialScale = transform.localScale;

        _targetScale = new Vector3(targetRadius * 2, targetRadius * 2, targetRadius * 2); //* 2 pour appliquer le diametre pas le rayon
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float progress = elapsedTime / durationScaling;

        if (elapsedTime < durationScaling)
        {
            transform.localScale = Vector3.Lerp(_initialScale, _targetScale, progress);

        }
        else
        {
            Destroy(gameObject);
        }

    }



}
