using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionMenuScript : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject optionGroup;
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI textSensiDisplay;
    [Header("Volume")]
    public Slider volumeSliderMaster;
    public Slider volumeSliderPlayer;
    public Slider volumeSliderSystem;
    public Slider volumeSliderMusic;
    public Slider volumeSliderMenu;

    // Start is called before the first frame update
    void Start()
    {
        optionGroup.SetActive(false);
        mouseSensitivitySlider.value = GameManager.Instance.parameters.sensitivity;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");

        volumeSliderMaster.value = GameManager.Instance.parameters.volumeMaster;
        volumeSliderPlayer.value = GameManager.Instance.parameters.volumePlayer; // que player
        volumeSliderSystem.value = GameManager.Instance.parameters.volumeSystem; // que pas du player
        volumeSliderMusic.value = GameManager.Instance.parameters.volumeMusic;   // ambiance 
        volumeSliderMenu.value = GameManager.Instance.parameters.volumeMenu;
    }

    void OnEnable()
    {
        GameManager.controls.Player.PauseMenu.performed += PauseMenu;
    }
    
    void OnDisable()
    {
        GameManager.controls.Player.PauseMenu.performed -= PauseMenu;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isOpen = !isOpen;
            optionGroup.SetActive(isOpen);
            
            if (isOpen) // => pause
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameManager.Instance.isPaused = true;
                Time.timeScale = 0f;
                //ATTENTION - enregistre les input et les faits quand on resume
            }
            else // => resume
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameManager.Instance.isPaused = false;
                Time.timeScale = 1f;//verif avec le slowTime
            }
        }
    }
    public void SetSensitivity()
    {
        GameManager.Instance.parameters.sensitivity = mouseSensitivitySlider.value;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");
    }

    #region SetVolumeS
    public void SetVolumeMaster()
    {
        GameManager.Instance.parameters.volumeMaster = volumeSliderMaster.value;
        SoundManager.Instance?.SetBusVolume(SoundManager.BusSound.Master, volumeSliderMaster.value);
    }

    public void SetVolumePlayer()
    {
        GameManager.Instance.parameters.volumePlayer = volumeSliderPlayer.value;
        SoundManager.Instance?.SetBusVolume(SoundManager.BusSound.Player, volumeSliderPlayer.value);
    }

    public void SetVolumeSystem()
    {
        GameManager.Instance.parameters.volumeSystem = volumeSliderSystem.value;
        SoundManager.Instance?.SetBusVolume(SoundManager.BusSound.System, volumeSliderSystem.value);
    }

    public void SetVolumeMusic()
    {
        GameManager.Instance.parameters.volumeMusic = volumeSliderMusic.value;
        SoundManager.Instance?.SetBusVolume(SoundManager.BusSound.Music, volumeSliderMusic.value);
    }

    public void SetVolumeMenu()
    {
        GameManager.Instance.parameters.volumeMenu = volumeSliderMenu.value;
        SoundManager.Instance?.SetBusVolume(SoundManager.BusSound.Menu, volumeSliderMenu.value);
    }
    
    #endregion


}
