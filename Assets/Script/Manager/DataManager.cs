using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class PlayerData
{
    public int stat_initiative;
    public int stat_strength;
    public int stat_evasion;
    public int stat_luck;
    public int stat_bargain;

    public int availablePoints;

    public float current_health;
    public float maximum_health;

    public Vector3 current_position;
}

public class MapData
{
    public bool[] foundWayPoint;
    // ∏  π‡»˘ ∫Œ∫– ºŒ¿Ã¥ı?
}


public class DataManager : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();
    public MapData mapData = new MapData();

    string path;
    string player_filename = "player_data";
    string map_filename = "map_data";

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
            
    }


}


