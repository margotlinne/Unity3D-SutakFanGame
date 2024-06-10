using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IUnitData
{
    NavMeshAgent agent;
    GameManager gameManager;
    public Enemies enemyType;
    private bool joinedBattle;

    [HideInInspector] public int initiative;
    public int Initiative => initiative;

    public Sprite portrait;
    public Sprite Portrait => portrait;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        initiative = enemyType.initiative;
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (gameManager.battleManager.inBattle && !joinedBattle)
        {
            gameManager.battleManager.units.Add(this.gameObject);
            joinedBattle = true;
        }
    }
}

public class EnemyPatter
{
    private float health;
    public EnemyPatter()
    {

    }
}
