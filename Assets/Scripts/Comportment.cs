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

    void Start()
    {
        stealable = true;
        Debug.Log("Comportment : " + this);
        
        if (GetComponents<C_Spawner>() != null)
        {
            foreach (C_Spawner spawnScript in GetComponents<C_Spawner>())
            {
                if (spawnScript.enabled == true)
                {
                    spawnScript.ReferenceComportments(this);
                    Debug.Log("Issue with ReferenceComportments() function in: " + this.name);
                }
            }
        }
    }
}
