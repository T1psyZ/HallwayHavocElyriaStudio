using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    public GameObject menuCanvas;        // The UI panel to show/hide
    public Button toggleMenuButton;      // The button that triggers the menu

    void Start()
    {
        if (menuCanvas == null)
        {
            Debug.LogError("Menu Canvas is not assigned in the inspector!");
            return;
        }

        menuCanvas.SetActive(false);  // Hide the menu at the start

        if (toggleMenuButton != null)
        {
            toggleMenuButton.onClick.AddListener(ToggleMenu);
            Debug.Log("ToggleMenuButton listener added.");
        }
        else
        {
            Debug.LogError("Toggle Menu Button is not assigned in the inspector!");
        }
    }

    void ToggleMenu()
    {
        if (menuCanvas != null)
        {
            bool newState = !menuCanvas.activeSelf;
            menuCanvas.SetActive(newState);
            Debug.Log("Menu toggled: " + newState);
        }
    }
}
