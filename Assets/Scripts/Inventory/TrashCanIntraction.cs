using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanInteraction : MonoBehaviour
{
    public GameObject trashCanUI;
    public GameObject inventoryUI;
    public GameObject interactButton;
    public GameObject player;
    public GameObject menuButton;
    public GameObject joystickControl;

    private bool playerInRange = false;
    TrashcanController trashcanController;

    private void Start()
    {
        trashcanController = FindObjectOfType<TrashcanController>();
    }
    void Update()
    {
        // Optional: keep this for desktop testing
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenTrashCan();
        }
    }

    public void OnMobileInteractPressed()
    {
        if (playerInRange)
        {
            OpenTrashCan();
        }
    }

    private void OpenTrashCan()
    {
        if (trashCanUI != null && inventoryUI != null)
        {
            trashcanController.inTrashcan = true;
            trashCanUI.SetActive(true);
            inventoryUI.SetActive(true);
            interactButton.SetActive(false); // Hide the interact button
            joystickControl.SetActive(false); // Disable joystick
            menuButton.SetActive(false);      // Disable menu button
        }
        else
        {
            Debug.LogWarning("UI references not set on TrashCanInteraction.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactButton.SetActive(true);
            joystickControl.SetActive(true); // Disable joystick
            menuButton.SetActive(true);

            // Optional: auto-close UIs
            if (trashCanUI != null) trashCanUI.SetActive(false);
            if (inventoryUI != null) inventoryUI.SetActive(false);
        }
    }
}
