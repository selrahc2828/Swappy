using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckNecessary : MonoBehaviour
{
    public TextMeshProUGUI manqueGMText;

    void Start()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            manqueGMText.gameObject.SetActive(true);
            Debug.LogWarning("Aucun GameManager dans la scène");
        }
    }
}
