using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SlowTimeFeedback : MonoBehaviour
{

    public PostProcessVolume MainVolume;
    public PostProcessProfile MainProfile;
    public PostProcessProfile SlowProfile;
    private void Start()
    {
        //MainVolume = GameObject.Find("Volume").GetComponent<PostProcessVolume>();
        //MainVolume.profile = MainProfile;
    }

    private void OnEnable()
    {
        GlobalEventManager.Instance.OnSlowMotionInput += ChangeProfiles;
    }
    private void OnDisable()
    {
        GlobalEventManager.Instance.OnSlowMotionInput -= ChangeProfiles;
    }

    void ChangeProfiles(bool slow)
    {
        Debug.Log(MainVolume.profile.name  );
        if (slow == true)
        {
            MainVolume.profile = SlowProfile;
        }
        else
        {
            MainVolume.profile = MainProfile;

        }
    }
}
