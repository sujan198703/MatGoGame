using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDataStorageManager : MonoBehaviour
{
    [Header("File Storage Configuration")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private PlayerDataManager playerDataManager = new PlayerDataManager();
    private List<PlayerDataStorageInterface> playerDataStorageObjects;
    private PlayerDataFileManager playerDataHandler;
    public static PlayerDataStorageManager instance { get; private set; }

    private float initialDuration = 3.0f; // 3 second buffer before game loads
    private bool initialDurationOver;

    void Awake()
    {
        // Instantiate
        if (instance == null)
            instance = this;

        // Replace persistentdatapath with local or server location
        this.playerDataHandler = new PlayerDataFileManager(Application.persistentDataPath + "/Matgo/", fileName, useEncryption);
        this.playerDataStorageObjects = FindAllStorageObjects();
        //LoadGame();

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (initialDuration > 0f) initialDuration -= Time.deltaTime;
        else
        if (!initialDurationOver) { initialDurationOver = true; LoadGame(); }
    }

    //public void NewGame()
    //{
    //    this.playerDataManager = new PlayerDataManager();
    //}

    public void LoadGame()
    {
        // Load any saved data from a file
        if (this.playerDataManager != null)
        {
            this.playerDataManager = playerDataHandler.Load();
            Debug.Log("Data was found. Loading.");
        }

        // Load any saved game from a file using the data handler
        //if (this.playerDataManager == null)
        //{
        //    Debug.Log("No data was found. Initializing to default values.");
        //    NewGame();
        //}

        // Push the loaded data to all other scripts that need it
        if (playerDataStorageObjects != null)
        {
            foreach (PlayerDataStorageInterface dataStorageObj in playerDataStorageObjects)
            {
                dataStorageObj.LoadData(playerDataManager);
            }
        }

        // Update Lobby Manager if available
        //if (LobbyManager.instance != null)
        //    LobbyManager.instance.UpdateValues(playerDataManager);
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

    void OnApplicationFocus(bool focus)
    {
        if (initialDurationOver)
        {
            if (focus) LoadGame();
            else SaveGame();
        }
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
}

