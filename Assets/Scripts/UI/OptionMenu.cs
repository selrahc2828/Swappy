using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI textSensiDisplay;

    public Slider volumeSliderMaster;
    public Slider volumeSliderPlayer;
    public Slider volumeSliderSystem;
    public Slider volumeSliderMusic;
    public Slider volumeSliderMenu;

    void Start()
    {
        mouseSensitivitySlider.value = GameManager.Instance.parameters.sensitivity;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");

        volumeSliderMaster.value = GameManager.Instance.parameters.volumeMaster;
        volumeSliderPlayer.value = GameManager.Instance.parameters.volumePlayer;
        volumeSliderSystem.value = GameManager.Instance.parameters.volumeSystem;
        volumeSliderMusic.value = GameManager.Instance.parameters.volumeMusic;
        volumeSliderMenu.value = GameManager.Instance.parameters.volumeMenu;
    }

    #region SetVolumeS
    public void SetSensitivity()
    {
        GameManager.Instance.parameters.sensitivity = mouseSensitivitySlider.value;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");
    }

    public void SetVolumeMaster()
    {
        GameManager.Instance.parameters.volumeMaster = volumeSliderMaster.value;
    }

    public void SetVolumePlayer()
    {
        GameManager.Instance.parameters.volumePlayer = volumeSliderPlayer.value;
    }

    public void SetVolumeSystem()
    {
        GameManager.Instance.parameters.volumeSystem = volumeSliderSystem.value;
    }

    public void SetVolumeMusic()
    {
        GameManager.Instance.parameters.volumeMusic = volumeSliderMusic.value;
    }

    public void SetVolumeMenu()
    {
        GameManager.Instance.parameters.volumeMenu = volumeSliderMenu.value;
    }
    #endregion
    
}
