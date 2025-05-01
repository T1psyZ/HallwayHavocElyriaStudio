using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Tree/Skill ISO")]
public class SkillISO : ScriptableObject
{
    public string skillName;
    public string maxLevel;
    public Sprite skillIcon;
   
}
