using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.IO;
public class SkillTreeManager : MonoBehaviour
{
    public TMP_Text skillPointsText;
    public List<SkillNode> allSkills;
    public List<LearningNode> allLearning;

    void Start()
    {
        LoadSkillTree();
    }

    void InitializeSkillNode(SkillNode skill, HashSet<SkillNode> visited)
    {
        if (skill == null || visited.Contains(skill))
            return;

        visited.Add(skill);

        skill.skillButton.onClick.AddListener(() => TryUnlockSkill(skill, false, 0));
        UpdateSkillUI(skill);

        foreach (var unlock in skill.unlockSkills)
        {
            InitializeSkillNode(unlock, visited);
        }
    }

    void InitializeSkillNodeDatabase(SkillNode skill, HashSet<SkillNode> visited)
    {
        if (skill == null || visited.Contains(skill))
            return;

        visited.Add(skill);
        skill.skillButton.onClick.AddListener(() => TryUnlockSkill(skill, false, 0));

        TryUnlockSkill(skill, true, skill.currentPoints);

        foreach (var unlock in skill.unlockSkills)
        {
            InitializeSkillNodeDatabase(unlock, visited);

        }
    }

    void TryUnlockLearning(LearningNode learning)
    {
        if (learning.isUnlocked)
        {
            learning.contextMenu.SetActive(!learning.contextMenu.activeSelf);
            return;
        }
        if (Stats_Manager.instance.skillPoints <= 0)
        {
            return;
        }
        learning.isUnlocked = true;
        learning.currentPoints++;
        UpdateLearningUi(learning);
        SaveSkillTree();
    }

    void TryUnlockSkill(SkillNode skill, bool load, int points)
    {
        Debug.Log($"Skill: {skill.skillName}, Unlocked: {skill.isUnlocked}, Points: {skill.currentPoints}/{skill.maxPoints}, Load: {load}, Points: {points}");
        if (!skill.isUnlocked || points <= 0 && load) return;

        skill.currentPoints = !load ? skill.currentPoints+1: points;
        Stats_Manager.instance.skillPoints = !load ? Stats_Manager.instance.skillPoints-1 : Stats_Manager.instance.skillPoints;
        UpdateSkillUI(skill);
        if (skill.skillName.Contains("Health"))
        {
            Stats_Manager.instance.maxHealth += 1;
            ;
        }
        else if (skill.skillName.Contains("Damage"))
        {
            Stats_Manager.instance.attackDamage += 2;
        }
        else if (skill.skillName.Contains("Stamina"))
        {
            Stats_Manager.instance.maxStamina += 1;
        }

        if (skill.currentPoints == skill.maxPoints)
        {
            foreach (var unlock in skill.unlockSkills)
            {
                unlock.isUnlocked = true;
                UpdateSkillUI(unlock);
            }
        }

        foreach (var disable in skill.disableSkills)
        {
            disable.isUnlocked = false;
            UpdateSkillUI(disable);
        }
        if (!load) SaveSkillTree();
    }

    void UpdateSkillUI(SkillNode skill)
    {
        
        skill.skillButton.interactable = skill.isUnlocked && skill.currentPoints < skill.maxPoints && Stats_Manager.instance.skillPoints > 0;
        skill.statusText.text = skill.isUnlocked ? $"{skill.currentPoints}/{skill.maxPoints}" : skill.currentPoints == skill.maxPoints ? "Maxed" : "Locked";
        skill.skillIcon.color = skill.isUnlocked ? Color.white : Color.gray;
        skill.statusText.color = skill.isUnlocked ? Color.white : Color.black;
        skillPointsText.text = $"Skill Points: {Stats_Manager.instance.skillPoints}";
    }

    void UpdateLearningUi(LearningNode learning)
    {
        learning.statusText.text = learning.isUnlocked ? "Learned" : "Locked";
        learning.learningIcon.color = learning.isUnlocked ? Color.white : Color.gray;
        learning.statusText.color = learning.isUnlocked ? Color.white : Color.black;
        skillPointsText.text = $"Skill Points: {Stats_Manager.instance.skillPoints}";
    }

    public void SaveSkillTree()
    {
        var skillData = allSkills;

        var learningData = allLearning;

        var saveObj = new SkillTreeSaveData { allSkills = skillData, allLearning = learningData };
        string json = JsonUtility.ToJson(saveObj, true);

        // Save to file (for example)
        File.WriteAllText(Application.persistentDataPath + "/skilltree_save20.json", json);
        Debug.Log("Skill tree saved: " + json);
    }

    public void LoadSkillTree()
    {
        string path = Application.persistentDataPath + "/skilltree_save20.json";
        if (!File.Exists(path))
        {
            foreach (var skill in allSkills)
            {
                InitializeSkillNode(skill, new HashSet<SkillNode>());
                foreach (var disable in skill.disableSkills)
                {
                    disable.isUnlocked = false;
                    UpdateSkillUI(disable);
                }
            }

            foreach (var learning in allLearning)
            {
                learning.learningButton.onClick.AddListener(() => TryUnlockLearning(learning));
                UpdateLearningUi(learning);
            }
            return;
        }

        string json = File.ReadAllText(path);
        var saveObj = JsonUtility.FromJson<SkillTreeSaveData>(json);
        var visited = new HashSet<SkillNode>();
        allSkills = saveObj.allSkills;

        foreach (var skill in saveObj.allSkills)
        {
            InitializeSkillNodeDatabase(skill, new HashSet<SkillNode>());
            foreach (var disable in skill.disableSkills)
            {
                disable.isUnlocked = false;
                UpdateSkillUI(disable);
            }
        }

        foreach (var learning in allLearning)
        {
            foreach (var save in saveObj.allLearning)
            {
                if (learning.learningName == save.learningName)
                {
                    learning.isUnlocked = save.isUnlocked;
                    break;
                }
            }
            learning.learningButton.onClick.AddListener(() => TryUnlockLearning(learning));
            UpdateLearningUi(learning);
        }

        Debug.Log("Skill tree loaded: " + json);
    }

    public class SkillTreeSaveData
    {
        public List<SkillNode> allSkills;
        public List<LearningNode> allLearning;
    }
}
