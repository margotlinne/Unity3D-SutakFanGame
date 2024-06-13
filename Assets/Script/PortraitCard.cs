using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PortraitCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHover = false;

    public bool hoverOnCharacter = false;

    public int id = -1;
    public string unitName;
    public string unitDamage;
    public string unitResistance;
    public string unitInitiative;
    public string unitTrait;

    GameManager gameManager;
    Outline outline;
    Color outlineColor;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outlineColor = outline.effectColor;
    }

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (isHover)
        {
            outline.enabled = true;
            outline.effectColor = outlineColor;

            // 아웃라인
            for(int i =0; i < gameManager.battleManager.units.Count; i++)
            {
                GameObject unit = gameManager.battleManager.units[i];

                if(unit.GetComponent<IUnitData>().ID == id)
                {
                    unit.GetComponent<IUnitData>().OutlineObj.SetActive(true);
                    break;
                }
            }

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
        else if (!isHover)
        {
            if (hoverOnCharacter)
            {
                outline.enabled = true;
                outline.effectColor = Color.black;
            }
            else
            {
                outline.enabled = false;
            }

            // 아웃라인 감추기
            for (int i = 0; i < gameManager.battleManager.units.Count; i++)
            {
                GameObject unit = gameManager.battleManager.units[i];

                if (unit.GetComponent<IUnitData>().ID == id)
                {
                    unit.GetComponent<IUnitData>().OutlineObj.SetActive(false);
                    break;
                }
            }
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
