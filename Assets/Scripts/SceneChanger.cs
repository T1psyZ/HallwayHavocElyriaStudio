using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator fadeAnim;
    public float fadeTime = .5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);
         SceneManager.LoadScene(sceneToLoad);
    }
}
