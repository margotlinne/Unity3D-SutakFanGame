using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlaceCasrds : MonoBehaviour
{
    public GameObject portraitCard;
    public GameObject endOfTurn;
    public Transform parent;

    private bool generatedCards;
    
    GameManager gameManager;
    

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (gameManager.battleManager.inBattle && !generatedCards && gameManager.battleManager.addedTurns)
        {
            placingCards();               
        }

        
        reorderCards();
        
        
    }


    void reorderCards()
    {
        for(int i = 0; i < gameManager.battleManager.cards.Count; i++)
        {
            PortraitCard card = gameManager.battleManager.cards[i];
            // 첫 번째 턴
            if (i < gameManager.battleManager.units.Count)
            {
                card.transform.SetSiblingIndex(i);
                Debug.Log(card);
                Debug.Log(card.transform.GetSiblingIndex());
            }
            // 두 번째 턴 (턴 종료 표시 오브젝트는 가만 두기 위해)
            else
            {
                Debug.Log("?");
                card.transform.SetSiblingIndex(i + 1);
                Debug.Log(card);
                Debug.Log(card.transform.GetSiblingIndex());
            }
            
        }
    }

    void placingCards()
    {
        for (int i = 0; i < gameManager.battleManager.turns.Count; i++)
        {

            GameObject card = gameManager.battleManager.turns[i];
            GameObject newCard = Instantiate(portraitCard);

            newCard.transform.SetParent(parent, false);
            newCard.GetComponent<Image>().sprite = card.GetComponent<IUnitData>().Portrait;

            PortraitCard _card = newCard.GetComponent<PortraitCard>();
            _card.unitInitiative = card.GetComponent<IUnitData>().Initiative.ToString();
            _card.id = card.GetComponent<IUnitData>().ID;
            gameManager.battleManager.cards.Add(newCard.GetComponent<PortraitCard>());

            if (i >= gameManager.battleManager.turns.Count / 2)
            {
                newCard.GetComponent<Image>().color = Color.gray;
            }

        }
        
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                GameObject endCard = Instantiate(endOfTurn);
                endCard.transform.SetParent(parent, false);
                // 첫 번째 턴 갯수 후에 턴 마지막임을 뜻하는 오브젝트 추가
                endCard.transform.SetSiblingIndex(gameManager.battleManager.units.Count);
            }
            else
            {
                GameObject endCard = Instantiate(endOfTurn);
                endCard.transform.SetParent(parent, false);
            }
        }       
        
        generatedCards = true;
    }

    void addOneTurn()
    {
        //for (int i = 0; i < gameManager.battleManager.units.Count; i++)
        //{
        //    GameObject card = gameManager.battleManager.units[i];
        //    GameObject newCard = Instantiate(portraitCard);

        //    newCard.transform.SetParent(parent, false);
        //    newCard.GetComponent<Image>().sprite = card.GetComponent<IUnitData>().Portrait;

        //    PortraitCard _card = newCard.GetComponent<PortraitCard>();
        //    _card.unitInitiative = card.GetComponent<IUnitData>().Initiative.ToString();
        //    _card.id = card.GetComponent<IUnitData>().ID;
        //    gameManager.battleManager.cards.Add(newCard.GetComponent<PortraitCard>());
        //}
        //GameObject endCard = Instantiate(endOfTurn);
        //endCard.transform.SetParent(parent, false);

        // 앞서 비활성화된 두 개의 턴을 원래 순서대로 (units) 뒤로 배치
    }
}
