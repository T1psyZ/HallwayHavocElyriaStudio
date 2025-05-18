using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    void Update()
    {
        
    }
    
    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Menu()
    {
       
    }
}
