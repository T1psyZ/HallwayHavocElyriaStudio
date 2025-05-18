using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedDoor : MonoBehaviour
{
    public List<GameObject> doors;
    public GameObject interactButton;
    public GameObject mutatedUI;
    public GameObject inventoryUI;
    public InventorySlot inventorySlot;
    bool canInteract = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (inventorySlot != null)
        {
            if (inventorySlot.currentItem != null)
            {                
                Item item = inventorySlot.currentItem.GetComponent<Item>();
                if (item.Name == "ForkScratcher")
                {
                    foreach (var door in doors)
                    {
                        Destroy(door);
                    }
                    Destroy(mutatedUI);
                    inventoryUI.SetActive(false);
                }
            }
        }
    }

    public void Interact()
    {
        if (canInteract)
        {
            mutatedUI.SetActive(true);
            inventoryUI.SetActive(true);
        }
    }

    public void Close()
    {
        mutatedUI.SetActive(false);
        inventoryUI.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactButton.SetActive(true);
            canInteract = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactButton.SetActive(false);
            canInteract = false;
        }
    }
}
