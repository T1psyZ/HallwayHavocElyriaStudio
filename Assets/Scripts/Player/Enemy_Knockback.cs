using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Knockbacl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy_Movement enemyMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<Enemy_Movement>();
    }
    public void Knockback(Transform playerTransform, float knockbackForce)
    {
        enemyMovement.ChangeState(EnemyState.Knockback);
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;
        Debug.Log("Knockback applied: " + direction * knockbackForce);  
    }
}
