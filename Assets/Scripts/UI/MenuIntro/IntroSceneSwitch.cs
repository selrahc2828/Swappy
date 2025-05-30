using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;
using TMPro;

public class IntroSceneSwitch: MonoBehaviour
{
    [Header("Transition Black Screen")]
    public GameObject BlackScreen;
    public AnimationCurve blackFadeINCurve;
    public float fadeINTimeRange = 9;
    private float _t = 0;
    
    public AnimationCurve blackFadeOUTCurve;
    public float fadeOUTTimeRange = 5;

    private bool playClicked = false;
    
    [Header("Transition UI menu")]
    public GameObject playButton;
    public GameObject parameterButton;
    public GameObject quitButton;
    public GameObject title;
    public GameObject parametrePanel;
    private float _tForButtons = 0;
    
    public float timerCinematicBeforePlay = 10;
    public float timerCinematic = 0;
    private bool isCinematicPlayed = false;
    
    [Header("Option Menu Slider")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSliderMaster;
    [SerializeField] private Slider volumeSliderPlayer;
    [SerializeField] private Slider volumeSliderSystem;
    [SerializeField] private Slider volumeSliderMusic;
    [SerializeField] private Slider volumeSliderMenu;
    [SerializeField] private TextMeshProUGUI textSensiDisplay;

    private void Start()
    {
        FMODMusicManager.instance.CreateMusicInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO);
        FMODMusicManager.instance.PlayMusicInstance(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));
        
        playButton.SetActive(false);
        parameterButton.SetActive(false);
        quitButton.SetActive(false);
        title.SetActive(false);
        parametrePanel.SetActive(false);
        BlackScreen.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            BlackScreen.SetActive(false);
            title.SetActive(true);
            playButton.SetActive(true);
            parameterButton.SetActive(true);
            quitButton.SetActive(true);
        }
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

    public void OnParameterButtonClicked()
    {
        parametrePanel.SetActive(true);
        playButton.SetActive(false);
        parameterButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void OnParameterPanelClose()
    {
        FMODEventManager.instance.PlayOneShot(FMODEventManager.instance.FMODEvents.Back, transform.position);
        parametrePanel.SetActive(false);
        playButton.SetActive(true);
        parameterButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void OnQuitButtonClicked()
    {
        
        FMODMusicManager.instance.StopMusic(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));
        FMODMusicManager.instance.ReleaseMusicInstance(FMODMusicManager.instance.GetMusicPlaylistInstance(FMODMusicManager.instance.FMODMusicEvents.INTRO));

        Application.Quit();
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
    public void SetSensitivity()
    {
        GameManager.Instance.parameters.sensitivity = mouseSensitivitySlider.value;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");
    }

    public void SetVolumeMaster()
    {
        GameManager.Instance.parameters.volumeMaster = volumeSliderMaster.value;
        FMODEventManager.instance.ChangeVolume(FMODEventManager.instance.Fmodbus.busMaster,GameManager.Instance.parameters.volumeMaster);
    }

    public void SetVolumePlayer()
    {
        GameManager.Instance.parameters.volumePlayer = volumeSliderPlayer.value;
        FMODEventManager.instance.ChangeVolume(FMODEventManager.instance.Fmodbus.busPlayer,GameManager.Instance.parameters.volumePlayer);
    }

    public void SetVolumeSystem()
    {
        GameManager.Instance.parameters.volumeSystem = volumeSliderSystem.value;
        FMODEventManager.instance.ChangeVolume(FMODEventManager.instance.Fmodbus.busSystem,GameManager.Instance.parameters.volumeSystem);
    }

    public void SetVolumeMusic()
    {
        GameManager.Instance.parameters.volumeMusic = volumeSliderMusic.value;
        FMODEventManager.instance.ChangeVolume(FMODEventManager.instance.Fmodbus.busMusic,GameManager.Instance.parameters.volumeMusic);
    }

    public void SetVolumeMenu()
    {
        GameManager.Instance.parameters.volumeMenu = volumeSliderMenu.value;
        FMODEventManager.instance.ChangeVolume(FMODEventManager.instance.Fmodbus.busMenu,GameManager.Instance.parameters.volumeMenu);
    }
    
    
}
