using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;


public class DataManager : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();
    public MapData mapData = new MapData();
    public InventoryData inventoryData = new InventoryData(40);

    string path;
    string player_filename = "player_data";
    string map_filename = "map_data";
    string inventory_filename = "inventory_data";

    void Awake()
    {
        path = Application.persistentDataPath + "/";
        //Debug.Log(path);
        LoadData();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
            Debug.Log("saved game");
        }
      
    }

    public void SaveData()
    {
        string data_player = JsonUtility.ToJson(playerData);
        File.WriteAllText(path + player_filename, data_player);

        string data_map = JsonUtility.ToJson(mapData);
        File.WriteAllText(path + map_filename, data_map);

        string data_inventory = JsonUtility.ToJson(inventoryData);
        File.WriteAllText(path + inventory_filename, data_inventory);
    }
    public void LoadData()
    {
        if (File.Exists(path + player_filename))
        {
            string data_player = File.ReadAllText(path + player_filename);
            playerData = JsonUtility.FromJson<PlayerData>(data_player);
        }

        if (File.Exists(path + map_filename))
        {
            string data_map = File.ReadAllText(path + map_filename);
            mapData = JsonUtility.FromJson<MapData>(data_map);
        }

        if (File.Exists(path + inventory_filename))
        {
            string data_inventory = File.ReadAllText(path + inventory_filename);
            inventoryData = JsonUtility.FromJson<InventoryData>(data_inventory);
        }

    }


}


