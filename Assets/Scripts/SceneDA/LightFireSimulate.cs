using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFireSimulate : MonoBehaviour
{
    
    public AnimationCurve lightIntensity;
    private Light _light;

    public float timeRange;
    private float _t;
    


    private void Start()
    {
        _light = GetComponent<Light>();
    }

    void Update()
    {
        _t += Time.deltaTime;
        _t %= timeRange; //Valeur boucle à duration
        float r = _t / timeRange; //Ratio de 0 à 1 (valeur normalisée)

        _light.intensity = lightIntensity.Evaluate(r);
    }
}
