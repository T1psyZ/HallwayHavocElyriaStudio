using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Experience_Manager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToLevel;
    public float expGrowthMultipliers = 2f;
    public Slider expSlider;
    public TMP_Text currentLevelText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GainExperience(2);
        }
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
        currentExp += amount;
        if (currentExp >= expToLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    public void LevelUp()
    {
        level++;
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultipliers);
    }

    public void UpdateUI()
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }
}
