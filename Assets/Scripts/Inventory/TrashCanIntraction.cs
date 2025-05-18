using UnityEngine;
using UnityEngine.UI;

public class TrashCanInteraction : MonoBehaviour
{
    public string trashCanType;
    public GameObject trashCanUI;
    public GameObject inventoryUI;
    public GameObject interactButton;
    public GameObject player;
    public GameObject menuButton;
    public GameObject joystickControl;
    public Button closeButton;
    private bool playerInRange = false;
    TrashcanController trashcanController;

    private void Start()
    {
        trashcanController = FindObjectOfType<TrashcanController>();
        closeButton.onClick.AddListener(CloseTrashCan);
    }
    void Update()
    {
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
            trashCanUI.SetActive(true);
            inventoryUI.SetActive(true);
            interactButton.SetActive(false); // Hide the interact button
            joystickControl.SetActive(false); // Disable joystick
            menuButton.SetActive(false);      // Disable menu button
        }
    }
    private void CloseTrashCan()
    {
        if (trashCanUI != null)
        {
            inventoryUI.SetActive(false);
            trashCanUI.SetActive(false);
            interactButton.SetActive(true); // Show the interact button again
            joystickControl.SetActive(true); // Enable joystick
            menuButton.SetActive(true);      // Enable menu button
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
            joystickControl.SetActive(true);
            menuButton.SetActive(true);

            // Optional: auto-close UIs
            if (trashCanUI != null) trashCanUI.SetActive(false);
            if (inventoryUI != null) inventoryUI.SetActive(false);
        }
    }
}
