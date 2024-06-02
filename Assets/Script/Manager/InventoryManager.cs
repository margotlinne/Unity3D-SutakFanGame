using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector]
    public Slot clickedSlot;
    public Slot[] slots;

    public InventoryItem[] items;

    private Dictionary<int, InventoryItem> itemDictionary;

    public GameObject hoverWindow;
    public GameObject rightClickWindow;
    public GameObject itemControlWindow;
    public GameObject seperateBtn;
    public GameObject consumeBtn;
    public TextMeshProUGUI hoverTitleTxt;
    public TextMeshProUGUI descriptionTxt;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI amountTxt;

    private int selectedAmount = 0;
    private bool buttonClick = false;
    public bool windowOn = false;
    public bool grappedItem = false;



    [HideInInspector]
    public int hoverId = -1;

    GameManager gameManager;
    
    void Start()
    {
        hoverWindow.SetActive(false);
        rightClickWindow.SetActive(false);
        itemControlWindow.SetActive(false);

        itemDictionary = new Dictionary<int, InventoryItem>();

        foreach (var item in items)
        {
            itemDictionary[item.id] = item;
        }

        gameManager = GameManager.instance;

        // 게임 시작 시 저장된 데이터에서 값 가져오기
        for (int i = 0; i < slots.Length; i++) 
        {
            slots[i].id = gameManager.dataManager.inventoryData.itemID[i];
            slots[i].itemImage.sprite = Resources.Load<Sprite>(gameManager.dataManager.inventoryData.imagePath[i]);
            slots[i].isEmpty = gameManager.dataManager.inventoryData.emptySlot[i];
            slots[i].amount = gameManager.dataManager.inventoryData.itemAmount[i];

            // 슬롯 마다 슬롯 고유의 아이디 제공
            slots[i].slotId = i;
        }
    }

    public void setDataValues()
    {
        // 데이터 입력하기
        for (int i = 0; i < slots.Length; i++)
        {
            gameManager.dataManager.inventoryData.itemID[i] = slots[i].id;

            string path = AssetDatabase.GetAssetPath(slots[i].itemImage.sprite);
            path = path.Replace("Assets/Resources/", "").Replace(".png", "");
            gameManager.dataManager.inventoryData.imagePath[i] = path;

            gameManager.dataManager.inventoryData.emptySlot[i] = slots[i].isEmpty;
            gameManager.dataManager.inventoryData.itemAmount[i] = slots[i].amount;
        }
    }

    public bool AddItem(InventoryItem inventoryItem)
    {
        bool added = false;

        for (int i =0; i < slots.Length; i++) 
        {
            Slot slot = slots[i].GetComponent<Slot>();
            // 슬롯에 같은 아이템을 발견했을 때
            if (slot.id == inventoryItem.id)
            {
                slot.amount++;
                slot.slotItem.setParentData();
                added = true;
                //setDataValues();
                return added;
            }
            // 모든 슬롯 검사 결과 같은 애가 없을 때
            else if (i == slots.Length - 1)
            {
                // 빈 슬롯에 추가
                for(int j = 0; j < slots.Length; j++)
                {
                    Slot _slot = slots[j].GetComponent<Slot>();
                    if (_slot.isEmpty)
                    {
                        _slot.itemImage.sprite = inventoryItem.icon;
                        _slot.isEmpty = false;
                        _slot.id = inventoryItem.id;
                        _slot.amount = 1;
                        added = true;
                        //Debug.Log("경로: "  + AssetDatabase.GetAssetPath(slots[j].itemInSlot.sprite));
                        //setDataValues();
                        _slot.slotItem.setParentData();
                        return added;
                    }
                    // 빈 슬롯이 없을 때
                    else if (j == slots.Length - 1)
                    {
                        Debug.Log("inventory is full!");
                        added = false;
                        return added;
                    }
                }
            }            
        }

        return added;
    }


    void Update()
    {
        //if (!buttonClick && rightClickWindow.activeSelf)
        //{
        //    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        //    {
        //        rightClickWindow.SetActive(false);
        //    }
        //}

        // 해당 ui 창이 활성화되어 있을 때만 텍스트 업데이트
        if (itemControlWindow.activeSelf)
        {
            amountTxt.text = selectedAmount.ToString();
        }



    }

    public void showHoverWindow(int id, int slotId)
    {
        hoverId = slotId;
        //Debug.Log("창 띄우기 함수 실행");
        Vector2 screenPosition = Input.mousePosition;
        Vector2 pos = new Vector2(screenPosition.x + 200, screenPosition.y - 100);

        hoverWindow.transform.position = pos;

        if (itemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            hoverTitleTxt.text = foundItem.itemName;
            descriptionTxt.text = foundItem.description;
        }
        hoverWindow.SetActive(true);
        // Debug.Log(hoverId);
    }

    public void hideHoverWindow()
    {
        hoverWindow.SetActive(false);
        hoverId = -1;
    }


    public void setRightClickWindow()
    {
        gameManager.uiManager.activeUI.Add(rightClickWindow);
        buttonClick = false;
        windowOn = true;

        rightClickWindow.SetActive(true);
        Vector2 screenPosition = Input.mousePosition;
        Vector2 pos = new Vector2(screenPosition.x + 70, screenPosition.y - 70);

        rightClickWindow.transform.position = pos;

        // 아이템 갯수가 1개라면 나누기 버튼은 미표시
        if (clickedSlot.amount == 1){ seperateBtn.SetActive(false); }
        else { seperateBtn.SetActive(true); }


        // 소비 가능 아이템이 아니라면 소모 버튼은 미표시
        if (itemDictionary.TryGetValue(clickedSlot.id, out InventoryItem foundItem))
        {
            if(foundItem.consumable) { consumeBtn.SetActive(true); }
            else { consumeBtn.SetActive(false); }
        }
    }
    
    void hideRightClickWindow()
    {
        rightClickWindow.SetActive(false);
        gameManager.uiManager.activeUI.Remove(rightClickWindow);
        windowOn = false;
    }


    void setItemControlWindow(string str)
    {
        windowOn = true;
        selectedAmount = 0;
        titleTxt.text = str;
        itemControlWindow.SetActive(true);
        gameManager.uiManager.activeUI.Add(itemControlWindow);
    }

    void hideItemControlWindow()
    {
        itemControlWindow.SetActive(false);
        gameManager.uiManager.activeUI.Remove(itemControlWindow);
        windowOn = false;

    }

    void SeperateItems(int num)
    {
        if (num > 0)
        {
            clickedSlot.amount -= num;

            for (int i = 0; i < slots.Length; i++)
            {
                Slot slot = slots[i].GetComponent<Slot>();
                if (slot.isEmpty)
                {
                    slot.id = clickedSlot.id;
                    slot.amount = num;
                    slot.itemImage.sprite = clickedSlot.itemImage.sprite;
                    slot.isEmpty = false;

                    // 나눠진 슬롯과 새로 나뉘어서 아이템이 추가된 슬롯의 아이템 데이터를 부모 슬롯 값으로 설정
                    slot.slotItem.setParentData();
                    clickedSlot.slotItem.setParentData();
                    //setDataValues();
                    break;
                }
            }
        }        

    }

    public void increaseAmountBtn()
    {
        if (titleTxt.text == "나누기")
        {
            if (selectedAmount < clickedSlot.amount - 1)
            {
                selectedAmount++;
            }
        }
        else
        {
            if (selectedAmount < clickedSlot.amount)
            {
                selectedAmount++;
            }
        }
        
    }

    public void decreaseAmountBtn()
    {
        if (selectedAmount > 0)
        {
            selectedAmount--;
        }
    }

    public void cancelBtn()
    {
        hideItemControlWindow();
    }




    public void confirmBtn()
    {
        if (titleTxt.text == "버리기")
        {
            clickedSlot.amount -= selectedAmount;
            if( clickedSlot.amount == 0) { clickedSlot.resetSlot(); }
            // 버리고 나서 amount 값을 업데이트하기 위해 슬롯의 아이템 값을 부모 슬롯의 값으로 설정
            clickedSlot.slotItem.setParentData();
        }
        else if (titleTxt.text == "나누기")
        {
            SeperateItems(selectedAmount);
        }

        //setDataValues();
        hideItemControlWindow();
    }

    public void DiscardBtn()
    {        
        buttonClick = true;

        // 1개보다 많을 경우 몇 개를 액션을 취해 줄것인지 결정
        if (clickedSlot.amount > 1)
        {
            setItemControlWindow("버리기");
        }
        else
        {
            clickedSlot.resetSlot();
            windowOn = false;
        }

        hideRightClickWindow();

    }


    public void SeperateBtn()
    {
        buttonClick = true;

        // 1개보다 많을 경우 몇 개를 액션을 취해 줄것인지 결정
        if (clickedSlot.amount > 1)
        {
            for (int i = 0; i < slots.Length; i++) 
            {
                Slot slot = slots[i].GetComponent<Slot>();
                if (slot.isEmpty) { break; }
                else if (i == slots.Length - 1) { Debug.Log("invetory is full, can't seperate the items!"); }
            }
            setItemControlWindow("나누기");
        }

        hideRightClickWindow();
    }

    public void ConsumeBtn()
    {
        buttonClick = true;

        clickedSlot.amount--;
        if (clickedSlot.amount == 0) { clickedSlot.resetSlot(); }
        Debug.Log("drank potion");

        hideRightClickWindow();            
        // 소모하고 나서 amount 값을 업데이트하기 위해 슬롯의 아이템 값을 부모 슬롯의 값으로 설정
        clickedSlot.slotItem.setParentData();
    }
}
