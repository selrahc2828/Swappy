using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Presentation : MonoBehaviour
{
    public GameObject player;
    public GameObject TP_1;
    public GameObject TP_2;
    public GameObject TP_3;
    public bool TPenabled;
    public ControllerPlanete controller;
    public GravityPlanete gravity;
    // Start is called before the first frame update
    void Start()
    {
        TPenabled = false;
        player = GameManager.Instance.player;
        controller = player.GetComponent<ControllerPlanete>();
        gravity = player.GetComponent<GravityPlanete>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (TPenabled)
            {
                controller.enabled = true;
                gravity.enabled = true;
                TPenabled = false;
            }
            else
            {
                controller.enabled = false;
                gravity.enabled = false;
                TPenabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            player.transform.position = TP_1.transform.position;
            player.transform.rotation = TP_1.transform.rotation;
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            player.transform.position = TP_2.transform.position;
            player.transform.rotation = TP_2.transform.rotation;
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            player.transform.position = TP_3.transform.position;
            player.transform.rotation = TP_3.transform.rotation;
        }
    }
}
