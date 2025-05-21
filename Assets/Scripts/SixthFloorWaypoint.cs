using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SixthFloorWaypoint : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Assign in Inspector

    SaveController saveController;

    private void Start()
    {
        saveController = FindObjectOfType<SaveController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "7thFloorAftermath")
            {
                
                saveController.SaveGame();
            }
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
