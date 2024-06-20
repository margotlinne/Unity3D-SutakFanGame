using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] units;

    public EquipManager equipManager;
    public InventoryManager inventoryManager;
    public UIManager uiManager;
    public BattleManager battleManager;
    public ConvoData convoData;
    public DataManager dataManager;

    public bool firstConvoDone = false;
    public bool getReward = false;
    public bool acceptedQuest = false;

    public GameObject cameraTarget;

    void Awake()
    {
        #region singleton
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
        #endregion
    }



}
