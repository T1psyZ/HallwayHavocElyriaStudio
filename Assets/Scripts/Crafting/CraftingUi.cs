using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUi : MonoBehaviour
{
    public GameObject inventoryUi;
    bool isOpen = false;

    public void ToggleCrafting()
    {
        if (!isOpen)
        {
            inventoryUi.SetActive(true);
            gameObject.SetActive(true);
            isOpen = true;
        }
        else
        {
            inventoryUi.SetActive(false);
            gameObject.SetActive(false);
            isOpen = false;
        }
    }
}