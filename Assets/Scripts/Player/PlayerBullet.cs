using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Assuming the enemy has an EnemyHealth script to handle damage
            Enemy_health enemyHealth = collision.GetComponent<Enemy_health>();
            if (enemyHealth != null)
            {
                Vector2 knockDirection = (collision.transform.position - transform.position).normalized;
                enemyHealth.ChangeHealth(-1); // Change this value as needed
              
            }
            Enemy_Health2 enemyHealth2 = collision.GetComponent<Enemy_Health2>();
            if (enemyHealth2 != null)
            {
                Vector2 knockDirection = (collision.transform.position - transform.position).normalized;
                enemyHealth2.ChangeHealth(-1); // Change this value as needed
            };
            Destroy(gameObject); // Destroy the bullet after hitting the player
        }
    }
}
