using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItems : MonoBehaviour
{
    public GameObject itemPrefab;
    [Range(0, 100)] public float dropChance;
}
