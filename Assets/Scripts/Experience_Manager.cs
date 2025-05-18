using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Experience_Manager : MonoBehaviour
{
    public float expGrowthMultipliers = 2f;
    public Slider expSlider;
    public TMP_Text currentLevelText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }


    private void OnEnable()
    {
        // Fix: Subscribe to the event from an instance of Enemy_health
        Enemy_health enemyHealth = FindObjectOfType<Enemy_health>();
        if (enemyHealth != null)
        {
            enemyHealth.OnMonsterDefeated += GainExperience;
        }
    }

    private void OnDisable()
    {
        // Fix: Unsubscribe from the event from the same instance of Enemy_health
        Enemy_health enemyHealth = FindObjectOfType<Enemy_health>();
        if (enemyHealth != null)
        {
            enemyHealth.OnMonsterDefeated -= GainExperience;
        }
    }

    public void GainExperience(int amount)
    {
        Stats_Manager.instance.currentExp += amount;
        if (Stats_Manager.instance.currentExp >= Stats_Manager.instance.expToLevel)
        {
            LevelUp();
        }

        
    }

    public void LevelUp()
    {
        Stats_Manager.instance.skillPoints += 2;
        Stats_Manager.instance.level++;
        Stats_Manager.instance.currentExp -= Stats_Manager.instance.expToLevel;
        Stats_Manager.instance.expToLevel = Mathf.RoundToInt(Stats_Manager.instance.expToLevel * expGrowthMultipliers);
    }

    public void UpdateUI()
    {
        expSlider.maxValue = Stats_Manager.instance.expToLevel;
        expSlider.value = Stats_Manager.instance.currentExp;
        currentLevelText.text = "Level: " + Stats_Manager.instance.level;
    }
}
