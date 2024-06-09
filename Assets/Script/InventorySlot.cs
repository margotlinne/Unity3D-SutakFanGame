using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{   
    Coroutine wait;
    GameManager gameManager;
    [HideInInspector] public Image slotBox;
    public Image itemImage;
    public TextMeshProUGUI amountTxt;
    public int amount;
    public string equipType = "";
    public bool isEmpty = true;
    public int id;
    private bool isHover = false;
    private bool rightClicking = false;
    public int slotId;
    public bool isEquipSlot = false;
    public bool equipableInSlot = false;
    public bool isCraftSlot = false;
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

        // 장비란이 아닌 그냥 인벤토리란에서만
        if (!isEquipSlot)
        {
            if (amount > 1 && !isEmpty)
            {
                amountTxt.text = amount.ToString();
            }
            else
            {
                amountTxt.text = "";
            }
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
                    Debug.Log("clicked  " + isEquipSlot);
                    rightClicking = true;
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

            if (gameManager.inventoryManager.isGrappingItem)
            {
                changeTransparency(0.5f);
            }
           
        }
        else if (!isHover)
        {
            changeTransparency(1f);
            HideHover();
        }     
        
        if (Input.GetMouseButtonUp(1))
        {
            rightClicking = false;
        }

     


    }

    void HideHover()
    {
        // 현재 떠 있는 호버의 슬롯 아이디가 내 아이디일 때 즉 나의 호버 창이 떠 있을 때
        if (gameManager.inventoryManager.hoverId == slotId)
        {
            gameManager.inventoryManager.closeHoverWindow();
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
        equipableInSlot = false;
        // 장비란은 장비 타입이 비어있어선 안 되므로(첨부터 고정)
        if (!isEquipSlot)
        {
            equipType = "";
        }
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
        gameManager.inventoryManager.showHoverWindow(id, slotId, equipableInSlot);

    }


    // 장비란에서 장착 해제나 조합란에서 빼기 버튼을 누를 때 인벤토리의 빈 칸으로 이동
    public void IntoEmptySlot(InventorySlot slot, ItemDragDrop item)
    {
        slot.id = item.droppedItemID;
        slot.amount = item.droppedItemAmount;
        slot.equipableInSlot = item.isEquipable;
        slot.itemImage.sprite = Resources.Load<Sprite>(item.droppedItemImagePath);
        slot.equipType = gameManager.inventoryManager.checkEquipType(item.droppedItemID).ToString();

        // 원래 아이템이 있던 슬롯 초기화
        item.parentObj.resetSlot();

        // 아이템을 받은 해당 슬롯의 아이템 업데이트            
        slot.slotItem.setParentData();
        slot.isEmpty = false;
    }



    /* 인벤토리에서 우클릭 후 장착 버튼을 눌러서 장착할 때, 혹은 드래그 앤 드롭으로 장착 아이템을 장착할 때
     * 매개변수로 받은 슬롯(장비 아이템이 이동할 장비란)과 아이템 정보 교환 */
    public void IntoEquipSlot(InventorySlot slot, ItemDragDrop item)
    {
        // 놓으려는 슬롯에 아이템이 없다면, 놓은 아이템이 장비면 장착
        if (slot.isEmpty && item.isEquipable)
        {
            Debug.Log("빈 슬롯에 아이템 드롭");
            slot.id = item.droppedItemID;
            slot.amount = item.droppedItemAmount;
            slot.equipableInSlot = item.isEquipable;
            slot.itemImage.sprite = Resources.Load<Sprite>(item.droppedItemImagePath);
            slot.equipType = gameManager.inventoryManager.checkEquipType(item.droppedItemID).ToString();

            // 원래 아이템이 있던 슬롯 초기화
            item.parentObj.resetSlot();

            // 아이템을 받은 해당 슬롯의 아이템 업데이트            
            slot.slotItem.setParentData();
            slot.isEmpty = false;
            Debug.Log("장비란의 아이템은 장착 가능?: " + slot.slotItem.isEquipable + "  이동한 아이템은 장착 가능?: " + item.isEquipable);

        }
        // 아이템이 있는 슬롯이고 본인 슬롯이 아닌 경우
        else if (slot.slotId != item.parentObj.slotId)
        {
            SwapSlotDatas(slot, item);
        }
    }

    // 드래그 앤 드롭으로 아이템이 스왑될 때
    public void SwapSlotDatas(InventorySlot slot, ItemDragDrop item)
    {
        Debug.Log("아이템 있는 슬롯에 아이템 드롭");
        int changeID = 0;
        int changeAmount = 0;
        string changeImagePath = "";
        bool changeIsEquipable;
        string changeEquipType;


        // 놓는 아이템과 놓으려는 곳의 아이템의 데이터를 교환 (예: a 망토 in 망토장비란, b 망토 in 인벤토리 -> b 망토 in 망토장비란, a 망토 in 인벤토리)
        changeID = slot.id;
        changeAmount = slot.amount;
        string path = AssetDatabase.GetAssetPath(slot.itemImage.sprite);
        changeImagePath = path.Replace("Assets/Resources/", "").Replace(".png", "");
        changeIsEquipable = slot.equipableInSlot;
        changeEquipType = slot.equipType;

        slot.id = item.droppedItemID;
        slot.amount = item.droppedItemAmount;
        slot.equipableInSlot = item.isEquipable;
        slot.itemImage.sprite = Resources.Load<Sprite>(item.droppedItemImagePath);
        slot.equipType = gameManager.inventoryManager.checkEquipType(item.droppedItemID).ToString();
        Debug.Log(slot.equipType);

        Debug.Log(changeID + " after change: " + slot.id);
        item.droppedItemID = changeID;
        item.droppedItemAmount = changeAmount;
        item.droppedItemImagePath = changeImagePath;
        item.isEquipable = changeIsEquipable;
        item.parentObj.equipType = changeEquipType;

        item.afterSwapItems();
        slot.slotItem.setParentData();
    }

    // 아이템이 해당 슬롯에 드롭될 때
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        ItemDragDrop itemDragDrop = droppedItem.GetComponent<ItemDragDrop>();

        if (itemDragDrop != null)
        {
            // 장비란이 아닌 경우 아이템끼리 교환 및 (쌓을 수 있는 아이템에 한해서) 쌓기
            if (!isEquipSlot)
            {
                // 놓으려는 슬롯에 아이템이 없다면
                if (isEmpty)
                {
                    Debug.Log("빈 슬롯에 아이템 드롭");
                    id = itemDragDrop.droppedItemID;
                    amount = itemDragDrop.droppedItemAmount;
                    itemImage.sprite = Resources.Load<Sprite>(itemDragDrop.droppedItemImagePath);
                    equipableInSlot = itemDragDrop.isEquipable;
                    equipType = itemDragDrop.parentObj.equipType;
                    Debug.Log(equipableInSlot);

                    // 원래 아이템이 있던 슬롯 초기화
                    itemDragDrop.parentObj.resetSlot();

                    // 아이템을 받은 해당 슬롯의 아이템 업데이트            
                    slotItem.setParentData();
                    isEmpty = false;


                    Debug.Log("id: " + id + " amount: " + amount);

                }
                /* 아이템이 있는 슬롯이고 본인 슬롯이 아닌 경우,
                 * 그리고 장비란에서 아이템을 드래그해 인벤토리 아이템과 스왑할 시 노말 아이템이 장비란에 장착되는 걸 방지 */
                else if (slotId != itemDragDrop.parentObj.slotId && !itemDragDrop.parentObj.isEquipSlot)
                {

                    // 놓으려는 슬롯의 아이템과 놓는 아이템의 종류가 같다면, 그리고 쌓을 수 있는 아이템이라면
                    if (id == itemDragDrop.droppedItemID && gameManager.inventoryManager.checkStackability(id))
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
                        SwapSlotDatas(this, itemDragDrop);
                    }
                }

            }
            // 장비 란에 아이템을 놓은 경우 
            else
            {
                // 해당 아이템이 장비 아이템이어야 함
                if (itemDragDrop.isEquipable)
                {
                    // 이 장비 란의 종류와 놓으려는 아이템의 장비 종류가 같아야 함. (예: 망토 장비란, 부츠 아이템 -> 장착 못함)
                    if (equipType == gameManager.inventoryManager.checkEquipType(itemDragDrop.droppedItemID).ToString())
                    {
                        Debug.Log("same type");

                        // 이 슬롯으로, 드롭된 아이템을 이동
                        IntoEquipSlot(this, itemDragDrop);
                    }
                }
                else { Debug.Log("장비 가능 아이템이 아님"); }
            }
        }       
    }
}
