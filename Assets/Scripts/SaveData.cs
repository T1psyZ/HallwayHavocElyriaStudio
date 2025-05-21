using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public Vector3? playerPosition;
    public string mapBoundary; //Boundary name MAP
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> trashcanSaveData;
    public int exp;
    public int expToLevel; 
    public int level;
    public string sceneName; // The name of the scene to load
}
