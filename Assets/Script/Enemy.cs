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

    public GameObject outlineObj;

    public bool isInBattle;
    public bool IsInBattle => isInBattle;

    public BattleRange range;

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
        if (gameManager.battleManager.inBattle && range.playerEntered && !joinedBattle)
        {
            isInBattle = true;
            gameManager.battleManager.units.Add(this.gameObject);
            joinedBattle = true;
        }

        if (gameManager.battleManager.inBattle)
        {
            if (isHover)
            {
                gameManager.battleManager.idHoverOnCharacter = id;
                outlineObj.SetActive(true);
            }
            // 호버하고 있는 턴 카드가 자신을 가리키는 것이면 아웃라인 활성화
            if (gameManager.battleManager.idHoverOnCard == id)
            {
                outlineObj.SetActive(true);
            }
            else if (!isHover)
            {
                outlineObj.SetActive(false);
            }

            if (gameManager.battleManager.idDoubleClick == id)
            {
                gameManager.cameraTarget = this.gameObject;
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
        gameManager.battleManager.idHoverOnCharacter = -1;
        outlineObj.SetActive(false);
    }
}

public class EnemyPattern
{
    private float health;
    public EnemyPattern()
    {

    }
}
