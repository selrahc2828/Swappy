using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisationGPS : MonoBehaviour
{
    public GameObject miniMapPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        miniMapPlayer.transform.rotation = Quaternion.Euler(
                                                GameManager.Instance.player.transform.rotation.eulerAngles.x, 
                                                GameManager.Instance.player.transform.rotation.eulerAngles.y, 
                                                GameManager.Instance.player.transform.rotation.eulerAngles.z);
    }
}
