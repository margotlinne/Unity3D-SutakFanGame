using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using UnityEngine.ProBuilder.MeshOperations;

public enum equipSlotType { cape, boots, sword, bow }

public class InventoryManager : MonoBehaviour
{
    [HideInInspector]
    public InventorySlot clickedSlot;
    public InventorySlot[] inventorySlots;
    public InventorySlot[] equipSlots;
    public InventorySlot[] craftSlots;

    public InventoryItem[] inventoryItems;
    public InventoryItem[] equipItems;
    public InventoryItem[] craftItems;
    public InventoryItem[] allItems;

    private Dictionary<int, InventoryItem> invenItemDictionary;
    private Dictionary<int, InventoryItem> equipItemDictionary;
    private Dictionary<CraftType, InventoryItem> craftItemDictionary;
    private Dictionary<int, InventoryItem> allItemDictionary;

    public GameObject hoverWindow;
    public GameObject rightClickWindow;
    public GameObject itemControlWindow;
    public GameObject seperateBtn;
    public GameObject consumeBtn;
    public GameObject equipBtn;
    public GameObject craftBtn;
    public TextMeshProUGUI craftBtnTxt;
    public TextMeshProUGUI equipBtnTxt;
    public TextMeshProUGUI hoverTitleTxt;
    public TextMeshProUGUI descriptionTxt;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI amountTxt;

    private int selectedAmount = 0;
    private bool buttonClick = false;
    public bool windowOn = false;
    public bool grappedItem = false;
    private int availableCraftAmount = 0;
    private int totalCraftAmount = 0;
    private int craftItemID = 0;


    [HideInInspector]
    public int hoverId = -1;

    GameManager gameManager;
    
