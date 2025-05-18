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
        MainVolume = GameObject.Find("Volume").GetComponent<PostProcessVolume>();
        MainVolume.profile = MainProfile;
    }

    void ChangeProfiles(bool slow)
    {
        if (slow == true)
        {
            MainVolume.profile = SlowProfile;
        }

        if (slow == false)
        {
            MainVolume.profile = MainProfile;
        }
    }
}
