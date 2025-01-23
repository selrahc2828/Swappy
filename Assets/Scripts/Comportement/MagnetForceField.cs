using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MagnetForceField : MonoBehaviour
{
    public float force;
    public float burstForce;
    public float intervalBetweenBurst;
    private float _timerBurst;
    public bool boolBurst;
    
    public MeshRenderer magnetFeedbackMaterial;
    public Color burstColor;
    public Color normalColor;
    public float delayDisplay;
    float _timerDisplay;

    private void Start()
    {
        normalColor = magnetFeedbackMaterial.material.GetColor("_Color0");
    }

    private void Update()
    {
        // Réduction du cooldown si nécessaire
        if (_timerBurst > 0)
        {
            _timerBurst -= Time.deltaTime;
        }

        DisplayColor();
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody _rb = other.GetComponent<Rigidbody>();
        if (_rb != null)
        {
            ApplyForce(_rb,other.gameObject);
            
            if (boolBurst)// magnet bounce
            {
                ApplyForce(_rb,other.gameObject);
                // boolBurst = false;
            }
        }
    }

    void ApplyForce(Rigidbody rbObj,GameObject objToApply)
    {
        Vector3 dir = (transform.position - objToApply.transform.position).normalized;
        Debug.DrawRay(objToApply.transform.position, dir*5, Color.green);
        
        if (!boolBurst)// magnet normal
        {
            rbObj.AddForce(dir * force, ForceMode.Force);
        }
        else // magnet bounce
        {
            rbObj.AddForce(dir * burstForce, ForceMode.Impulse);
        }
    }

    public void Bounce()
    {
        if (_timerBurst <= 0)
        {
            boolBurst = true;
            _timerBurst = intervalBetweenBurst;
        }
        else
        {
            Debug.Log("Burst BounceMagnet en cooldown.");
        }
    }

    public void DisplayColor()
    {
        if (boolBurst)
        {
            _timerDisplay = delayDisplay;
        }
        if (_timerDisplay >= 0)
        {
            _timerDisplay -= Time.deltaTime;
            magnetFeedbackMaterial.material.SetColor("_Color0", burstColor);
        }
        else
        {
            magnetFeedbackMaterial.material.SetColor("_Color0", normalColor);
        }
        Debug.Log($"timer display: {_timerDisplay}");

    }
}
