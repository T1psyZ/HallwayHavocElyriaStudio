using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Knockbacl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy_Movement enemyMovement;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<Enemy_Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Knockback(Transform playerTransform, float knockbackForce)
    {
        enemyMovement.ChangeState(EnemyState.Knockback);
        spriteRenderer.color = Color.red;
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;
        spriteRenderer.color = Color.white;
        enemyMovement.ChangeState(EnemyState.Chasing);
    }
}
