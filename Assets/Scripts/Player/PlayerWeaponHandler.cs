using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerWeaponHandler : MonoBehaviour
{
    private PlayerController playerMovement;
    public List<GameObject> micWeapons = new List<GameObject>();
    public int PickupRange;
    private string currentWeaponType;
    private GameObject currentWeaponEquipped = null;
    public Variables attackType;
    Animator weaponAnimator;
    Vector2 oldMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerController>();
    }

    private void Update()
    {
        WeaponAttack();
        PickUpUpdate();
        WeaponFacing();
    }

    void WeaponAttack()
    {
        
        if (Input.GetKeyDown(KeyCode.R) && currentWeaponEquipped != null)
        {
            string attackTypeValue = attackType.declarations.Get<string>("AttackType");
            weaponAnimator = currentWeaponEquipped.GetComponent<Animator>();

            if (string.IsNullOrEmpty(attackTypeValue))
            {
                return;
            }

            if (attackTypeValue == "Up")
            {
                weaponAnimator.SetFloat("AttackType", 0.1f);
            }
            else if (attackTypeValue == "Down")
            {
                weaponAnimator.SetFloat("AttackType", 0.2f);
            }
            else if (attackTypeValue == "Left")
            {
                weaponAnimator.SetFloat("AttackType", 0.3f);
            }
            else if (attackTypeValue == "Right")
            {
                weaponAnimator.SetFloat("AttackType", 0.4f);
            }
            else if (attackTypeValue == "UpLeft")
            {
                weaponAnimator.SetFloat("AttackType", 0.5f);
            }
            else if (attackTypeValue == "UpRight")
            {
                weaponAnimator.SetFloat("AttackType", 0.6f);
            }
            else if (attackTypeValue == "DownLeft")
            {
                weaponAnimator.SetFloat("AttackType", 0.7f);
            }
            else if (attackTypeValue == "DownRight")
            {
                weaponAnimator.SetFloat("AttackType", 0.8f);
            }
            
            StartCoroutine(ResetAttack());
        }
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        weaponAnimator.SetFloat("AttackType", 0f);
    }
    void WeaponFacing()
    {
        var x = playerMovement.movement.x;
        var y = playerMovement.movement.y;

        if (oldMovement != playerMovement.movement && currentWeaponEquipped != null)
        {
            currentWeaponEquipped.SetActive(false);
        }

        if (currentWeaponType == "MicWeapon")
        {
            if (x == 0 && y == 1) // Up
            {
                currentWeaponEquipped = micWeapons[0];
                attackType.declarations.Set("AttackType", "Up");
            }
            else if (x == 0 && y == -1) // Down
            {
                currentWeaponEquipped = micWeapons[1];
                attackType.declarations.Set("AttackType", "Down");
            }
            else if (x == -1 && y == 0) // Left
            {
                currentWeaponEquipped = micWeapons[2];
                attackType.declarations.Set("AttackType", "Left");
            }
            else if (x == 1 && y == 0) // Right
            {
                currentWeaponEquipped = micWeapons[3];
                attackType.declarations.Set("AttackType", "Right");
            }
            else if (x == -1 && y == 1) // Up Left
            {
                currentWeaponEquipped = micWeapons[4];
                attackType.declarations.Set("AttackType", "UpLeft");
            }
            else if (x == 1 && y == 1) // Up Right
            {
                currentWeaponEquipped = micWeapons[5];
                attackType.declarations.Set("AttackType", "UpRight");
            }
            else if (x == -1 && y == -1) // Down Left
            {
                currentWeaponEquipped = micWeapons[6];
                attackType.declarations.Set("AttackType", "DownLeft");
            }
            else if (x == 1 && y == -1) // Down Right
            {
                currentWeaponEquipped = micWeapons[7];
                attackType.declarations.Set("AttackType", "DownRight");
            }
        }

        if (currentWeaponEquipped != null)
        {
            currentWeaponEquipped.SetActive(true);
        }

        oldMovement.x = x;
        oldMovement.y = y;
    }

    void PickUpUpdate()
    {
        var micsOnGround = GameObject.FindGameObjectsWithTag("MicWeapon");

        foreach (GameObject mic in micsOnGround)
        {
            float distance = Vector3.Distance(mic.transform.position, transform.position);
            if (distance <= PickupRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentWeaponType = "MicWeapon";
                    Destroy(mic);
                }
            }
        }
    }
}
