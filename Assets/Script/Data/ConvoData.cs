using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class ConvoData : MonoBehaviour
{
    public AllData datas;
    public TextAsset data;

    void Awake()
    {
        datas = JsonUtility.FromJson<AllData>(data.text);       
    }

    void Start()
    {
        try
        {
            if (datas != null)
            {
                Debug.Log("successfuly loaded data");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }        
    }  
}



[System.Serializable]
public class AllData
{
    public AmtakConvo[] FirstTalk;
    public AmtakConvo[] SecondTalk;
    public AmtakConvo[] RewardTalk;
}

[System.Serializable]
public class AmtakConvo
{
    public string convo;
}

