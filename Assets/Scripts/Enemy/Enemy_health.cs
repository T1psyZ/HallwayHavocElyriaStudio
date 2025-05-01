using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    [Header("Loot")]
    public List<LootItems> lootTable = new List<LootItems>();
    public int ExpRewarded = 20;

    public delegate void MonsterDefeated(int exp);
    public event MonsterDefeated OnMonsterDefeated;
    public int currentHealth;
    public int maxHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            OnMonsterDefeated(ExpRewarded);
            died();
        }
    }

    void died()
    {
        GetComponent<LootBag>().InstanstiateLoot(transform.position);
        Destroy(gameObject);
    }


}
