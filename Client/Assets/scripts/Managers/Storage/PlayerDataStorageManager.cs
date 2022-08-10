using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataStorageManager : MonoBehaviour
{
    [Header("File Storage Configuration")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private List<PlayerDataStorageInterface> playerDataStorageObjects = new List<PlayerDataStorageInterface>();
    private PlayerDataManager playerDataManager;
    private PlayerDataFileManager playerDataFileHandler;
    private PlayerDataCloudManager playerDataCloudHandler;

    public static PlayerDataStorageManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        playerDataFileHandler = new PlayerDataFileManager(Application.persistentDataPath, fileName, useEncryption);
        LoadGame();
    }

    public void SaveThenLoad()
    {
        SaveGame();
        LoadGame();
    }
    
    public void LoadThenSave()
    {
        LoadGame();
        SaveGame();
    }

    public void LoadGame()
    {
        // Load any saved data from a file
        this.playerDataManager = playerDataFileHandler.Load();

        // Load any saved game from a file using the data handler
        if (this.playerDataManager == null)
        {
            Debug.Log("No data was found. Initializing to default values.");
            NewGame();
        }

        // Push the loaded data to all other scripts that need it
        foreach (PlayerDataStorageInterface dataStorageObj in playerDataStorageObjects)
        {
            if (dataStorageObj != null)
                dataStorageObj.LoadData(playerDataManager);
        }

        SaveGame();
    }

    void NewGame()
    {
        this.playerDataManager = new PlayerDataManager();
    }

    public void SaveGame()
    {
        // Pass the data to other scripts so they can update it
        foreach (PlayerDataStorageInterface dataStorageObj in playerDataStorageObjects)
        {
            if (dataStorageObj != null) dataStorageObj.SaveData(ref playerDataManager);
        }

        // Save that data to a file using the data handler
        playerDataFileHandler.Save(playerDataManager);
    }

    // Pass all Interface references so when Saving/Loading, call their implicit functions directly using reference
    public void AddToDataStorageObjects(PlayerDataStorageInterface pdsInterfaceObj) => playerDataStorageObjects.Add(pdsInterfaceObj);

    // Helper functions
    public DateTime StringToDateTime(string DateString)
    {
        if (String.IsNullOrEmpty(DateString))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(DateString);
        }
    }

    void OnApplicationQuit() => SaveGame();
}

