using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanInteraction : MonoBehaviour
{
    public GameObject trashCanUI;
    public GameObject inventoryUI;
    public GameObject player;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (trashCanUI != null && inventoryUI != null)
            {
                trashCanUI.SetActive(true);
                inventoryUI.SetActive(true);
            }
            else
            {
                Debug.LogWarning("UI references not set on TrashCanInteraction.");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Optional: auto-close when player leaves
            if (trashCanUI != null) trashCanUI.SetActive(false);
            if (inventoryUI != null) inventoryUI.SetActive(false);
        }
    }
}
