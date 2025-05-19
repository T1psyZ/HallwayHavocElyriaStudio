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
                    item.Pickup();
                    Destroy(collision.gameObject);
                    itemCount++;
                }
            }

            if (itemCount == 3 && SceneManager.GetActiveScene().name == "InsideGymScene")
            {
                saveController.SaveGame();
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
