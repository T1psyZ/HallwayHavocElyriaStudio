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
    private Animator animator;
    public float deathAnimDuration = 1.0f; // Set this to your death animation's length

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        animator = GetComponentInChildren<Animator>();
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
            StartCoroutine(DieWithAnimation());
        }
    }

    IEnumerator DieWithAnimation()
    {
        if (GetComponent<LootBag>() != null)
        {
            GetComponent<LootBag>().InstanstiateLoot(transform.position);
        }

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
            yield return new WaitForSeconds(deathAnimDuration);
        }

        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}