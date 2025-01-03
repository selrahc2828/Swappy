using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmuableAutoMesh : MonoBehaviour
{
    public MeshFilter meshFilter;
    
    // Start is called before the first frame update
    void Start()
    {
        meshFilter.mesh = GetComponentInParent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
