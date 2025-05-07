using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    void Update()
    {
        var lootsOnGround = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject loot in lootsOnGround)
        {
            float distance = Vector3.Distance(loot.transform.position, transform.position);
            if (distance <= 1)
            {
                Item item = loot.GetComponent<Item>();
                if (item != null) { }
                {
                    bool itemAdded = inventoryController.AddItem(loot.gameObject);

                    if (itemAdded)
                    {
                        Destroy(loot.gameObject);
                    }
                }
                break; // Exit loop after picking one up
            }
        }
    }
    //// Update is called once per frame
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Item"))
    //    {
    //        Item item = collision.GetComponent<Item>();
    //        if (item != null) { }
    //        {
    //            bool itemAdded = inventoryController.AddItem(collision.gameObject);

    //            if (itemAdded)
    //            {
    //                Destroy(collision.gameObject);
    //            }
    //        }
    //    }
    //}
}
