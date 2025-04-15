using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float moveSpeed;
    public float walkSpeed;
    public float runSpeed;

    public Rigidbody2D rgbd2d;
    public Animator animator;
    PlayerHealth playerHealth;
    bool isUsingStamina = false;
    bool isGainingStamina = false;
    public VirtualJoystick joystick;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {

        rgbd2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.IsPlayerDied())
        {
            movement = Vector2.zero;
            return;
        }
        movement = Vector2.zero;
        movement.x = joystick.HorizontalRaw();
        movement.y = joystick.VerticalRaw();

        if (Input.GetKey(KeyCode.LeftShift) && playerHealth.currentStamina > 0)
        {
            moveSpeed = runSpeed;
            if (!isUsingStamina)
            {
                StartCoroutine(UseStaminaWithCooldown());
            }
        }
        else
        {
            moveSpeed = walkSpeed;
        }
        if (movement != Vector2.zero)
        {

            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            //animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            //animator.SetBool("IsWalking", false);
        }   
    }

    private void FixedUpdate()
    {
        rgbd2d.MovePosition(rgbd2d.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (!isGainingStamina && moveSpeed != runSpeed)
        {
            StartCoroutine(GainStaminaWithCooldown());
        }

    }

    public bool IsMoving()
    {
        return movement != Vector2.zero;
    }

    private IEnumerator UseStaminaWithCooldown()
    {
        isUsingStamina = true;
        playerHealth.UseStamina();
        yield return new WaitForSeconds(0.5f); // Cooldown duration
        isUsingStamina = false;
    }

    private IEnumerator GainStaminaWithCooldown()
    {
        isGainingStamina = true;
        playerHealth.GainStamina();
        yield return new WaitForSeconds(1f); // Cooldown duration
        isGainingStamina = false;
    }
}
