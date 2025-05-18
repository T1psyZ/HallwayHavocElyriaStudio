using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerWeaponHandler : MonoBehaviour
{
    public Transform attackpoint;
    public LayerMask enemyLayers;
    public int PickupRange;
    public Variables attackType;

    private bool isDashing = false;
    private PlayerController playerMovement;
    private float timer;

    public GameObject attackBtn;
    public GameObject pickupBtn;
    public GameObject bullet;
    public GameObject enemy;
    private bool isThrowing = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerController>();
        attackBtn.SetActive(true);
        pickupBtn.SetActive(true);
    }

    private void Update()
    {
        WeaponAttackUpdate();

        // Optional for keyboard-based pickup (for testing in editor)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void WeaponAttackUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isDashing)
        {
            ThrowableAttack();
            StartCoroutine(Dash());
            playerMovement.animator.SetBool("IsAttacking", true);
        }
    }

    public void OnAttackButtonPressed()
    {
        if (!isDashing)
        {
            StartCoroutine(Dash());
            playerMovement.animator.SetBool("IsAttacking", true);
        }
    }

    public void OnPickupButtonPressed()
    {
        TryPickup();
    }

    private void TryPickup()
    {
        var sticksOnGround = GameObject.FindGameObjectsWithTag("StickWeapon");

        foreach (GameObject stick in sticksOnGround)
        {
            float distance = Vector3.Distance(stick.transform.position, transform.position);
            if (distance <= PickupRange)
            {
                playerMovement.weaponEquipped = "StickWeapon";
                Destroy(stick);
                break; // Exit loop after picking one up
            }
        }
    }

    public void dealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackpoint.position,
            Stats_Manager.instance.weaponRange,
            enemyLayers);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<Enemy_health>().ChangeHealth(-Stats_Manager.instance.attackDamage);
            enemies[0].GetComponent<Player_Knockbacl>().Knockback(transform, Stats_Manager.instance.knockbackForce);
        }

        timer = Stats_Manager.instance.cooldown;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        playerMovement.enabled = false;
        float elapsedTime = 0f;

        Vector2 dashDirection = playerMovement.movement.normalized;
        while (elapsedTime < Stats_Manager.instance.dashDuration)
        {
            playerMovement.rgbd2d.MovePosition(
                playerMovement.rgbd2d.position + dashDirection * Stats_Manager.instance.dashSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isDashing = false;
        playerMovement.enabled = true;
    }

    public void ThrowableAttack()
    {
        if (!Stats_Manager.instance.canThrow || isThrowing)
        {
            return;
        }
        // Instantiate bullet at attackpoint
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);

        // Calculate direction
        Vector2 direction = (enemy.transform.position - transform.position).normalized;

        // Rotate bullet to face direction (assuming bullet faces right)
        b.transform.right = direction;

        // Set velocity
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        float bulletSpeed = 10f; // Set your desired speed
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
        StartCoroutine(ThrowingCooldown());
    }

    IEnumerator ThrowingCooldown()
    {
        isThrowing = true;
        yield return new WaitForSeconds(Stats_Manager.instance.cooldown);
        isThrowing = false;
    }
}
