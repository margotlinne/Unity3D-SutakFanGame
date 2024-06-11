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
        if (gameManager.battleManager.inBattle && !generatedCards)
        {
            for (int i = 0; i < gameManager.battleManager.units.Count; i++)
            {
                GameObject card = gameManager.battleManager.units[i];
                GameObject newCard = Instantiate(portraitCard);
                newCard.transform.SetParent(parent, false);
                newCard.GetComponent<Image>().sprite = card.GetComponent<IUnitData>().Portrait;
                PortraitCard _card = newCard.GetComponent<PortraitCard>();
                _card.unitInitiative = card.GetComponent<IUnitData>().Initiative.ToString();
            }
            GameObject endCard = Instantiate(endOfTurn);
            endCard.transform.SetParent(parent, false);

            generatedCards = true;
        }
    }
}
