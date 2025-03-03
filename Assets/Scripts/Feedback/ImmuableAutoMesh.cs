using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmuableAutoMesh : MonoBehaviour
{
    public MeshFilter meshFilter;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<Transform>().GetComponentInChildren<MeshFilter>().tag == "Untagged")
        {
            meshFilter.mesh = GetComponentInParent<GameObject>().GetComponentInChildren<MeshFilter>().mesh;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
