using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image image;
    public InventorySlot parentObj;
    
    GameManager gameManager;
    [HideInInspector] public bool isEquipable;
    [HideInInspector] public bool isDragging;
    [HideInInspector] public int droppedItemID;
    [HideInInspector] public int droppedItemAmount;
    [HideInInspector] public string droppedItemImagePath;

    public Color orgColor;
    //[HideInInspector] public bool droppedItemEmpty;
    

    void Start()
    {
        setParentData();
        orgColor = parentObj.GetComponent<Image>().color;
        gameManager = GameManager.instance;
    }

    public void afterSwapItems()
    {
        parentObj.id = droppedItemID;
        parentObj.amount = droppedItemAmount;
        parentObj.itemImage.sprite = Resources.Load<Sprite>(droppedItemImagePath);
        parentObj.equipableInSlot = isEquipable;

        //parentObj.isEmpty = droppedItemEmpty;
    }

    public void setParentData()
    {
        droppedItemID = parentObj.id;
        droppedItemAmount = parentObj.amount;
        string path = AssetDatabase.GetAssetPath(parentObj.itemImage.sprite);
        droppedItemImagePath = path.Replace("Assets/Resources/", "").Replace(".png", "");
        isEquipable = parentObj.equipableInSlot;

        //droppedItemEmpty = parentObj.isEmpty;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameManager.inventoryManager.draggedItem = this;
        gameManager.inventoryManager.isGrappingItem = true;
        parentObj.GetComponent<Image>().color = Color.red;
        //Debug.Log(parentObj.amount);

        transform.SetParent(transform.root);
        isDragging = true;

        // 아이템 드래그를 놓을 때 슬롯에 놓았는지 확인하기 위해 레이캐스트를 꺼두고 슬롯에 레이캐스트가 닿도록 
        image.raycastTarget = false;

        Debug.Log(isEquipable);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameManager.inventoryManager.draggedItem = null;
        gameManager.inventoryManager.isGrappingItem = false;
        parentObj.GetComponent<Image>().color = orgColor;

        transform.SetParent(parentObj.gameObject.transform);
        isDragging = false;

        // 아이템 드래그를 놓으면 레이캐스트를 켜서 다시 집을 수 있도록
        image.raycastTarget = true;
    }
}
