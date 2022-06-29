using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDataStorageManager : MonoBehaviour
{
    [HideInInspector] public PlayerDataManager playerDataManager;
    private List<PlayerDataStorageInterface> playerDataStorageObjects;
    private PlayerDataFileManager playerDataHandler;
    public static PlayerDataStorageManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        instance = this;
    }

    private void Start()
    {
        // Replace persistentdatapath with local or server location
        //this.playerDataHandler = new PlayerDataFileManager(Application.persistentDataPath, fileName);
        this.playerDataStorageObjects = FindAllStorageObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.playerDataManager = new PlayerDataManager();
    }

    public void LoadGame()
    {
        // Load any saved data from a file
        this.playerDataManager = playerDataHandler.Load();

        // Load any saved game from a file using the data handler
        if (this.playerDataManager == null)
        {
            Debug.Log("No data was found. Initializing to default values.");
            NewGame();
        }

        // Push the loaded data to all other scripts that need it
        foreach (PlayerDataStorageInterface dataStorageObj in playerDataStorageObjects)
        {
            dataStorageObj.LoadData(playerDataManager);
        }

        Debug.Log("Total Nyans " + playerDataManager.nyangs);
    }

    public void SaveGame()
    {
        // Pass the data to other scripts so they can update it
        foreach (PlayerDataStorageInterface dataStorageObj in playerDataStorageObjects)
        {
            dataStorageObj.SaveData(ref playerDataManager);
        }

        // Save that data to a file using the data handler
        playerDataHandler.Save(playerDataManager);
    }

    private List<PlayerDataStorageInterface> FindAllStorageObjects()
    {
        IEnumerable<PlayerDataStorageInterface> dataStorageObjects = FindObjectsOfType<MonoBehaviour>().OfType<PlayerDataStorageInterface>();

        return new List<PlayerDataStorageInterface>(dataStorageObjects);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}

