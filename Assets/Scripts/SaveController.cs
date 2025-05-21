using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private TrashcanController trashcanController;
    public bool loadPosition = false;
    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData30.json");
        inventoryController = FindObjectOfType<InventoryController>();
        trashcanController = FindObjectOfType<TrashcanController>();
        LoadGame();
        SaveScene();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            trashcanSaveData = trashcanController.GetTrashcanItems(),
            exp = Stats_Manager.instance.currentExp,
            expToLevel = Stats_Manager.instance.expToLevel,
            level = Stats_Manager.instance.level
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void SaveScene()
    {
        var saveLocationScene = Path.Combine(Application.persistentDataPath, "saveScene.json");
        SaveData saveData = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name
        };
        File.WriteAllText(saveLocationScene, JsonUtility.ToJson(saveData));
    }

    public string GetSaveScene()
    {   
        if (File.Exists(Path.Combine(Application.persistentDataPath, "saveScene.json")))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "saveScene.json")));
            return saveData.sceneName;
        }
        else
        {
            return "InsideGymScene";
        }

    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {

            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            
            if (loadPosition) GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            Stats_Manager.instance.currentExp = saveData.exp;
            Stats_Manager.instance.expToLevel = saveData.expToLevel;
            Stats_Manager.instance.level = saveData.level;
            if (FindObjectOfType<CinemachineConfiner>() != null && FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D != null)
            {
                FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            }

            if (inventoryController != null)
            {
                inventoryController.SetInventoryItems(saveData.inventorySaveData);
            }
            if (trashcanController != null)
            {
                trashcanController.SetTrashcanItems(saveData.trashcanSaveData);
            }
        }
        else
        {
            SaveGame();
        }
    }
}