    void Start()
    {
        hoverWindow.SetActive(false);
        rightClickWindow.SetActive(false);
        itemControlWindow.SetActive(false);

        allItemDictionary = new Dictionary<int, InventoryItem>();

        foreach (var item in allItems)
        {
            allItemDictionary[item.id] = item;
        }

        invenItemDictionary = new Dictionary<int, InventoryItem>();

        foreach (var item in inventoryItems)
        {
            invenItemDictionary[item.id] = item;
        }

        equipItemDictionary = new Dictionary<int, InventoryItem>();

        foreach (var item in equipItems)
        {
            equipItemDictionary[item.id] = item;
        }

        craftItemDictionary = new Dictionary<CraftType, InventoryItem>();

        foreach (var item in craftItems)
        {
            craftItemDictionary[item.craftType] = item;
        }

        gameManager = GameManager.instance;

        // 게임 시작 시 저장된 데이터에서 값 가져오기
        for (int i = 0; i < inventorySlots.Length; i++) 
        {
            inventorySlots[i].id = gameManager.dataManager.inventoryData.itemID[i];
            inventorySlots[i].itemImage.sprite = Resources.Load<Sprite>(gameManager.dataManager.inventoryData.imagePath[i]);
            inventorySlots[i].isEmpty = gameManager.dataManager.inventoryData.emptySlot[i];
            inventorySlots[i].isEquipSlot = false;
            inventorySlots[i].isCraftSlot = false;
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
            equipSlots[i].isCraftSlot = false;

            // equiptype은 각 슬롯에 인스펙터에서 직접 할당

            equipSlots[i].amount = gameManager.dataManager.equipData.itemAmount[i];
            equipSlots[i].equipableInSlot = gameManager.dataManager.equipData.equipableInSlot[i];

            // 슬롯 마다 슬롯 고유의 아이디 제공
            equipSlots[i].slotId = inventorySlots.Length + i;
        }

        // 조합 슬롯은 게임을 끝내도 해당 란에 저장되어 있을 필요가 없으므로 데이터 저장은 안 함
        for (int i = 0; i < craftSlots.Length; i++) 
        {
            craftSlots[i].resetSlot();
            craftSlots[i].isEquipSlot = false;
            craftSlots[i].isCraftSlot = true;

            // 슬롯 마다 슬롯 고유의 아이디 제공
            craftSlots[i].slotId = (inventorySlots.Length + equipSlots.Length) + i;
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

    // 주운 아이템을 새로운 빈 슬롯에 추가하는 메서드. 아이템을 드래그앤 드롭으로 빈 슬롯에 놓는 것과 다른 개념
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

        // 인벤토리 창이 사라졌을 때, 조합란에 있던 애들을 다시 인벤토리란으로 이동
        if (!gameManager.uiManager.inventoryCanvas.activeSelf)
        {
            for (int i = 0; i < craftSlots.Length; i++)
            {
                // 조합란이 비어 있지 않으면 
                if (!craftSlots[i].isEmpty)
                {
                    Debug.Log("조합란에서 아이템란으로");
                    CraftslotToInventory(craftSlots[i]);
                }
            }
        }

    }

    public void CraftslotToInventory(InventorySlot slot)
    {
        // 조합란의 아이템이 쌓기 가능한 아이템일 때
        if (checkStackability(slot.id))
        {
            //Debug.Log("쌓기 가능한 아이템이 조합란에서 인벤토리란으로");
            for (int j = 0; j < inventorySlots.Length; j++)
            {
                // 인벤토리란 내에 같은 아이템이 있다면 쌓기
                if (inventorySlots[j].id == slot.id)
                {
                    // Debug.Log("같은 아이템 찾음-----------");
                    inventorySlots[j].amount++;
                    inventorySlots[j].slotItem.setParentData();
                    slot.resetSlot();
                    break;
                }
                // 마지막 슬롯까지 같은 아이템을 못 찾았다면
                else if (j == inventorySlots.Length - 1)
                {
                    // Debug.Log("같은 아이템 못 찾음**********");
                    for (int p = 0; p < inventorySlots.Length; p++)
                    {
                        // 비어 있는 슬롯을 찾아서 해당 조합란의 아이템 이동 
                        if (inventorySlots[p].isEmpty)
                        {
                            slot.IntoEmptySlot(inventorySlots[p], slot.slotItem);
                            break;
                        }
                        // 비어 있는 슬롯이 없는 경우는 생기지 않도록 빈 슬롯이 하나 뿐일 때 나누기 버튼을 못하게 막을 것임                                    
                    }
                    break;
                }
            }
        }
        // 쌓기 불가능한 아이템일 땐 빈 슬롯으로
        else
        {
            for (int j = 0; j < inventorySlots.Length; j++)
            {
                // 비어 있는 슬롯을 찾아서 해당 조합란의 아이템 이동 
                if (inventorySlots[j].isEmpty)
                {
                    slot.IntoEmptySlot(inventorySlots[j], slot.slotItem);
                    break;
                }
                // 비어 있는 슬롯이 없는 경우는 생기지 않도록 빈 슬롯이 하나 뿐일 때 나누기 버튼을 못하게 막을 것임                                    
            }
        }
    }

    public InventoryItem returnInventoryItem(int id)
    {
        InventoryItem item = null;
        if (invenItemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            item = foundItem;
        }
        return item;
    }
    
    // 조합 결과 아이템의 아이디 변환
    public int checkCraftID(CraftType type)
    {
        int returnVal = 0;
        if (craftItemDictionary.TryGetValue(type, out InventoryItem foundItem))
        {
            returnVal = foundItem.id;
            
        }

        return returnVal;
    }


    // 해당 조합 가능 아이템의 조합 결과 타입 반환
    public CraftType checkCraftType(int id)
    {
        CraftType returnVal = CraftType.None;
        if (invenItemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            returnVal = foundItem.craftType;
        }

        return returnVal;
    }

    // 해당 조합 결과물 아이템 타입의 조합 재료 갯수 반환
    public int checkCraftAmount(CraftType type)
    {
        int returnVal = 0;
        if (craftItemDictionary.TryGetValue(type, out InventoryItem foundItem))
        {
            if (foundItem.craftType == type)
            {
                returnVal = foundItem.numForCraft;
            }
        }

        return returnVal;
    }
    

    public bool checkStackability(int id)
    {
        bool returnVal = false;
        if(invenItemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            returnVal = foundItem.stackable;
        }

        return returnVal;
    }

    public bool checkCraftability(int id)
    {
        bool returnVal = false;
        if (allItemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            returnVal = foundItem.craftable;
        }

        return returnVal;
    }

    public string checkEquipType(int id)
    {
        string returnVal = "none";
        if (equipItemDictionary.TryGetValue(id, out InventoryItem foundItem))
        {
            returnVal = foundItem.equipType.ToString();
        }

        return returnVal;
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
            if (invenItemDictionary.TryGetValue(id, out InventoryItem foundItem))
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
        }
        else
        {
            // 소모 가능 아이템인지 여부 확인 후 버튼 활/비활성화
            if (invenItemDictionary.TryGetValue(clickedSlot.id, out foundItem))
            {
                if (foundItem.consumable) { consumeBtn.SetActive(true); }
                else { consumeBtn.SetActive(false); }
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

        // 조합 슬롯이라면
        if (clickedSlot.isCraftSlot)
        {
            craftBtnTxt.text = "빼기";
        }
        else
        {
            craftBtnTxt.text = "조합";
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
        else if (titleTxt.text == "버리기")
        {
            if (selectedAmount < clickedSlot.amount)
            {
                selectedAmount++;
            }
        }
        else if (titleTxt.text == "선택")
        {
            if (selectedAmount < availableCraftAmount)
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
        else if (titleTxt.text == "선택")
        {
            totalCraftAmount = selectedAmount;
            craftingItems();          
        }

        //setDataValues();
        hideItemControlWindow();
    }

    public void craftConfirmBtn()
    {
        bool isCraftable = false;
        bool checkFull = true;
        int count = 0;
        CraftType targetType = CraftType.None;
        int targetAmount = 0;
        int amount = 0;
        List<InventorySlot> slot = new List<InventorySlot>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].isEmpty)
            {
                checkFull = false;
            }
        }

        // 인벤토리가 가득 차지 않았을 때 조합 가능
        if (!checkFull)
        {
            for (int i = 0; i < craftSlots.Length; i++)
            {
                if (!craftSlots[i].isEmpty)
                {
                    count++;
                    slot.Add(craftSlots[i]);
                }
            }

            // 조합란에 아이템 2개 이상 있을 때
            if (count > 1)
            {
                for (int i = 0; i < slot.Count; i++)
                {
                    // 조합란에 있는 아이템은 애초에 조합 가능한, craftable 아이템이어야 함
                    if (checkCraftability(slot[i].id))
                    {
                        isCraftable = true;
                        /* 첫 번째 비어 있지 않은 조합란의 CraftType을 타겟으로 지정, 
                        * 해당 아이템에 필요한 재료의 갯수 가져옴 */
                        if (i == 0)
                        {
                            targetType = checkCraftType(slot[i].id);
                            craftItemID = checkCraftID(targetType);
                            targetAmount = checkCraftAmount(targetType);
                            Debug.Log(targetType.ToString());
                            amount++;
                            Debug.Log("필요한 재료 개수: " + targetAmount + "조합란 차 있는 수: " + count);
                            // 애초에 예를 들어 3개 재료가 필요한데 count가 2라면 부족한 것이므로 조합 불가능
                            if (count != targetAmount)
                            {
                                Debug.Log("ingredients are wrong, can't craft");
                            }
                        }
                        else
                        {
                            // 나머지 조합란의 아이템에 같은 타입의 아이템이 있다면
                            if (checkCraftType(slot[i].id) == targetType)
                            {
                                amount++;
                            }
                            // 아니라면 타겟 아이템에 필요한 재료가 아니므로 조합 불가능
                            else
                            {
                                Debug.Log("ingredients are wrong, can't craft");
                            }
                        }
                    }

                    // 조합 불가능한 아이템이라면
                    else
                    {
                        Debug.Log("the item is not ingredient");
                        isCraftable = false;
                        break;
                    }
                    
                }

                // 조합 가능
                if (amount == targetAmount && isCraftable)
                {
                    bool overTwo = false;

                    // 조합란의 아이템이 두 개 이상일 경우에 몇 개 제작하고 싶은지 선택하도록
                    for (int i = 0; i < slot.Count; i++)
                    {
                        // 만약 조합란의 아이템이 2개 이상이라면 
                        if (slot[i].amount > 1)
                        {     
                            // 가장 작은 값으로 설정
                            if (availableCraftAmount == 0)
                            {
                                availableCraftAmount = slot[i].amount;
                            }
                            else if (slot[i].amount < availableCraftAmount)
                            {
                                availableCraftAmount = slot[i].amount;
                            }
                            // 모든 조합란의 아이템이 두 개 이상이라면
                            if (i == slot.Count -1)
                            {
                                Debug.Log(availableCraftAmount);
                                overTwo = true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    // 조합란의 아이템이 두 개 이상이 아니라면 아이템이 1개 있던 조합란의 경우 슬롯 비우기
                    if (!overTwo)
                    {
                        totalCraftAmount = 1;
                        craftingItems();
                    }
                    // 조합란의 아이템이 두 개 이상이라면 몇 개 조합할 것인지 선택하도록
                    else
                    {
                        // 선택 창에서 갯수 선택을 하고 나면 craftingItems 메서드에서 조합된 아이템 인벤토리에 추가 작업
                        setItemControlWindow("선택");
                    }                                   
                }
            }
        }
        else { Debug.Log("inventory is full, can't craft"); }         
    }

    // totalCraftAmount 값만큼 조합템을 만들고 조합란 비우기
    void craftingItems()
    {
        // 조합란 비우기, 수 조정하기
        for (int i = 0; i < craftSlots.Length; ++i)
        {
            // 조합란의 아이템 갯수가 조합 하려는 아이템 갯수와 같으면 조합란 비우기
            if (craftSlots[i].amount - totalCraftAmount == 0)
            {
                craftSlots[i].resetSlot();
            }
            else if (craftSlots[i].amount > 1)
            {
                craftSlots[i].amount -= totalCraftAmount;
                craftSlots[i].slotItem.setParentData();
            }
        }
        // 조합된 아이템 추가하기
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].isEmpty)
            {
                for (int j = 0; j < totalCraftAmount; j++)
                {
                    AddItem(returnInventoryItem(craftItemID));
                }
                // 조합 후엔 해당 값 0으로 초기화
                availableCraftAmount = 0;
                totalCraftAmount = 0;
                break;
            }
        }
    }


    #region 우클릭 창 버튼들
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
        int countInventorySlot = 0;
        bool emptyCraftSlot = true;
        /* 만약 비어있는 슬롯이 1칸 밖에 없으면서 조합란이 모두 비어 있지 않으면 
         * 나누기를 해서 빈 인벤토리란이 없을 때 인벤토리를 닫아 조합란의 아이템이 갈 곳을 잃는 경우를 막기 위해 확인*/
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (!inventorySlots[i].isEmpty)
            {
                countInventorySlot++;
            }
        }

