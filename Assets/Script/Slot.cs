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
    private bool hoverWindow = false;
    public bool isHover = false;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (isEmpty) { itemInSlot.gameObject.SetActive(false); }
        else if (!isEmpty) { itemInSlot.gameObject.SetActive(true); }
        

        if (amount > 1 && !isEmpty)
        {
            amountTxt.text = amount.ToString(); 
        }

        if (isHover)
        {
            if (Input.GetMouseButtonDown(1) && !isEmpty)
            {
                Debug.Log("clicked");
                gameManager.inventoryManager.clickedSlot = this;
                gameManager.inventoryManager.setRightClickWindow();
            }

            if (!gameManager.inventoryManager.windowOn)
            {
                wait = StartCoroutine(ShowItemDetail());
            }
            else
            {
                if (wait != null)
                {
                    StopCoroutine(wait);
                }
                gameManager.inventoryManager.hideHoverWindow();
            }
        }
        else
        {
            if (wait != null)
            {
                StopCoroutine(wait);
            }

            hoverWindow = false;
            //gameManager.inventoryManager.hideHoverWindow();
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
        if (!hoverWindow)
        {
            yield return new WaitForSeconds(1f);

        }
        // 1초 후에 수행할 작업
        gameManager.inventoryManager.showHoverWindow(id);
        hoverWindow = true;
    }

}
