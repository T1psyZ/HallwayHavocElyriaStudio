using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots;
    public SkillISO skillISO;
    public Image skillIcon;
    public int currentLevel;

    public bool isUnlocked;
    public TMP_Text skillLevelText;
    public Button skillButton;

    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMax;

    private void OnValidate()
    {
        if (skillISO != null && skillLevelText != null)
        {
            // Ensure maxLevel is parsed to an integer before comparison
            if (int.TryParse(skillISO.maxLevel, out int maxLevel))
            {
                UpdateUI();
            }
        }
    }

    public void TryUpgradeSkill()
    {
        // Parse maxLevel to an integer for comparison
        if (isUnlocked && int.TryParse(skillISO.maxLevel, out int maxLevel) && currentLevel < maxLevel)
        {
            currentLevel++;
            if (gameObject.name.Contains("Health"))
            {
                Stats_Manager.instance.maxHealth += 1;
            }else if (gameObject.name.Contains("Damage"))
            {
                Stats_Manager.instance.attackDamage += 2;
            }
            else if (gameObject.name.Contains("Stamina"))
            {
                Stats_Manager.instance.maxStamina += 1;
            }
                OnAbilityPointSpent?.Invoke(this);

            // Fix: Parse maxLevel to an integer for comparison
            if (currentLevel >= maxLevel)
            {
                OnSkillMax?.Invoke(this);
            }
            UpdateUI();
        }
    }
    public bool CanUnlockSkill()
    {
        foreach (SkillSlot slot in prerequisiteSkillSlots)
        {
            //if (slot.isUnlocked || slot.currentLevel < int.Parse(slot.skillISO.maxLevel))
            //{
            //    return false;
            //}
        }
        return true;
    }

    public void Unlock()
    {
        isUnlocked = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (skillISO != null)
        {
            skillIcon.sprite = skillISO.skillIcon;

            if (isUnlocked)
            {
                skillButton.interactable = true;
                skillLevelText.text = currentLevel + "/" + skillISO.maxLevel;
                skillIcon.color = Color.white;
                skillLevelText.color = Color.white;
            }
            else
            {
                skillButton.interactable = false;
                skillLevelText.text = "Locked";
                skillIcon.color = Color.gray;
                skillLevelText.color = Color.black;
            }
        }
    }
}
