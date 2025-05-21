using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnTimer1 : MonoBehaviour
{
    public float quitTime = 5f; // Time in seconds before quitting
    private bool done = false;

    void Update()
    {
        quitTime -= Time.deltaTime;
        if (quitTime <= 0 && !done)
        {
            done = true;
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
