using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRange : MonoBehaviour
{
    public bool playerEntered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player entered");
            playerEntered = true;
        }
    }
}
