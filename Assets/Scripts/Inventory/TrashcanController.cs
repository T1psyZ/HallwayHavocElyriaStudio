using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashcanController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    [Header("UI References")]
    public GameObject trashCanUI;             // The whole TrashCan UI container
    public GameObject trashCanPanel;          // The panel that holds the slots
    public GameObject trashCanSlotPrefab;     // Prefab for each inventory slot
    public Button closeButton;                // Button to close the trash UI
    public GameObject inventoryUI;            // Reference to the Inventory panel (assign in Inspector)
    public GameObject interactButton;
    public GameObject menuButton;
    public GameObject joystickControl;
    [Header("Slot Settings")]
    public int slotCount = 1;                 // Number of slots in the trashcan
    [Header("Public Checks")]
    public bool inTrashcan = false; // Set to true when the player is in the trashcan UI

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
            inTrashcan = false;
            interactButton.SetActive(true); // Show the interact button again
            joystickControl.SetActive(true); // Enable joystick
            menuButton.SetActive(true);      // Enable menu button
            trashCanUI.SetActive(false); // Deactivate the whole TrashCan UI
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

        foreach (Transform slotTransform in trashCanPanel.transform)
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

        return invData;
    }

    public void SetTrashcanItems(List<InventorySaveData> inventorySaveData)
    {
        for (int i = 0; i < slotCount; i++)
        {
            trashCanSlotPrefab.gameObject.name = "TrashCanSlot";
            Instantiate(trashCanSlotPrefab, trashCanPanel.transform);
        }

        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                InventorySlot slot = trashCanPanel.transform.GetChild(data.slotIndex).GetComponent<InventorySlot>();
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
