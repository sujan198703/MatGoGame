using UnityEngine;
using System;
using System.IO;

public class PlayerDataFileManager
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "USE_APPID_OR_SOMETHING";

    public PlayerDataFileManager(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
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

                // Decrypt
                if (useEncryption)
                {
                    playerDataToLoad = EncryptDecrypt(playerDataToLoad);
                }

                // Deserialize read data
                loadedPlayerData = JsonUtility.FromJson<PlayerDataManager>(playerDataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured while trying to load data from file: " + fullPath + "\n" + e);
            }
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

            // Encrypt
            if (useEncryption)
            {
                playerDataJson = EncryptDecrypt(playerDataJson); 
            }

            // Write the serialized player data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(playerDataJson);
                }            
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}
