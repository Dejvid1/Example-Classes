using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Create Item",menuName = "Item",order = 1)]
public class Item: ScriptableObject 
{
    public string name;
    public string description;
    public Sprite icon;
    public int maxStack;
    public Rarity rarity;
    public int maxSpawnedAmount;
    public int id;
    public enum Rarity
    {
        common = 100,
        rare = 50,
        epic = 25,
        legendary = 10,
    }
}

