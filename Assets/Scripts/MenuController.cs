using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    public GameObject menuCanvas;        // The UI panel to show/hide
    public Button toggleMenuButton;      // The button that triggers the menu
    public GameObject joystickControl;   // Joystick to hide/show

    void Awake()
    {
        Debug.Log("MenuController Awake called.");
    }

    void OnEnable()
    {
        Debug.Log("MenuController enabled.");
    }

    void OnDisable()
    {
        Debug.Log("MenuController disabled.");
    }

    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);
        Debug.Log("MenuController Start called.");

        if (menuCanvas == null)
        {
            Debug.LogError("Menu Canvas is not assigned in the inspector!");
            return;
        }

        menuCanvas.SetActive(false);  // Hide the menu at the start
        Debug.Log("Menu canvas set to inactive at start.");

        if (toggleMenuButton != null)
        {
            toggleMenuButton.onClick.AddListener(ToggleMenu);
            Debug.Log("ToggleMenuButton listener added.");
        }
        else
        {
            Debug.LogError("Toggle Menu Button is not assigned in the inspector!");
        }

        if (joystickControl == null)
        {
            Debug.LogWarning("Joystick Control is not assigned in the inspector!");
        }
    }

    private void Update()
    {
        // Check for mobile input to toggle menu
        if (Input.GetKeyDown(KeyCode.Tab)) // Replace with your mobile input check
        {
            ToggleMenu();
            Debug.Log("Menu toggled via mobile input.");
        }
    }
    void ToggleMenu()
    {
        Debug.Log("ToggleMenu called.");

        if (menuCanvas != null)
        {
            bool newState = !menuCanvas.activeSelf;
            menuCanvas.SetActive(newState);
            Debug.Log("Menu canvas active state set to: " + newState);

            if (joystickControl != null)
            {
                joystickControl.SetActive(!newState); // Hide when menu is shown, show when menu is hidden
                Debug.Log("JoystickControl active state set to: " + (!newState));
            }
            else
            {
                Debug.LogWarning("Joystick Control is null when toggling menu.");
            }

            Debug.Log("Menu toggled: " + newState);
        }
        else
        {
            Debug.LogError("Menu Canvas is null when trying to toggle menu.");
        }
    }
}
