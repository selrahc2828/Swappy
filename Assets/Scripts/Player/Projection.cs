using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Projection : MonoBehaviour
{
    public LayerMask hitLayer;
    public float projectionTimeDuration;
    private Ray _ray;
    private float _projectionTimer;

    public float coeffRefill = 0.75f;
    public float range;
    public float coeffReducDistance;
    private float _distance;
    
    [Header("Debug")]
    public TextMeshProUGUI timerProjectionText;

    public bool activeGizmoRange;

    private void Start()
    {
        _projectionTimer = projectionTimeDuration;
    }

    private void Update()
    {
        if (GameManager.Instance.etatIsProjected)
        {
            _projectionTimer -= Time.deltaTime;
        }
        else if (_projectionTimer < projectionTimeDuration)
        {
            _projectionTimer += Time.deltaTime * coeffRefill;
        }

        if (_projectionTimer <= 0)
        {
            ResetProjection();
        }

        _projectionTimer = Mathf.Clamp(_projectionTimer, 0, projectionTimeDuration);
        timerProjectionText.text = _projectionTimer.ToString("F2");// F2 arrondi à 2 décimal
    }

    public void ProjectionBehavior()
    {
        RaycastHit _hit;
        _ray = GameManager.Instance.camControllerScript.mainCamera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out _hit, range, hitLayer);
        
        // on peut se projeter
        if (!GameManager.Instance.etatIsProjected && _hit.collider != null && _projectionTimer > 0)
        {
            // on retire du temps de projection proportionnelle à la distance 
            _distance = Vector3.Distance(_hit.collider.transform.position, transform.position);
            float reduction = _distance * coeffReducDistance;
            Debug.Log("reduc : " + (_projectionTimer - reduction));

            if (_projectionTimer - reduction > 0)
            {
                _projectionTimer -= reduction;
                Debug.Log("Apply : " + (_projectionTimer));

                GameManager.Instance.etatIsProjected = true;
                GameManager.Instance.camControllerScript.ChangeFocusTarget(_hit.collider.transform);
            }
        }
        else if (GameManager.Instance.etatIsProjected)
        {
            ResetProjection();
        }
    }
    
    public void ResetProjection()
    {
        GameManager.Instance.etatIsProjected = false;
        GameManager.Instance.camControllerScript.ChangeFocusTarget(); //pos cam player
    }

    void OnDrawGizmos()
    {
        if (activeGizmoRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);            
        }
    }
    
}
