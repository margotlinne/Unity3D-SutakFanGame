using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Create")]
public class Enemies : ScriptableObject
{
    public int id;
    public float health;
    public string enemyName;
    public int initiative;
    public float damage;
}
