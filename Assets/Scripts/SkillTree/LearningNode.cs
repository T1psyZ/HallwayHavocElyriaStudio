using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

[System.Serializable]
public class LearningNode
{
    public string learningName;
    public Image learningIcon;
    public Button learningButton;
    public TMP_Text statusText;
    public GameObject contextMenu;
    public bool isUnlocked = false;
    public int currentPoints = 0;
    public int maxPoints = 1;
}