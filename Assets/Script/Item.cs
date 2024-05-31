using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameManager gameManager;
    public InventoryItem inventoryItem;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void OnMouseDown()
    {
        bool delete = false;
        delete = gameManager.inventoryManager.AddItem(inventoryItem);
        //if (delete) { Destroy(this.gameObject); } 
    }
}
