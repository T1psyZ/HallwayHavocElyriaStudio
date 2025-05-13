using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    public GameObject[] uiDeactivate; 
    public GameObject[] uiActivate;
    public TMPro.TMP_Text text;
    public float timerDuration = 10f;

    private float timer;
    void Start()
    {
        timer = timerDuration;
        foreach (var ui in uiDeactivate)
        {
            ui.SetActive(false);
        }
        foreach (var ui in uiActivate)
        {
            ui.SetActive(true);
        }
    }

    void Update()
    {
        // Decrease the timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // Update the timer text
            text.text = $"Time Remaining: {Mathf.Ceil(timer)}s";
        }
        else
        {
            // Timer has ended, trigger an action
            TimerEnded();
        }
    }

    private void TimerEnded()
    {
        // Example action: Deactivate all UI elements
        foreach (var ui in uiActivate)
        {
            ui.SetActive(false);
        }

        foreach (var ui in uiDeactivate)
        {
            ui.SetActive(true);
        }
    }
}
