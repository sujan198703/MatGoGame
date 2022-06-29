using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerDataFileManager
{
    public PlayerDataManager Load()
    {
        if (!File.Exists(Application.dataPath + "/MOTGOShopData.json"))
            return null;

        string PlayerData = File.ReadAllText(Application.dataPath + "/MOTGOShopData.json");
        PlayerDataManager playerData = JsonUtility.FromJson<PlayerDataManager>(PlayerData);
        return playerData;
    }

    public void Save(PlayerDataManager playerData)
    {
        string playerDataJson = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(Application.dataPath + "/MOTGOShopData.json", playerDataJson);
    }
}
