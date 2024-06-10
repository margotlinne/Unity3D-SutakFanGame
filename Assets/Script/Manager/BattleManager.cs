using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitData
{
    int Initiative { get; }
    Sprite Portrait { get; }
}

public class BattleManager : MonoBehaviour
{
    public bool toMove = false;
    public bool clickedToMove = false;
    public bool inBattle = false;

    public List<GameObject> units = new List<GameObject>();

    public GameObject battleCanvas;

    void Update()
    {
        battleCanvas.SetActive(inBattle);

        if (!inBattle)
        {
            units.Clear();
        }
        else
        {
            if (units.Count > 0)
            {
                // 주도권 값을 기준으로 오름차순으로 정렬
                units.Sort((x, y) =>
                {
                    IUnitData xInitiative = x.GetComponent<IUnitData>();
                    IUnitData yInitiative = y.GetComponent<IUnitData>();
                    return xInitiative.Initiative.CompareTo(yInitiative.Initiative);
                });
            }
        }
    }

    public void moveBtn()
    {
        toMove = true;
        Debug.Log("clicked move button");
    }


}
