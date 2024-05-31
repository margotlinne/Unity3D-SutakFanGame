using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Coroutine wait;
    GameManager gameManager;
    public Image itemInSlot;
    public TextMeshProUGUI amountTxt;
    public int amount;
    public bool isEmpty = true;
    public int id;
    private bool isHover = false;

    void Start()
    {
        gameManager = GameManager.instance;

        setImageNText();
    }

    void setImageNText()
    {
        if (isEmpty) { itemInSlot.gameObject.SetActive(false); }
        else if (!isEmpty) { itemInSlot.gameObject.SetActive(true); }


        if (amount > 1 && !isEmpty)
        {
            amountTxt.text = amount.ToString();
        }
        else
        {
            amountTxt.text = "";
        }
    }

    void Update()
    {
        setImageNText();

        if (isHover)
        {
            // 우클릭 시 아이템 버리기 등 작업 창 띄우기
            if (Input.GetMouseButtonDown(1) && !isEmpty)
            {
                Debug.Log("clicked");
                gameManager.inventoryManager.clickedSlot = this;
                gameManager.inventoryManager.setRightClickWindow();
            }



            // 호버 시 호버 창 띄우기 (wait이 null일 때 조건을 안 주니 이상했음)
            if (wait == null)
            {
                wait = StartCoroutine(ShowItemDetail());
            }
            
            // 우클릭 윈도우나 버리기/나누기 숫자 셀렉터 윈도우가 떠 있을 땐 호버 숨기기
            if (gameManager.inventoryManager.windowOn)
            {
                HideHover();
            }
            //Debug.Log("호버");        
        }
        else
        {
            HideHover();
        }        
    }

    void HideHover()
    {
        if (gameManager.inventoryManager.hoverId == id)
        {
            gameManager.inventoryManager.hideHoverWindow();
        }

        if (wait != null)
        {
            StopCoroutine(wait);
            wait = null;
        }
    }

    public void resetSlot()
    {
        id = 0;
        isEmpty = true;
        amount = 0;
        amountTxt.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }


    IEnumerator ShowItemDetail()
    {
        // 1초 대기
        yield return new WaitForSeconds(1f);


        // 1초 후에 수행할 작업
        gameManager.inventoryManager.showHoverWindow(id);

    }

}
