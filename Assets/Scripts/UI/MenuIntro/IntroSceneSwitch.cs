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
    [Header("scene")]
    public Scene NextScene;

    [Header("Transition Black Screen")]
    public GameObject BlackScreen;
    public AnimationCurve blackFadeINCurve;
    public float fadeINTimeRange = 9;
    private float _t = 0;
    
    public AnimationCurve blackFadeOUTCurve;
    public float fadeOUTTimeRange = 5;

    private bool playClicked = false;
    
    [Header("Transition UI menu")]
    public AnimationCurve UIFadeINCurve;
    public GameObject playButton;
    public GameObject parameterButton;
    public GameObject quitButton;
    public GameObject title;
    public float timeRangeToButton = 2.3f;
    private float _tForButtons = 0;
    
    public float timerCinematicBeforePlay = 10;
    public float timerCinematic = 0;
    private bool isCinematicPlayed = false;

    private void Start()
    {
        FMODMusicManager.instance.CreateMusicInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO);
        FMODMusicManager.instance.PlayMusicInstance(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));
        
        playButton.SetActive(false);
        parameterButton.SetActive(false);
        quitButton.SetActive(false);
        title.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (playClicked)
        {
            if (_t / fadeOUTTimeRange <= 1 )
            {
            
                BlackFadeOut();
            }
            else
            {
                if (!isCinematicPlayed)
                {
                    FMODEventManager.instance.PlayOneShot(FMODEventManager.instance.FMODEvents.TransistionCrash, transform.position);
                    isCinematicPlayed = true;
                }
                timerCinematic += Time.fixedDeltaTime;
                if (timerCinematic >= timerCinematicBeforePlay) SceneManager.LoadScene(1, LoadSceneMode.Single);
                
                
            }
            
            
        }
        else
        {
            if (_t / fadeINTimeRange <= 1 )
            {
            
                BlackFadeIn();
            }
            // else
            // {
            //     title.SetActive(true);
            //     _tForButtons += Time.deltaTime;
            //
            //     if (_tForButtons / timeRangeToButton >= 1)
            //     {
            //         playButton.SetActive(true);
            //         parameterButton.SetActive(true);
            //         quitButton.SetActive(true);
            //     }
            //
            //     // todo apparition progressive des buttons
            //
            // }
        }

        if (FMODMusicManager.instance.GetMusicTimelineValue(
                FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO)) >
            8823)
        {
            title.SetActive(true);
        }
        if (FMODMusicManager.instance.GetMusicTimelineValue(
                FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO)) >
            11971 )
        {
            playButton.SetActive(true);
            //parameterButton.SetActive(true);
            //quitButton.SetActive(true);
        }

        if (FMODMusicManager.instance.GetMusicTimelineValue(
                FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO)) >
            15313)
        {
            parameterButton.SetActive(true);
        }
        
        if (FMODMusicManager.instance.GetMusicTimelineValue(
                FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO)) >
            18424)
        {
            quitButton.SetActive(true);
        }
        

        

       
        
        
        
    }

    public void OnPlayButtonClicked()
    {
        playButton.SetActive(false);
        parameterButton.SetActive(false);
        quitButton.SetActive(false);
        title.SetActive(false);
        
        _t = 0;
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
