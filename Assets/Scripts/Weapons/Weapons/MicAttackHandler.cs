using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIcAttackHandler : MonoBehaviour
{
    Animator weaponAnimator;
    void Start()
    {
        weaponAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Done()
    {
        //weaponAnimator.SetFloat("AttackType", 0f);
    }
}
