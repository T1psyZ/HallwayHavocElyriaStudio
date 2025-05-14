using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerScript : MonoBehaviour
{
    public GameObject locker, skillTreeButton;
    public Button skilltTreee, closeButton;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        closeButton.onClick.AddListener(CloseLocker);
        if (skilltTreee != null)
        {
            skilltTreee.onClick.AddListener(SkillTree);
        }
    }

    public void OpenLocker()
    {
        var dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist <= 1)
        {
            locker.SetActive(true);
        }
    }

    void CloseLocker()
    {
        locker.SetActive(false);
    }

    void SkillTree()
    {
        skilltTreee.gameObject.SetActive(false);
        skillTreeButton.SetActive(true);
    }
}
