using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrashcanController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    [Header("UI References")]
    public List<GameObject> trashCanUI;             // The whole TrashCan UI container
    public List<GameObject> trashCanPanel;          // The panel that holds the slots
    public List<GameObject> trashCanSlotPrefab;     // Prefab for each inventory slot
    public Button closeButton;                // Button to close the trash UI
    public GameObject inventoryUI;            // Reference to the Inventory panel (assign in Inspector)
    public GameObject interactButton;
    public GameObject menuButton;
    public GameObject joystickControl;

    [Header("Slot Settings")]
    public int slotCount = 1;                 // Number of slots in the trashcan

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseTrashcan);
        }
        else
        {
            Debug.LogWarning("Close button not assigned on TrashcanController.");
        }
    }

    private void OnEnable()
    {
        Debug.Log("TrashcanController active on: " + gameObject.name);
    }

    public void CloseTrashcan()
    {
        if (trashCanUI != null)
        {
            interactButton.SetActive(true); // Show the interact button again
            joystickControl.SetActive(true); // Enable joystick
            menuButton.SetActive(true);      // Enable menu button
            foreach (var ui in trashCanUI)
            {
                ui.SetActive(false); // Deactivate the whole TrashCan UI
            }
        }
        else
        {
            Debug.LogWarning("TrashCanUI is not assigned.");
        }

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false); // Also deactivate the Inventory UI
        }
        else
        {
            Debug.LogWarning("InventoryUI is not assigned.");
        }
    }

    public List<InventorySaveData> GetTrashcanItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (var panel in trashCanPanel)
        {
            foreach (Transform slotTransform in panel.transform)
            {
                InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
                if (slot.currentItem != null)
                {
                    Item item = slot.currentItem.GetComponent<Item>();
                    invData.Add(new InventorySaveData
                    {
                        itemID = item.ID,
                        slotIndex = slotTransform.GetSiblingIndex()
                    });
                }
            }            
        }

        return invData;
    }

    public void SetTrashcanItems(List<InventorySaveData> inventorySaveData)
    {
        for (int i = 0; i < trashCanPanel.Count(); i++)
        {
            for (int j = 0; j < slotCount; j++)
            {
                Instantiate(trashCanSlotPrefab[i], trashCanPanel[i].transform);
            }
        }


        foreach (InventorySaveData data in inventorySaveData)
        {
            foreach (var panel in trashCanPanel)
            {
                if (data.slotIndex < slotCount)
                {
                    InventorySlot slot = panel.transform.GetChild(data.slotIndex).GetComponent<InventorySlot>();
                    GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                    if (itemPrefab != null)
                    {
                        GameObject item = Instantiate(itemPrefab, slot.transform);
                        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        slot.currentItem = item;
                    }
                }
            }

        }
    }
}
