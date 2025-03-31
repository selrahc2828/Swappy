using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private bool activeOnAwake;

    private void Awake()
    {
        gameObject.SetActive(activeOnAwake);
    }
}
