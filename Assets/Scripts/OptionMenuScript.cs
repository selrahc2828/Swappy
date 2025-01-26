using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionMenuScript : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject optionGroup;
    public Slider volumeSlider;
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI textSensiDisplay;

    // Start is called before the first frame update
    void Start()
    {
        optionGroup.SetActive(false);
        mouseSensitivitySlider.value = GameManager.sensitivity;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");
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
            
            Debug.Log($"Option Menu Opened : {isOpen}");
            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameManager.Instance.isPaused = true;
                Time.timeScale = 0f;
                Debug.Log("Option Menu Open");

            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameManager.Instance.isPaused = false;
                Time.timeScale = 1f;//verif avec le slowTime
                Debug.Log("Option Menu Closed");


            }
        }
    }
    public void SetSensitivity()
    {
        // GameManager.Instance.SetSensi(mouseSensitivitySlider.value);
        GameManager.sensitivity = mouseSensitivitySlider.value;
        textSensiDisplay.text = mouseSensitivitySlider.value.ToString("F2");
    }
}
