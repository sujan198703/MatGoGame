using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerDataFileManager
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public PlayerDataFileManager(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public PlayerDataManager Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        PlayerDataManager loadedPlayerData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // Deserialized data from file
                string playerDataToLoad = "";
              
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        playerDataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize read data
                loadedPlayerData = JsonUtility.FromJson<PlayerDataManager>(playerDataToLoad);
            }
            catch (Exception) { }
        }
        return loadedPlayerData;
    }

    public void Save(PlayerDataManager playerData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize player data obj to Json
            string playerDataJson = JsonUtility.ToJson(playerData, true);

            // Write the serialized player data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(playerDataJson);
                }            
            }
        }
        catch (Exception) {}
    }
}
