using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject menuGroup;
    
    public GameObject firstButton;
    
    [Header("Inventaire")]
    public GameObject inventoryGroup;
    [Header("Cassettes")]
    public GameObject tapeGroup;
    [Header("Map")]
    public GameObject mapGroup;
    [Header("Options")]
    public GameObject optionGroup;
    
    [Header("Menus")]
    public InventoryMenu inventoryMenu;
    public TapeMenu tapeMenu;
    public OptionMenu mapMenu;
    public OptionMenu optionMenu;
    
    void Start()
    {
        menuGroup.SetActive(false);
    }

    void OnEnable()
    {
        GameManager.controls.Player.PauseMenu.performed += PauseMenu;
        GameManager.controls.Pause.Resume.performed += ResumeAction;
        
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);

        // Lancer le OnClick manuellement si tu veux
        Button btn = firstButton?.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.Invoke();
        }
    }
    
    void OnDisable()
    {
        GameManager.controls.Player.PauseMenu.performed -= PauseMenu;
        GameManager.controls.Pause.Resume.performed -= ResumeAction;
    }

    public void PauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //changer quand on merge avec le multi scene
            GameManager.controls.Pause.Enable();
            GameManager.controls.Player.Disable();
            isOpen = true;
            GameManager.Instance.isPaused = true;
            
            menuGroup.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.Instance.isPaused = true;
            Time.timeScale = 0f;
        }
    }

    public void ResumeAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Resume();
        }
    }

    public void Resume()
    {
        //changer quand on merge avec le multi scene

        GameManager.controls.Pause.Disable();
        GameManager.controls.Player.Enable();
        GameManager.Instance.isPaused = false;
        isOpen = false;
            
        menuGroup.SetActive(isOpen);
            
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.isPaused = false;
        Time.timeScale = 1f;//verif avec le slowTime
    }
    
    public void OpenInventory()
    {
        inventoryGroup.SetActive(true);
        tapeGroup.SetActive(false);
        mapGroup.SetActive(false);
        optionGroup.SetActive(false);
    }

    public void OpenOptions()
    {
        inventoryGroup.SetActive(false);
        tapeGroup.SetActive(false);
        mapGroup.SetActive(false);
        optionGroup.SetActive(true);
    }

    public void OpenTape()
    {
        inventoryGroup.SetActive(false);
        tapeGroup.SetActive(true);
        mapGroup.SetActive(false);
        optionGroup.SetActive(false);
        
        tapeMenu.SetTapeButtons();
        tapeMenu.CenterMiddleButtonIfNoneSelected();
        tapeMenu.AdjustScrollPadding();
    }
    public void OpenMap()
    {
        inventoryGroup.SetActive(false);
        tapeGroup.SetActive(false);
        mapGroup.SetActive(true);
        optionGroup.SetActive(false);
    }
    
}
