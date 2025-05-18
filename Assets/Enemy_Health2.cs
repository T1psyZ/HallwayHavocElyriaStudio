using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Health2 : MonoBehaviour
{
    [Header("Loot")]
    public List<LootItems> lootTable = new List<LootItems>();
    public int ExpRewarded = 20;

    public delegate void MonsterDefeated(int exp);
    public event MonsterDefeated OnMonsterDefeated;
    public int currentHealth;
    public int maxHealth;

    EnemyHealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
          
            died();
        }
    }

    void died()
    {
        if (GetComponent<LootBag>() != null)
        {
            GetComponent<LootBag>().InstanstiateLoot(transform.position);
        }
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


}
