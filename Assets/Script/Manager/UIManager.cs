using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum ContainerType { equip, stat, craft }


// Äµ¹ö½º ¶ç¿ì±â, ¶ç¿î Äµ¹ö½º esc·Î ¼ø¼­´ë·Î ¼û±â±â, ÀÎº¥Åä¸® ÇÁ¸®ºä/½ºÅÝ/Á¶ÇÕ ¶ç¿ì°í ¼û±â±â ´ã´ç
public class UIManager : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public GameObject mapCanvas;


    public GameObject[] containers;

    public bool isCanvasOn = false;
    public int currentlyOpenUI;


    public List<GameObject> activeUI = new List<GameObject>();


    void Start()
    {
        inventoryCanvas.SetActive(false);        
    }

    void Update()
    {        
        if (activeUI.Count > 0)
        {
            currentlyOpenUI = activeUI.Count - 1;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                activeUI[currentlyOpenUI].SetActive(false);
                activeUI.Remove(activeUI[currentlyOpenUI]);
            }
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryCanvas.activeSelf && !isCanvasOn)
            {
                inventoryCanvas.SetActive(true);
                activeUI.Add(inventoryCanvas);
            }
            else
            {
                inventoryCanvas.SetActive(false);
                activeUI.Remove(inventoryCanvas);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!mapCanvas.activeSelf && !isCanvasOn)
            {
                mapCanvas.SetActive(true);
                activeUI.Add(mapCanvas);
            }
            else
            {
                mapCanvas.SetActive(false);
                activeUI.Remove(mapCanvas);
            }
        }


        if (inventoryCanvas.activeSelf || mapCanvas.activeSelf)
        {
            isCanvasOn = true;
        }
        else { isCanvasOn = false; }
    }








    public void equipBtn() 
    {
        setContainer(ContainerType.equip);
    }

    public void statBtn()
    {
        setContainer(ContainerType.stat);
    }

    public void craftBtn()
    {
        setContainer(ContainerType.craft);
    }

    void setContainer(ContainerType type)
    {
        for (int i = 0; i < containers.Length; i++)
        {
            if (i == (int)type)
            {
                containers[i].gameObject.SetActive(true);
            }
            else
            {
                containers[i].gameObject.SetActive(false);
            }
        }
    }

}
