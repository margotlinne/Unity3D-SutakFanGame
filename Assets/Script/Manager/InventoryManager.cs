using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using static UnityEditor.Progress;

public enum equipSlotType { cape, boots, sword, bow }

public class InventoryManager : MonoBehaviour
{
    [HideInInspector]
    public InventorySlot clickedSlot;
    public InventorySlot[] inventorySlots;
    public InventorySlot[] equipSlots;

    public InventoryItem[] inventoryItems;
    public InventoryItem[] equipItems;

    private Dictionary<int, InventoryItem> itemDictionary;
    private Dictionary<int, InventoryItem> equipItemDictionary;

    public GameObject hoverWindow;
    public GameObject rightClickWindow;
    public GameObject itemControlWindow;
    public GameObject seperateBtn;
    public GameObject consumeBtn;
    public GameObject equipBtn;
    public GameObject craftBtn;
    public TextMeshProUGUI equipBtnTxt;
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

        foreach (var item in inventoryItems)
        {
            itemDictionary[item.id] = item;
        }

        equipItemDictionary = new Dictionary<int, InventoryItem>();

        foreach (var item in equipItems)
        {
            equipItemDictionary[item.id] = item;
        }

        gameManager = GameManager.instance;

        // 게임 시작 시 저장된 데이터에서 값 가져오기
        for (int i = 0; i < inventorySlots.Length; i++) 
        {
            inventorySlots[i].id = gameManager.dataManager.inventoryData.itemID[i];
            inventorySlots[i].itemImage.sprite = Resources.Load<Sprite>(gameManager.dataManager.inventoryData.imagePath[i]);
            inventorySlots[i].isEmpty = gameManager.dataManager.inventoryData.emptySlot[i];
            inventorySlots[i].isEquipSlot = false;
            inventorySlots[i].equipType = gameManager.dataManager.inventoryData.equipType[i];

            inventorySlots[i].amount = gameManager.dataManager.inventoryData.itemAmount[i];
            inventorySlots[i].equipType = gameManager.dataManager.inventoryData.equipType[i];
            inventorySlots[i].equipableInSlot = gameManager.dataManager.inventoryData.equipableInSlot[i];

            // 슬롯 마다 슬롯 고유의 아이디 제공
            inventorySlots[i].slotId = i;
        }

        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].id = gameManager.dataManager.equipData.itemID[i];
            equipSlots[i].itemImage.sprite = Resources.Load<Sprite>(gameManager.dataManager.equipData.imagePath[i]);
            equipSlots[i].isEmpty = gameManager.dataManager.equipData.emptySlot[i];
            equipSlots[i].isEquipSlot = true;

            // equiptype은 각 슬롯에 인스펙터에서 직접 할당

            equipSlots[i].amount = gameManager.dataManager.equipData.itemAmount[i];
            equipSlots[i].equipableInSlot = gameManager.dataManager.equipData.equipableInSlot[i];

            // 슬롯 마다 슬롯 고유의 아이디 제공
            equipSlots[i].slotId = inventorySlots.Length + i;
        }
    }


    public void setDataValues()
    {
        // 데이터 입력하기
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            gameManager.dataManager.inventoryData.itemID[i] = inventorySlots[i].id;

            string path = AssetDatabase.GetAssetPath(inventorySlots[i].itemImage.sprite);
            path = path.Replace("Assets/Resources/", "").Replace(".png", "");
            gameManager.dataManager.inventoryData.imagePath[i] = path;

            gameManager.dataManager.inventoryData.emptySlot[i] = inventorySlots[i].isEmpty;
            gameManager.dataManager.inventoryData.itemAmount[i] = inventorySlots[i].amount;

            gameManager.dataManager.inventoryData.equipType[i] = inventorySlots[i].equipType;
            gameManager.dataManager.inventoryData.equipableInSlot[i] = inventorySlots[i].equipableInSlot;
        }

        for (int i = 0; i < equipSlots.Length; i++)
        {
            gameManager.dataManager.equipData.itemID[i] = equipSlots[i].id;

            string path = AssetDatabase.GetAssetPath(equipSlots[i].itemImage.sprite);
            path = path.Replace("Assets/Resources/", "").Replace(".png", "");
            gameManager.dataManager.equipData.imagePath[i] = path;

            gameManager.dataManager.equipData.emptySlot[i] = equipSlots[i].isEmpty;
            gameManager.dataManager.equipData.itemAmount[i] = equipSlots[i].amount;

            gameManager.dataManager.equipData.equipableInSlot[i] = equipSlots[i].equipableInSlot;
        }
    }

    public bool AddItem(InventoryItem inventoryItem)
    {
        bool added = false;

        for (int i = 0; i < inventorySlots.Length; i++) 
        {
            InventorySlot slot = inventorySlots[i];
           // Debug.Log(i + "번째 순회 중 " + "획득한 아이템의 id: " + inventoryItem.id + " 확인중인 슬롯 내부의 아이템의 id: " + slot.id);

            // 슬롯에 같은 아이템을 발견했을 때        
            if (slot.id == inventoryItem.id)
            {
                Debug.Log("같은 아이템 확인");
                // 쌓을 수 있는 아이템일 때
                if (checkStackability(slot.id))
                {
                    Debug.Log("쌓을 수 있는 아이템으로, 쌓음");
                    slot.amount++;
                    slot.slotItem.setParentData();
                    added = true;
                    //setDataValues();
                    break;
                }
                // 쌓을 수 없는 아이템일 때
                else
                {
                    Debug.Log("쌓을 수 없음: " + checkStackability(slot.id));
                    added = IntoEmptySlot(inventoryItem);
                    break;
                }
            }
            // 모든 슬롯 검사 결과 같은 애가 없을 때 
            else if (i == inventorySlots.Length - 1)
            {
                added = IntoEmptySlot(inventoryItem);
                break;
            }
        }

        return added;
    }

    bool IntoEmptySlot(InventoryItem inventoryItem)
    {
        bool val = false;
        // 빈 슬롯에 추가
        for (int j = 0; j < inventorySlots.Length; j++)
        {
            InventorySlot slot = inventorySlots[j];
            if (slot.isEmpty)
            {
                slot.itemImage.sprite = inventoryItem.icon;
                slot.isEmpty = false;
                slot.id = inventoryItem.id;
                slot.amount = 1;
                slot.equipType = inventoryItem.equipType.ToString();
                val = true;
                //Debug.Log("경로: "  + AssetDatabase.GetAssetPath(slots[j].itemInSlot.sprite));
                //setDataValues();
                if (inventoryItem.equipable)
                {
                    Debug.Log("장비 아이템 획득");
                    slot.equipableInSlot = true;
                }
                else
                {
                    Debug.Log("기본 아이템 획득");
                    slot.equipableInSlot = false;
                }

                slot.slotItem.setParentData();
                break;
            }
            // 빈 슬롯이 없을 때
            else if (j == inventorySlots.Length - 1)
            {
                Debug.Log("inventory is full!");
                val = false;
                break;
            }
        }
        return val;
    }


    void Update()
    {
        // 호버 창 뜨고 해당 칸에서 마우스가 벗어나면 호버 창 없애는 거 어떻게 할지?

        // 해당 ui 창이 활성화되어 있을 때만 텍스트 업데이트
        if (itemControlWindow.activeSelf)
        {
            amountTxt.text = selectedAmount.ToString();
        }


    }

    

    public bool checkStackability(int id)
    {
        if(itemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            return foundItem.stackable;
        }
        else
        {
            return false;
        }
    }

    public string checkEquipType(int id)
    {
        if (equipItemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            return foundItem.equipType.ToString();
        }
        else
        {
            return "none";
        }
    }

    public void showHoverWindow(int id, int slotId, bool equipable)
    {
        hoverId = slotId;
        Vector2 screenPosition = Input.mousePosition;
        Vector2 pos = new Vector2(screenPosition.x + 200, screenPosition.y - 100);

        hoverWindow.transform.position = pos;

        // 기본 인벤토리 갯수에 이어서 장비란의 id 값이 정해졌으므로
        if (!equipable)
        {
            if (itemDictionary.TryGetValue(id, out InventoryItem foundItem))
            {
                hoverTitleTxt.text = foundItem.itemName;
                descriptionTxt.text = foundItem.description;
            }
        }
        else
        {
            if (equipItemDictionary.TryGetValue(id, out InventoryItem foundItem))
            {
                hoverTitleTxt.text = foundItem.itemName;
                descriptionTxt.text = foundItem.description;
            }
        }
        hoverWindow.SetActive(true);
        //Debug.Log("창 띄우기 함수 실행");
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

        InventoryItem foundItem;

        // 장비 아이템일 때
        if (clickedSlot.equipableInSlot)
        {
            // 장비는 소모가 불가능하므로 소모 버튼 비활성화
            consumeBtn.SetActive(false);            
            
            // 장비는 조합이 불가능하므로 조합 버튼 비활성화
            craftBtn.SetActive(false);
        }
        else
        {
            // 소모 가능 아이템인지 여부 확인 후 버튼 활/비활성화
            if (itemDictionary.TryGetValue(clickedSlot.id, out foundItem))
            {
                if (foundItem.consumable) { consumeBtn.SetActive(true); }
                else { consumeBtn.SetActive(false); }
            }

            // 조합 가능 아이템인지 여부 확인 후 버튼 활/비활성화
            if (itemDictionary.TryGetValue(clickedSlot.id, out foundItem))
            {
                if (foundItem.craftable) { craftBtn.SetActive(true); }
                else { craftBtn.SetActive(false); }
            }

        }

        // 장비 슬롯이라면
        if (clickedSlot.isEquipSlot)
        {
            // 장착 버튼 활성화 및 텍스트 조정
            equipBtn.SetActive(true);
            equipBtnTxt.text = "장착해제";
        }
        // 기본 인벤토리 슬롯에서 아이템의 종류에 따라 텍스트 변경
        else
        {
            if (clickedSlot.equipableInSlot)
            {
                if (clickedSlot.equipType == "cape")
                {
                    equipBtnTxt.text = "장착(망토)";
                }
                else if (clickedSlot.equipType == "boots")
                {
                    equipBtnTxt.text = "장착(부츠)";
                }
                else if (clickedSlot.equipType == "sword")
                {
                    equipBtnTxt.text = "장착(검)";
                }
                else if (clickedSlot.equipType == "bow")
                {
                    equipBtnTxt.text = "장착(활)";
                }
                equipBtn.SetActive(true);


                Debug.Log("장착 가능 아이템에 우클릭" + equipBtnTxt.text);
            }
            // 장비 아이템이 아닌 경우 장착 버튼 비활성화
            else
            {
                equipBtn.SetActive(false);
            }
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

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                // 비어 있는 슬롯에 추가
                if (slot.isEmpty)
                {
                    slot.id = clickedSlot.id;
                    slot.amount = num;
                    slot.itemImage.sprite = clickedSlot.itemImage.sprite;
                    slot.isEmpty = false;
                    slot.equipType = clickedSlot.equipType;

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
            for (int i = 0; i < inventorySlots.Length; i++) 
            {
                InventorySlot slot = inventorySlots[i].GetComponent<InventorySlot>();
                if (slot.isEmpty) { break; }
                else if (i == inventorySlots.Length - 1) { Debug.Log("invetory is full, can't seperate the items!"); }
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

    public void EquipBtn()
    {
        // 만약 클릭한 슬롯이 일반 인벤토리 슬롯이라면 장비란으로의 이동이므로
        if (!clickedSlot.isEquipSlot)
        {
            if (clickedSlot.equipType == "cape")
            {
                // 해당 장비란으로 (첫 번째 매개변수) 클릭된 슬롯의 장비 아이템(두 번째 매개변수)을 이동
                clickedSlot.IntoEquipSlot(equipSlots[(int)equipSlotType.cape], clickedSlot.slotItem);
            }
            else if (clickedSlot.equipType == "boots")
            {
                clickedSlot.IntoEquipSlot(equipSlots[(int)equipSlotType.boots], clickedSlot.slotItem);
            }
            else if (clickedSlot.equipType == "sword")
            {
                clickedSlot.IntoEquipSlot(equipSlots[(int)equipSlotType.sword], clickedSlot.slotItem);
            }
            else if (clickedSlot.equipType == "bow")
            {
                clickedSlot.IntoEquipSlot(equipSlots[(int)equipSlotType.bow], clickedSlot.slotItem);
            }
        }

        // 장비란에서 클릭이라면 장착 해제이므로
        else
        {
           for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].isEmpty)
                {
                    clickedSlot.IntoEmptySlot(inventorySlots[i], clickedSlot.slotItem);
                    break;
                }
            }
        }

        hideRightClickWindow();
        
    }

    public void CraftBtn()
    {

        hideRightClickWindow();
    }
}
