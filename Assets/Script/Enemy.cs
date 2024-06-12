using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IUnitData
{
    NavMeshAgent agent;
    GameManager gameManager;
    public Enemies enemyType;
    private bool joinedBattle = false;
    private bool isHover = false;

    [HideInInspector] public int initiative;
    public int Initiative => initiative;

    public Sprite portrait;
    public Sprite Portrait => portrait;

    [HideInInspector] public int id;
    public int ID => id;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        initiative = enemyType.initiative;
        id = enemyType.id;
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

        if (gameManager.battleManager.inBattle)
        {
            if (isHover)
            {
                for (int i = 0; i < gameManager.battleManager.cards.Count; i++)
                {
                    PortraitCard card = gameManager.battleManager.cards[i];
                    if (card.id == id)
                    {
                        card.hoverOnCharacter = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < gameManager.battleManager.cards.Count; i++)
                {
                    PortraitCard card = gameManager.battleManager.cards[i];
                    if (card.id == id)
                    {
                        card.hoverOnCharacter = false;
                        break;
                    }
                }
            }
        }
    }

    public void OnMouseEnter()
    {
        isHover = true;
    }

    public void OnMouseExit()
    {
        isHover = false;
    }
}

public class EnemyPattern
{
    private float health;
    public EnemyPattern()
    {

    }
}
