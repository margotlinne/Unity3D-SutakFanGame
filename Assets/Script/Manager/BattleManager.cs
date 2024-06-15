using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IUnitData
{
    int Initiative { get; }

    Sprite Portrait { get; }

    int ID { get; }

    bool IsInBattle { get; }
}

public class BattleManager : MonoBehaviour
{
    public bool toMove = false;
    public bool clickedToMove = false;
    public bool inBattle = false;

    public TextMeshProUGUI unitNameTxt;
    public TextMeshProUGUI unitDamageTxt;
    public TextMeshProUGUI unitResistanceTxt;
    public TextMeshProUGUI unitInitiativeTxt;
    public TextMeshProUGUI unitTraitTxt;

    public List<GameObject> units = new List<GameObject>();
    public List<PortraitCard> cards = new List<PortraitCard>();
    public List<GameObject> turns = new List<GameObject>();

    public GameObject unitInfoWindow;
    public GameObject battleCanvas;
    public GameObject actionGroup;

    public int idHoverOnCard = -1;
    public int idHoverOnCharacter = -1;

    public int currentTurn = 0;

    public bool addedTurns = false;

    private bool setUnit = false;

    public bool delayedTurn = false;

    GameManager gameManager;

    Coroutine wait;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        battleCanvas.SetActive(inBattle);


        if (!inBattle)
        {
            units.Clear();

            if (wait != null)
            {
                StopCoroutine(wait);
            }
        }
        else
        {
            if (!setUnit)
            {
                wait = StartCoroutine(WaitForSettingTurns());
            }
            else
            {
                // 주도권 값을 기준으로 오름차순으로 정렬
                units.Sort((x, y) =>
                {
                    IUnitData xInitiative = x.GetComponent<IUnitData>();
                    IUnitData yInitiative = y.GetComponent<IUnitData>();
                    return xInitiative.Initiative.CompareTo(yInitiative.Initiative);
                });

                if (!addedTurns)
                {
                    // 실제 턴 순서대로 게임 오브젝트를 리스트에 추가
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < units.Count; j++)
                        {
                            turns.Add(units[j]);
                        }
                    }
                    addedTurns = true;
                }
                else
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (i == currentTurn)
                        {
                            cards[i].transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                        }
                        else 
                        {
                            cards[i].transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                    }
                }
            }            
            
                       
        }
    }

    public void showActionGroup()
    {
        actionGroup.SetActive(true);
    }
    public void hideActionGroup()
    {
        actionGroup.SetActive(false);
    }

    public void showInfoWindow(string name, string damage, string initiative, string resistance, string trait)
    {
        unitInfoWindow.SetActive(true);
        GameManager.instance.uiManager.activeUI.Add(unitInfoWindow);

        unitInitiativeTxt.text = "주도권: " + initiative;
    }

    public void moveBtn()
    {
        toMove = true;
        Debug.Log("clicked move button");
    }

    public void endTurnBtn()
    {
        cards[currentTurn].gameObject.SetActive(false);
        currentTurn++;
    }

    public void dealyTurnBtn()
    {
        PortraitCard card = cards[currentTurn];
        for (int i = 0; i < units.Count - 1; i++)
        {
            cards[i] = cards[i + 1];
        }
        cards[units.Count - 1] = card;


        

        GameObject turn = turns[currentTurn];
        for (int i = 0; i < units.Count - 1; i++)
        {
            turns[i] = turns[i + 1];
        }
        turns[units.Count - 1] = turn;

        delayedTurn = true;
    }

    IEnumerator WaitForSettingTurns()
    {
        yield return new WaitForSeconds(0.5f);

        // 대기 후 실행할 작업
        setUnit = true;
        // 예: 특정 게임 오브젝트를 리스트에 추가
        // addedObjects.Add(someGameObject);
    }

}
