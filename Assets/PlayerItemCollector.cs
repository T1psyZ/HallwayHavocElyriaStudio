using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerItemCollector : MonoBehaviour
{
    SaveController saveController;
    private InventoryController inventoryController;
    int itemCount = 0;
    public string sceneName;
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        saveController = FindObjectOfType<SaveController>();
    }

    void Update()
    {
        //var lootsOnGround = GameObject.FindGameObjectsWithTag("Item");
        //foreach (GameObject loot in lootsOnGround)
        //{
        //    float distance = Vector3.Distance(loot.transform.position, transform.position);
        //    if (distance < 2)
        //    {
        //        Item item = loot.GetComponent<Item>();
        //        if (item != null) { }
        //        {
        //            bool itemAdded = inventoryController.AddItem(loot.gameObject);

        //            if (itemAdded)
        //            {
        //                Destroy(loot.gameObject);
        //                itemCount++;
        //                break;
        //            }
        //        }
        //    }
        //}

    }
    //// Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null) { }
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                    itemCount++;
                }
            }

            if (itemCount == 3)
            {
                saveController.SaveGame();
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
