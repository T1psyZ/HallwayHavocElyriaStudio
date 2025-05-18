using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUi : MonoBehaviour
{
    public GameObject inventoryUi;
    public GameObject crafttingUi;
    bool isOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrafting();
        }
    }
    public void ToggleCrafting()
    {
        if (!isOpen)
        {
            inventoryUi.SetActive(true);
            crafttingUi.SetActive(true);
            isOpen = true;
        }
        else
        {
            inventoryUi.SetActive(false);
            crafttingUi.SetActive(false);
            isOpen = false;
        }
    }
}