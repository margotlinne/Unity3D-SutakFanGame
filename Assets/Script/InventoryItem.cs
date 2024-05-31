using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create")]
public class InventoryItem : ScriptableObject
{
    public int id;
    public string itemName;
    [TextArea(5, 10)]
    public string description;
    public ItemType type;
    public Sprite icon;
    public bool consumable;
    public bool craftable;
    public bool stackable;
}

public enum ItemType { healthUp, damageUp, armourUp, evasionUp, criticalUp, Ingredient}