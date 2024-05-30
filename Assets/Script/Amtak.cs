using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amtak : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("clicked");
        ConvoManager.instance.clickToTalk = true;
        ConvoManager.instance.target = this.gameObject.transform;
    }
}
