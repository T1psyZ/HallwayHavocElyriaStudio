using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkillNode
{
    public string skillName;
    public Image skillIcon;
    public Button skillButton;
    public TMP_Text statusText;
    public List<SkillNode> unlockSkills;
    public List<SkillNode> disableSkills;
    public bool isUnlocked = false;
    public int currentPoints = 0;
    public int maxPoints = 1;
}