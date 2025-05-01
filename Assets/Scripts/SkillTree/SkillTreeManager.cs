using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
  
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;
    public int availablePoints;


    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointsSpent;
        SkillSlot.OnSkillMax += HandleSkillMaxed;
    }
    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointsSpent;
        SkillSlot.OnSkillMax -= HandleSkillMaxed;
    }
    private void Start()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            Debug.Log($"Skill: {slot.name} has {slot.prerequisiteSkillSlots.Count} prerequisites.");
            foreach (SkillSlot pre in slot.prerequisiteSkillSlots)
            {
                Debug.Log($"-- Prerequisite: {pre.name}");
            }

            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot));
        }
        UpdatePointsText(0); // Initialize points text
    }

    private void CheckAvailablePoints(SkillSlot slot)
    {
        if (availablePoints > 0 )
        {
            slot.TryUpgradeSkill();
        }
    }

    private void HandleAbilityPointsSpent(SkillSlot skillslot)
    {
        // Check if the skill is already unlocked
        if (availablePoints > 0 )
        {
            
            UpdatePointsText(-1); // Update points text
        }
       
    }

    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        Debug.Log($"Skill Maxed: {skillSlot.name}");

        foreach (SkillSlot slot in skillSlots)
        {
            if (slot.prerequisiteSkillSlots.Contains(skillSlot))
            {
                Debug.Log($"{slot.name} has {skillSlot.name} as prerequisite.");

                if (!slot.isUnlocked)
                {
                    Debug.Log($"{slot.name} is currently locked. Checking CanUnlockSkill...");

                    if (slot.CanUnlockSkill())
                    {
                        Debug.Log($"{slot.name} meets all conditions. Unlocking now!");
                        slot.Unlock();
                    }
                    else
                    {
                        Debug.Log($"{slot.name} cannot unlock yet (other prerequisites not complete).");
                    }
                }
                else
                {
                    Debug.Log($"{slot.name} is already unlocked, skipping.");
                }
            }
        }
    }


    public void UpdatePointsText(int amount)
    {
        availablePoints += amount;
        pointsText.text = "Points: " + availablePoints;
    }
}