        for (int i = 0; i < craftSlots.Length; i++)
        {
            if (!craftSlots[i].isEmpty)
            {
                emptyCraftSlot = false;
                break;
            }
        }

        // 비어 있는 슬롯이 1개 밖에 없고 조합란이 비어 있지 않았다면
        if (countInventorySlot == inventorySlots.Length - 1 && !emptyCraftSlot)
        {
            Debug.Log("empty craft slot first to seperate items, your inventory is almost full");
        }
        // 위의 경우가 아닌 보통의 경우
        else
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
        // 조합란에서 우클릭해서 빼기 버튼을 눌렀다면 해당 아이템을 다시 인벤토리함으로 옮겨야 함
        if (clickedSlot.isCraftSlot)
        {
            CraftslotToInventory(clickedSlot);
        }
        else
        {
            // 조합란에 아이템을 넣어야 하므로 조합 컨테이너를 띄움
            gameManager.uiManager.craftBtn();
            for (int i = 0; i < craftSlots.Length; i++)
            {
                if (craftSlots[i].isEmpty)
                {
                    clickedSlot.IntoEmptySlot(craftSlots[i], clickedSlot.slotItem);
                    break;
                }
                else if (i == craftSlots.Length - 1)
                {
                    Debug.Log("carft slot is full!");
                }
            }
        }

        hideRightClickWindow();
    }
    #endregion
}
