using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public Vector3 playerPosition;
    public string mapBoundary; //Boundary name MAP
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> trashcanSaveData;
}
