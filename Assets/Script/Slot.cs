using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    Image slotBox;
    Coroutine wait;
    GameManager gameManager;
    public Image itemImage;
    public TextMeshProUGUI amountTxt;
    public int amount;
    public bool isEmpty = true;
    public int id;
    private bool isHover = false;
    public int slotId;
    [HideInInspector] public ItemDragDrop slotItem;


    void Awake()
    {
        slotItem = itemImage.GetComponent<ItemDragDrop>();
        slotBox = GetComponent<Image>();
    }

    void Start()
    {
        gameManager = GameManager.instance;

        setImageNText();
    }

    void setImageNText()
    {
        if (isEmpty) { itemImage.gameObject.SetActive(false); }
        else if (!isEmpty) { itemImage.gameObject.SetActive(true); }


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
            if (!isEmpty)
            {
                // 우클릭 시 아이템 버리기 등 작업 창 띄우기
                if (Input.GetMouseButtonDown(1))
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
            }

            if (gameManager.inventoryManager.grappedItem)
            {
                changeTransparency(0.5f);
            }
           
        }
        else if (!isHover)
        {
            changeTransparency(1f);
            HideHover();
        }        
    }

    void HideHover()
    {
        // 현재 떠 있는 호버의 슬롯 아이디가 내 아이디일 때 즉 나의 호버 창이 떠 있을 때
        if (gameManager.inventoryManager.hoverId == slotId)
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
    
    void changeTransparency(float value)
    {
        Color slotColor = slotBox.color;
        slotColor.a = value;
        slotBox.color = slotColor;
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
        gameManager.inventoryManager.showHoverWindow(id, slotId);

    }

    // 아이템이 해당 슬롯에 드롭될 때
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        ItemDragDrop itemDragDrop = droppedItem.GetComponent<ItemDragDrop>();


        // 놓으려는 슬롯에 아이템이 없다면
        if (isEmpty)
        {
            Debug.Log("빈 슬롯에 아이템 드롭");
            id = itemDragDrop.droppedItemID;
            amount = itemDragDrop.droppedItemAmount;
            itemImage.sprite = Resources.Load<Sprite>(itemDragDrop.droppedItemImagePath);

            // 원래 아이템이 있던 슬롯 초기화
            itemDragDrop.parentObj.resetSlot();

            // 아이템을 받은 해당 슬롯의 아이템 업데이트            
            slotItem.setParentData();
            isEmpty = false;


            Debug.Log("id: " + id + " amoount: " + amount);
            
        }
        // 아이템이 있는 슬롯이나 본인 슬롯이 아닌 경우
        else if (slotId != itemDragDrop.parentObj.slotId)
        {
            Debug.Log("아이템 있는 슬롯에 아이템 드롭");
            int changeID = 0;
            int changeAmount = 0;
            string changeImagePath = "";

            // 놓으려는 슬롯의 아이템과 놓는 아이템의 종류가 같다면
            if (id == itemDragDrop.droppedItemID)
            {
                amount++;

                // 원래 아이템이 있던 슬롯 초기화
                itemDragDrop.parentObj.resetSlot();

                // 아이템을 받은 해당 슬롯의 아이템 업데이트
                slotItem.setParentData();
            }
            else
            {
                // 놓는 아이템과 놓으려는 곳의 아이템의 데이터를 교환
                changeID = id;
                changeAmount = amount;
                string path = AssetDatabase.GetAssetPath(itemImage.sprite);
                changeImagePath = path.Replace("Assets/Resources/", "").Replace(".png", "");

                id = itemDragDrop.droppedItemID;
                amount = itemDragDrop.droppedItemAmount;
                itemImage.sprite = Resources.Load<Sprite>(itemDragDrop.droppedItemImagePath);

                Debug.Log(changeID + " after change: " + id);
                itemDragDrop.droppedItemID = changeID;
                itemDragDrop.droppedItemAmount = changeAmount;
                itemDragDrop.droppedItemImagePath = changeImagePath;

                itemDragDrop.afterSwapItems();
                slotItem.setParentData();
            }          
        }


    }
}
