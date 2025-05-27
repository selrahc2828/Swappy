using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using FMOD.Studio;
using FMODUnity;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class IntroSceneSwitch: MonoBehaviour
{
    
    public Scene NextScene;

    public GameObject BlackScreen;
    public AnimationCurve blackFadeINCurve;
    public float fadeINTimeRange = 9;
    private float _t = 0;
    
    public AnimationCurve blackFadeOUTCurve;
    public float fadeOUTTimeRange = 5;

    private bool playClicked = false;
    
    public GameObject playButton;
    public GameObject parameterButton;
    public GameObject quitButton;
    public GameObject title;
    

    private void Start()
    {
        NextScene = SceneManager.GetSceneByBuildIndex(1);
        FMODMusicManager.instance.CreateMusicInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO);
        FMODMusicManager.instance.PlayMusicInstance(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));
        
        playButton.SetActive(false);
        parameterButton.SetActive(false);
        quitButton.SetActive(false);
        title.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_t / fadeINTimeRange <= 1)
        {
            Debug.Log(blackFadeINCurve.Evaluate(_t / fadeINTimeRange));
            BlackFadeIn();
        }
        else
        {
            Debug.Log("finig");
            playButton.SetActive(true);
            parameterButton.SetActive(true);
            quitButton.SetActive(true);
            title.SetActive(true);
            
        }

        

        if (playClicked)
        {
            BlackFadeOut();
            if (blackFadeOUTCurve.Evaluate(_t / fadeOUTTimeRange) >= fadeOUTTimeRange)
            {
                var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                SceneManager.UnloadSceneAsync(currentScene);
                SceneManager.LoadScene(NextScene.name);
            }
        }
        
    }

    public void OnPlayButtonClicked()
    {
        playClicked = true;
        BlackScreen.SetActive(true);
        // Stop la zic en fade de 5sec
        FMODMusicManager.instance.StopMusic(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));
        FMODMusicManager.instance.ReleaseMusicInstance(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));
        
        
    }

    public void BlackFadeIn()
    {
        _t += Time.deltaTime;
        
        BlackScreen.GetComponent<Image>().color = new Color(0, 0, 0, blackFadeINCurve.Evaluate(_t / fadeINTimeRange));
    }
    
    public void BlackFadeOut()
    {
        _t += Time.deltaTime;
        
        BlackScreen.GetComponent<Image>().color = new Color(0, 0, 0, blackFadeOUTCurve.Evaluate(_t / fadeOUTTimeRange));
    }
    
}
