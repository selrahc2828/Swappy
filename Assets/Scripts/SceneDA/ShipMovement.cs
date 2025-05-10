using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public AnimationCurve positionCurve;
    
    
    [SerializeField] private float x_duration;
    private float z_duration;

    private float _t_x;
    private float _t_z;
    
    private Vector3 StartPos;

    private void Start()
    {
        z_duration = x_duration * 2;
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _t_x += Time.deltaTime;
        _t_x %= x_duration; //Valeur boucle à duration
        float r_x = _t_x / x_duration; //Set la position en x
        
        _t_z += Time.deltaTime;
        _t_z %= z_duration; //Valeur boucle à duration
        float r_z = _t_z / z_duration; //Set la position en z
        
        transform.position = new Vector3(StartPos.x + positionCurve.Evaluate(r_x), StartPos.y + positionCurve.Evaluate(r_z), StartPos.z );
    }

    
}
