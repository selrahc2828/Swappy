using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public Color _color;
    private MaterialPropertyBlock propertyBlock;
    
    
    // Start is called before the first frame update
    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        propertyBlock.SetColor("_Color", _color);
        
        //GetComponent<MeshRenderer>().materials[1].GetPropertyNames;
    }
}
