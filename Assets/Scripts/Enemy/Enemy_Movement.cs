using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{

    public float speed;
    public float pushForce = 2f;
    private Rigidbody2D rb;
    private Transform Player;
    private int facingDirection = -1;
    private Animator anim;
    private EnemyState enemyState, newState;
    private PlayerHealth playerHealth;

    public float attackRange = 1;
    bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }


    // Update is called once per frame
    void Update()
    {
        isAttacking = false;
        if (enemyState == EnemyState.Chasing)
        {
            Chase();
        }
        else if (enemyState == EnemyState.Attacking)
        {
            //rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerMovement = collision.gameObject.GetComponent<PlayerController>();

            if (playerMovement != null)
            {
                if (playerMovement.IsMoving())
                {
                    // Stop both the player and the enemy
                    rb.velocity = Vector2.zero;
                    collision.rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    // Push the player
                    Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                    collision.rigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Force);
                }
            }
        }
    }

    void Chase()
    {
        if (playerHealth.IsPlayerDied())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(transform.position, Player.transform.position) <= attackRange)
        {
            isAttacking = true;
            ChangeState(EnemyState.Attacking);
            
        }
        else if (Player.position.x > transform.position.x && facingDirection == 1 ||
               Player.position.x < transform.position.x && facingDirection == -1)
        {
            Flip();
        }
        else
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }

    }
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerStay2D(Collider2D collision) //OntriggerENTER
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Player == null)
            {
                Player = collision.transform;
                playerHealth = collision.GetComponent<PlayerHealth>();
            }
            if (!isAttacking) ChangeState(EnemyState.Chasing);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //OntriggerEXIT 
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }
    void ChangeState(EnemyState newState)
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
}
