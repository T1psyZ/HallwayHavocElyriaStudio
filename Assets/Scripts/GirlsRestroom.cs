using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GirlsRestroom : MonoBehaviour
{
    public string GirlsRestroomLoad;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        { 
            SceneManager.LoadScene(GirlsRestroomLoad);
        }
    }
}
