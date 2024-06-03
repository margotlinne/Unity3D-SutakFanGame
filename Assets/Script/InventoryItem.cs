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
    public ItemType iteype;
    public ArmorType armorType;
    public PotionType potionType;
    public Sprite icon;
    public bool consumable;
    public bool craftable;
    public bool stackable;
    public bool equipable;
}
public enum PotionType { healthUp, damageUp, armourUp, evasionUp, criticalUp, None}
public enum ItemType { Potion, Ingredient, Armor}
public enum ArmorType { cape, hat, boots, gloves, None}