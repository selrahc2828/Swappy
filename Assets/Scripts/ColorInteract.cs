using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInteract : MonoBehaviour
{

    private Material _startColor;
    private Material _defaultColor;

    private Material _interactAVolerMat;
    private Material _interactRienAVolerMat;
    private Material _interactNOTPossibleMat;

    private bool _actif;

    private Color _Color3;
    private Color _Color2;
    private Material material;


    // Start is called before the first frame update
    void Start()
    {
        _Color3 = GameManager.Instance.Uncomportemented_color;
        _Color2 = GameManager.Instance.Uncomportemented_color;
        material = GetComponent<MeshRenderer>().materials[1];

        material.SetColor("_Color3", _Color3);
        material.SetColor("_Color2", _Color2);
        
        _defaultColor = GameManager.Instance.defaultColor;
        _interactAVolerMat = GameManager.Instance.interactAVolerMat;
        _interactNOTPossibleMat = GameManager.Instance.interactNOTPossibleMat;
        _interactRienAVolerMat = GameManager.Instance.interactRienAVolerMat;
        

        //si pas de champs renseign�, on lui ajoute lui donne le material sur l'objet, sinon blanc
        //Debug.Log("_startColor : " + _startColor);
        if (_startColor == null)
        {
            if (GetComponent<MeshRenderer>() != null )
            {
                _startColor = GetComponent<MeshRenderer>().material;
            }
            else
            {
                _startColor = _defaultColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Color3 == GameManager.Instance.Uncomportemented_color)
        {
            if (GetComponents<Repulse>().Length == 1)
            {
                material.SetColor("_Color3", GameManager.Instance.Repulsive_color);
            
            }
            else if (GetComponents<Immuable>().Length == 1)
            {
                material.SetColor("_Color3", GameManager.Instance.immuable_color);
            
            }
            else if (GetComponents<Fusée>().Length == 1)
            {
                material.SetColor("_Color3", GameManager.Instance.fusee_color);
            
            }
            else if (GetComponents<Magnet_Effect>().Length == 1)
            {
                material.SetColor("_Color3", GameManager.Instance.aimant_color);
            
            }
            else if (GetComponents<Physics_Stealable>().Length == 1)
            {
                material.SetColor("_Color3", GameManager.Instance.Rebond_color);
            
            }
        }
        if (_Color2 == GameManager.Instance.Uncomportemented_color)
        {
            if (GetComponents<Repulse>().Length == 2)
            {
                material.SetColor("_Color2", GameManager.Instance.Repulsive_color);
            
            }
            else if (GetComponents<Immuable>().Length == 2)
            {
                material.SetColor("_Color2", GameManager.Instance.immuable_color);
            
            }
            else if (GetComponents<Fusée>().Length == 2)
            {
                material.SetColor("_Color2", GameManager.Instance.fusee_color);
            
            }
            else if (GetComponents<Magnet_Effect>().Length == 2)
            {
                material.SetColor("_Color2", GameManager.Instance.aimant_color);
            
            }
            else if (GetComponents<Physics_Stealable>().Length == 2)
            {
                material.SetColor("_Color2", GameManager.Instance.Rebond_color);
            
            }
        }
        
        
        

        if (GetComponent<Comportment>() != null)
        {
            _actif = true;
        }
        else
        {
            _actif = false;
        }

        if (GetComponent<MeshRenderer>() != null)
        {
            if (Time.timeScale != 1)
            {
                if (gameObject.CompareTag("NotInteract"))
                {
                    GetComponent<MeshRenderer>().material = _interactNOTPossibleMat;
                }
                else if (_actif)
                {
                    // si un comportement est � voler 
                    GetComponent<MeshRenderer>().material = _interactAVolerMat;

                }
                else
                {
                    //si rien � voler
                    GetComponent<MeshRenderer>().material = _interactRienAVolerMat;
                }
            }
            else
            {
                GetComponent<MeshRenderer>().material = _startColor;
            }
        }
        
    }
}
