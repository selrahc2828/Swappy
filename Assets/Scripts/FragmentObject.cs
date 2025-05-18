using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// [RequireComponent(typeof(LineRenderer))]
public class FragmentObject : MonoBehaviour
{
    [SerializeField] private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }
}
