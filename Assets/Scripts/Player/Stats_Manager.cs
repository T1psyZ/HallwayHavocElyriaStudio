using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stats_Manager : MonoBehaviour
{
    public static Stats_Manager instance;
    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Stamina")]
    public int currentStamina;
    public int gainStamina;
    public int useStamina;
    public int maxStamina;

    [Header("Experience")]
    public int level;
    public int currentExp;
    public int expToLevel;

    [Header("Combat")]
    public float weaponRange ;
    public int attackDamage;
    public float dashSpeed ;
    public float dashDuration ;
    public float cooldown;
    public float knockbackForce ;
    public float knockbackDuration ;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;

    private void Awake()
    {
         if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
