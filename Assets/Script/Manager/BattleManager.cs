using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool toMove = false;
    public bool clickedToMove = false;
    public bool inBattle = false;

    public GameObject battleCanvas;

    void Update()
    {
        battleCanvas.SetActive(inBattle);
    }

    public void moveBtn()
    {
        toMove = true;
        Debug.Log("clicked move button");
    }


}
