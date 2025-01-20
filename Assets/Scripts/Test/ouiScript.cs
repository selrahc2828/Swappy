using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ouiScript : MonoBehaviour
{
    
    public static ouiScript Instance;
    public static Controls controls;
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        controls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
