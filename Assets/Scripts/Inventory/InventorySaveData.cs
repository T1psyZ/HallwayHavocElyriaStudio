using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable] 
public class InventorySaveData
{
    public int itemID;
    public string lootType; // The index of the slot  in the inventory 
    public int slotIndex;
}
