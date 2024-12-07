using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckNecessary : MonoBehaviour
{
    public GameObject manqueGMText;

    void Start()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            manqueGMText = GameObject.FindGameObjectWithTag("Necessary");
            if (manqueGMText != null)
            {
                manqueGMText.GetComponent<TextMeshProUGUI>().enabled = true;
            }
            Debug.LogWarning("Aucun GameManager dans la sc�ne");
        }
    }
}
