using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MagnetForceField : MonoBehaviour
{
    public float force;
    public MeshRenderer magnetFeedbackMaterial;
    public bool affectedPlayer;
    
    [Header("BounceMagnet")]
    public float burstForce;
    public float intervalBetweenBurst;
    public float _timerBurst;
    public bool boolBurst;
    public Color burstColor;
    public Color normalColor;
    public float delayDisplay;
    [SerializeField] float _timerDisplay;

    private void Start()
    {
        normalColor = magnetFeedbackMaterial.material.GetColor("_Color0");
    }

    private void Update()
    {
        if (_timerBurst > 0)
        {
            _timerBurst -= Time.deltaTime;
        }
        else
        {
            boolBurst = false;
        }
        
        DisplayColor();
    }

    private void OnTriggerStay(Collider other)
    {
        if (affectedPlayer)//on va chercher le RB dans le parent pour le player
        {
            Rigidbody _rbPlayer = other.GetComponentInParent<Rigidbody>();

            if (_rbPlayer != null)
            {
                ApplyForce(_rbPlayer,other.gameObject);
            }
        }
        
        Rigidbody _rb = other.GetComponent<Rigidbody>();
        if (_rb != null)
        {
            ApplyForce(_rb,other.gameObject);

            if (boolBurst)// magnet bounce
            {
                ApplyForce(_rb,other.gameObject, true);
            }
        }
    }

    void ApplyForce(Rigidbody rbObj,GameObject objToApply, bool burst = false)
    {
        Vector3 dir = (transform.position - objToApply.transform.position).normalized;
        
        if (!burst)// magnet normal
        {
            rbObj.AddForce(dir * force, ForceMode.Force);
            // Debug.DrawRay(objToApply.transform.position, dir*5, Color.green);

        }
        else // magnet bounce => burst
        {
            rbObj.AddForce(dir * burstForce, ForceMode.Impulse);
            Debug.DrawRay(objToApply.transform.position, dir*5, Color.red);
        }
    }

    public void Bounce()
    {
        // boolBurst = false dans le comportement CollisionEnd
        if (_timerBurst <= 0)
        {
            boolBurst = true;
            //reset des temps
            _timerBurst = intervalBetweenBurst;
            _timerDisplay = delayDisplay;
        }
        else
        {
            Debug.Log("Burst BounceMagnet en cooldown.");
        }
    }

    public void DisplayColor()
    {
      
        if (_timerDisplay >= 0)
        {
            _timerDisplay -= Time.deltaTime;
            magnetFeedbackMaterial.material.SetColor("_Color0", burstColor);
        }
        else
        {
            magnetFeedbackMaterial.material.SetColor("_Color0", normalColor);
            boolBurst = false;
        }
    }
}
