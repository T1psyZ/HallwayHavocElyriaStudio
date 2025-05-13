using UnityEngine;

public class TrashCanInteraction : MonoBehaviour
{
    public string trashCanType;
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
