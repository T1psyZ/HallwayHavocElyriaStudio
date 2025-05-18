using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public int itemID;
    public string lootName;
    public string name;
    public int dropChance;

    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;   
        this.dropChance = dropChance;
    }

}

