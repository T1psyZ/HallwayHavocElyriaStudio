using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangeOnTimer : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    public bool additive;
    public string sceneToUnload = null  ;
    bool done = false;
    void Update()
    {
        changeTime -= Time.deltaTime;
        if(changeTime <= 0 && !done)
        {
            SceneManager.LoadScene(sceneName);

        }
    }
}
