using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeController : MonoBehaviour
{
    [Header("References")]
    public GameObject skillTreeCanvas;        // The UI panel to show/hide
    public Button toggleSkillTreeButton;

    public GameObject joystickControl;
    
    // The button that triggers the menu

    void Start()
    {

        if (skillTreeCanvas == null)
        {
            Debug.LogError("Menu Canvas is not assigned in the inspector!");
            return;
        }

        skillTreeCanvas.SetActive(false);  // Hide the menu at the start

        if (toggleSkillTreeButton != null)
        {
            toggleSkillTreeButton.onClick.AddListener(ToggleSkillTree);
            Debug.Log("ToggleMenuButton listener added.");
        }
        else
        {
            Debug.LogError("Toggle Menu Button is not assigned in the inspector!");
        }
    }

    void ToggleSkillTree()
    {
        if (skillTreeCanvas != null)
        {
            bool newState = !skillTreeCanvas.activeSelf;
            skillTreeCanvas.SetActive(newState);
            joystickControl.SetActive(!newState); // Hide joystick if skill tree is shown, and show it when skill tree is hidden
            Debug.Log("Menu toggled: " + newState);

        }
    }
}
