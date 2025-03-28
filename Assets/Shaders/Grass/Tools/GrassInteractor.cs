using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassInteractor : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_positionMoving", transform.position);
    }
}
