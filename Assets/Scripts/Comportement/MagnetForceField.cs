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
    private float _timer;
    public bool boolBurst;
    
    public MeshRenderer magnetFeedbackMaterial;
    public Color burstColor;
    public Color normalColor;

    private void Start()
    {
        normalColor = magnetFeedbackMaterial.material.GetColor("_Color0");
    }

    private void Update()
    {
        // Réduction du cooldown si nécessaire
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
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
                // _sm.rend.materials[1].SetColor("_Color2", colorSlot1);

                magnetFeedbackMaterial.material.SetColor("_Color0", burstColor);
                // delay pour remettre normalColor
                // magnetFeedbackMaterial.material.SetColor("_Color0", normalColor);

                boolBurst = false;
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
        if (_timer <= 0)
        {
            boolBurst = true;
            _timer = intervalBetweenBurst;
        }
        else
        {
            Debug.Log("Burst BounceMagnet en cooldown.");
        }
    }
}
