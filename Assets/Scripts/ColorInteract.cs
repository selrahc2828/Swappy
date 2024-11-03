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


    // Start is called before the first frame update
    void Start()
    {
        _defaultColor = GameManager.Instance.defaultColor;
        _interactAVolerMat = GameManager.Instance.interactAVolerMat;
        _interactNOTPossibleMat = GameManager.Instance.interactNOTPossibleMat;
        _interactRienAVolerMat = GameManager.Instance.interactRienAVolerMat;

        //si pas de champs renseigné, on lui ajoute lui donne le material sur l'objet, sinon blanc
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
                    // si un comportement est à voler 
                    GetComponent<MeshRenderer>().material = _interactAVolerMat;

                }
                else
                {
                    //si rien à voler
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
