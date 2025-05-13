using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{

    public float speed;
    public float pushForce = 2f;
    private Rigidbody2D rb; 
    private int facingDirection = -1;
    private Animator anim;
    public EnemyState enemyState, newState;
    private PlayerHealth playerHealth;
    GameObject player;

    public float attackRange = 5;
    public float chaseRange = 10;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        ChangeState(EnemyState.Idle);
    }


    // Update is called once per frame
    void Update()
    {
        var dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist > chaseRange)
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
        else
        {
            if (!isAttacking)
                ChangeState(EnemyState.Chasing);
            PlayerController playerMovement = player.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                if (playerMovement.IsMoving())
                {
                    // Stop both the player and the enemy
                    rb.velocity = Vector2.zero;
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else
                {
                    // Push the player
                    Vector2 pushDirection = (player.transform.position - transform.position).normalized;
                    player.GetComponent<Rigidbody2D>().AddForce(pushDirection * pushForce, ForceMode2D.Force);
                }
            }


        }
        if (enemyState != EnemyState.Knockback)
        {
            isAttacking = false;
            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        PlayerController playerMovement = collision.gameObject.GetComponent<PlayerController>();

    //        if (playerMovement != null)
    //        {
    //            if (playerMovement.IsMoving())
    //            {
    //                // Stop both the player and the enemy
    //                rb.velocity = Vector2.zero;
    //                collision.rigidbody.velocity = Vector2.zero;
    //            }
    //            else
    //            {
    //                // Push the player
    //                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
    //                collision.rigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Force);
    //            }
    //        }
    //    }
    //}

    void Chase()
    {
        if (playerHealth.IsPlayerDied())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            isAttacking = true;
            ChangeState(EnemyState.Attacking);
            
        }
        else if (player.transform.position.x > transform.position.x && facingDirection == 1 ||
               player.transform.position.x < transform.position.x && facingDirection == -1)
        {
            Flip();
        }
        else
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }

    }
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void ChangeState(EnemyState newState)
    {
        enemyState = newState;
        anim.SetInteger("AnimMode", enemyState == EnemyState.Attacking ? 1 : enemyState == EnemyState.Chasing? 2: 0);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
}
