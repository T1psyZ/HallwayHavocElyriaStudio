using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public InventorySlot[] craftingSlots;
    public InventorySlot resultSlot;
    public List<CraftingRecipe> recipes;
    SaveController saveController;

    private void Start()
    {
        saveController = FindObjectOfType<SaveController>();
    }


    public void Craft()
    {
        List<string> currentItemNames = new List<string>();
        currentItemNames.Clear();
        foreach (InventorySlot slot in craftingSlots)
        {
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                Debug.Log(item.Name);
                currentItemNames.Add(item != null ? item.Name : "");
            }
            else
            {
                currentItemNames.Add("");
            }
        }

        foreach (CraftingRecipe recipe in recipes)
        {
            if (recipe.Matches(currentItemNames))
            {
                foreach (InventorySlot slot in craftingSlots)
                {
                    if (slot.currentItem != null)
                    {
                        Destroy(slot.currentItem);
                        slot.currentItem = null;
                    }
                }
                GameObject resultItem = Instantiate(recipe.resultPrefab, resultSlot.transform);
                resultSlot.currentItem = resultItem;
                if (recipe.resultPrefab.gameObject.name == "CraftedLoot2")
                {
                    Stats_Manager.instance.canThrow = true;
                }

                return;
            }
        }

        // No match found
        resultSlot.currentItem = null;
    }



}
