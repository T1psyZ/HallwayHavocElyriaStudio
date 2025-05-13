using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    public void InstanstiateLoot(Vector3 spawnPosition)
    {
        foreach (var item in lootList)
        {
                // Define the range for the random offset
                float range = 2.0f;

                // Calculate a random offset within the range
                Vector3 randomOffset = new Vector3(
                    Random.Range(-range, range), // Random X offset
                    0,                          // Keep Y the same (or adjust if needed)
                    Random.Range(-range, range) // Random Z offset
                );

                // Add the random offset to the enemy's position
                Vector3 randomPosition = spawnPosition + randomOffset;

                GameObject lootGameObject = Instantiate(droppedItemPrefab, randomPosition, Quaternion.identity);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = item.lootSprite;
                lootGameObject.GetComponent<Image>().sprite = item.lootSprite;
                lootGameObject.GetComponent<Item>().ID = item.itemID;

                Variables variables = lootGameObject.GetComponent<Variables>();
                if (variables != null)
                {
                    variables.declarations.Set("lootType", item.lootName);
                }
                else
                {
                    Debug.LogWarning("Variables component is missing on the lootGameObject.");
                }
        }

    }
}
