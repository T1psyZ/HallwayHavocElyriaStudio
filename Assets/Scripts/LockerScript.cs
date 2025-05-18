using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockerScript : MonoBehaviour
{
    public GameObject locker, skillTreeButton;
    public Button skilltTreee, closeButton;
    public GameObject interactButton;
    GameObject player;

    bool canInteract = false;

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
        if (canInteract)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactButton.SetActive(true);
            Transform childTransform = transform.GetChild(0);
            GameObject childObject = childTransform.gameObject;
            childObject.SetActive(true);
            canInteract = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactButton.SetActive(false);
            Transform childTransform = transform.GetChild(0);
            GameObject childObject = childTransform.gameObject;
            childObject.SetActive(false);
            canInteract = false;
        }
    }
}
