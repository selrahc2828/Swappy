using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ListComp
{
    Null,
    Mvt,
    Fixe,
    Destruct
}
//[ExecuteInEditMode]
public class Comportment : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public ListComp typeComp;
    public bool stealable = true;
    public GameObject originalOwner;
}
