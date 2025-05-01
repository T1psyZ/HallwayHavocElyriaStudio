using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;

    public Rigidbody2D rgbd2d;
    public Animator animator;
    PlayerHealth playerHealth;

    bool isUsingStamina = false;
    bool isGainingStamina = false;

    public VirtualJoystick joystick;

    public Button sprintButton;
    public Button interactButton;

    private bool isSprintPressed = false;

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public string weaponEquipped;

    void Start()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        moveSpeed = Stats_Manager.instance.walkSpeed;

        if (sprintButton != null)
        {
            sprintButton.onClick.AddListener(ToggleSprint);
        }

        if (interactButton != null)
        {
            interactButton.onClick.AddListener(Interact);
        }
    }

    void Update()
    {
        if (playerHealth.IsPlayerDied())
        {
            movement = Vector2.zero;
            return;
        }

        movement = new Vector2(joystick.HorizontalRaw(), joystick.VerticalRaw());

        if (weaponEquipped == "StickWeapon")
        {
            animator.SetBool("HadAStick", true);
        }

        if (isSprintPressed && Stats_Manager.instance.currentStamina > 0)
        {
            moveSpeed = Stats_Manager.instance.runSpeed;
            animator.SetBool("isRunning", true);

            if (!isUsingStamina)
            {
                StartCoroutine(UseStaminaWithCooldown());
            }
        }
        else
        {
            moveSpeed = Stats_Manager.instance.walkSpeed;
            animator.SetBool("isRunning", false);
        }

        if (movement != Vector2.zero)
        {
            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void FixedUpdate()
    {
        rgbd2d.MovePosition(rgbd2d.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (!isGainingStamina && moveSpeed != Stats_Manager.instance.runSpeed)
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
        yield return new WaitForSeconds(0.5f);
        isUsingStamina = false;
    }

    private IEnumerator GainStaminaWithCooldown()
    {
        isGainingStamina = true;
        playerHealth.GainStamina();
        yield return new WaitForSeconds(1f);
        isGainingStamina = false;
    }

    public void StopAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

    public void ToggleSprint()
    {
        isSprintPressed = !isSprintPressed;
    }

    public void Interact()
    {
        Debug.Log("Interact button pressed");
        // Add interaction logic here
    }
}
