using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Assuming the player has a PlayerHealth script to handle damage
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Vector2 knockDirection = (collision.transform.position - transform.position).normalized;
                playerHealth.ChangeHealth(-2, knockDirection * 10f);
            }
            Destroy(gameObject); // Destroy the bullet after hitting the player
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the bullet when it hits a wall
        }
    }
}
