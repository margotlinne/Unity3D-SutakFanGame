using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum StatType { initiative, strength, evasion, luck, bargain }

public class StatManager : MonoBehaviour
{
    GameManager gameManager;
    public Button[] increaseBtn;
    public Button[] decreaseBtn;
    public TextMeshProUGUI[] statsTxt;
    public TextMeshProUGUI availablePointTxt;

    void Start()
    {
        for (int i = 0; i < increaseBtn.Length; i++)
        {
            int index = i;
            increaseBtn[i].onClick.AddListener(() => OnIncreaseButtonClick(index));
            decreaseBtn[i].onClick.AddListener(() => OnDecreaseButtonClick(index));
        }


        gameManager = GameManager.instance;
        UpdateStats();
    }


    void Update()
    {
       // UpdateStats();
    }

    void OnIncreaseButtonClick(int index)
    {
        if (gameManager.dataManager.playerData.availablePoints > 0)
        {
            increaseStat((StatType)index);
            gameManager.dataManager.playerData.availablePoints--;
        }
    }

    void OnDecreaseButtonClick(int index)
    {
        decreaseStat((StatType)index);
    }

    void increaseStat(StatType type)
    {
        switch (type)
        {
            case StatType.initiative:
                gameManager.dataManager.playerData.stat_initiative++;
                break;
            case StatType.strength:
                gameManager.dataManager.playerData.stat_strength++;
                break;
            case StatType.evasion:
                gameManager.dataManager.playerData.stat_evasion++;
                break;
            case StatType.luck:
                gameManager.dataManager.playerData.stat_luck++;
                break;
            case StatType.bargain:
                gameManager.dataManager.playerData.stat_bargain++;
                break;
        }
        UpdateStats();
    }

    void decreaseStat(StatType type)
    {
        switch (type)
        {
            case StatType.initiative:
                if (gameManager.dataManager.playerData.stat_initiative > 0)
                {
                    gameManager.dataManager.playerData.stat_initiative--;
                    gameManager.dataManager.playerData.availablePoints++;
                }
                break;
            case StatType.strength:
                if (gameManager.dataManager.playerData.stat_strength > 0)
                {
                    gameManager.dataManager.playerData.stat_strength--;
                    gameManager.dataManager.playerData.availablePoints++;
                }
                break;
            case StatType.evasion:
                if (gameManager.dataManager.playerData.stat_evasion > 0)
                {
                    gameManager.dataManager.playerData.stat_evasion--;
                    gameManager.dataManager.playerData.availablePoints++;
                }
                break;
            case StatType.luck:
                if (gameManager.dataManager.playerData.stat_luck > 0)
                {
                    gameManager.dataManager.playerData.stat_luck--;
                    gameManager.dataManager.playerData.availablePoints++;
                }
                break;
            case StatType.bargain:
                if (gameManager.dataManager.playerData.stat_bargain > 0)
                {
                    gameManager.dataManager.playerData.stat_bargain--;
                    gameManager.dataManager.playerData.availablePoints++;
                }
                break;
        }
        UpdateStats();
    }

    void UpdateStats()
    {
        statsTxt[(int)StatType.initiative].text = gameManager.dataManager.playerData.stat_initiative.ToString();
        statsTxt[(int)StatType.strength].text = gameManager.dataManager.playerData.stat_strength.ToString();
        statsTxt[(int)StatType.evasion].text = gameManager.dataManager.playerData.stat_evasion.ToString();
        statsTxt[(int)StatType.luck].text = gameManager.dataManager.playerData.stat_luck.ToString();
        statsTxt[(int)StatType.bargain].text = gameManager.dataManager.playerData.stat_bargain.ToString();

        availablePointTxt.text = "사용 가능한 포인트 : " + gameManager.dataManager.playerData.availablePoints.ToString();
    }
}
