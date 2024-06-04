using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipData 
{
    public string[] imagePath;
    public int[] itemID;
    //public bool[] emptySlot;
    public bool equipable;
    public string[] equipType;

    public EquipData(int size)
    {
        imagePath = new string[size];
        itemID = new int[size];
       // emptySlot = new bool[size];
        equipable = true;


        // 각 배열의 초기값 설정
        for (int i = 0; i < size; i++)
        {
            imagePath[i] = "Resources/unity_builtin_extra";
            itemID[i] = 0;
           // emptySlot[i] = true;
            equipType[i] = "none";
        }

    }
}
