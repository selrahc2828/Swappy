using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSD : MonoBehaviour
{
    [SerializeField]
    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
     _soundManager = FindAnyObjectByType<SoundManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _soundManager.PlayPlayerSound(SoundManager.SoundPlayer.slowTime);
        }
        
    }
}
