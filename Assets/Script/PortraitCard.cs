using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PortraitCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHover = false;
    public string unitName;
    public string unitDamage;
    public string unitResistance;
    public string unitInitiative;
    public string unitTrait;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (isHover)
        {
            GetComponent<Outline>().enabled = true;

            // 더블 클릭으로?
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("clicked portrait card");
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                gameManager.battleManager.showInfoWindow("", "", unitInitiative, "", "");
            }
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
