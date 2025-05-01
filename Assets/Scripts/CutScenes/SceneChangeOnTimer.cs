using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangeOnTimer : MonoBehaviour
{
    public float changeTime;
    public string sceneName;

    void Update()
    {
        changeTime -= Time.deltaTime;
        if(changeTime <= 0)
        {
           SceneManager.LoadScene(sceneName);   
        }
    }
}
