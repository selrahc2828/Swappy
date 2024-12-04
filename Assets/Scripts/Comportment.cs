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

    void Awake()
    {      
        if (GetComponent<C_Spawner>()) //v�rifie si un spawner est pr�sent sur l'objet (� remplacer plus tard par un type de comportement "Takeover" si on a + que spawner)
        {
            foreach (C_Spawner spawnScript in GetComponents<C_Spawner>())
            {
                if (spawnScript.enabled && spawnScript != this)
                {
                    spawnScript.ReferenceComportments(this); //appel le script du spawner permettant de r�f�rencer ce comportement en tant que comportement enfant et de le d�sactiver
                    return;
                }
            }
        }
    }
}
