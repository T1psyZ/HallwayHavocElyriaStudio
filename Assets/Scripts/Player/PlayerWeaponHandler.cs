using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public Transform attackpoint;
    public LayerMask enemyLayers;
    public int PickupRange;
    public Variables attackType;

    private bool isDashing = false;
    private PlayerController playerMovement;
    private float timer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerController>();
    }

    private void Update()
    {
        WeaponAttackUpdate();
        PickUpUpdate();
    }

    void WeaponAttackUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isDashing)
        {
            StartCoroutine(Dash());
            playerMovement.animator.SetBool("IsAttacking", true);
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

    void PickUpUpdate()
    {
        var sticksOnGround = GameObject.FindGameObjectsWithTag("StickWeapon");

        foreach (GameObject stick in sticksOnGround)
        {
            float distance = Vector3.Distance(stick.transform.position, transform.position);
            if (distance <= PickupRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerMovement.weaponEquipped = "StickWeapon";
                    Destroy(stick);
                }
            }
        }
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
}
