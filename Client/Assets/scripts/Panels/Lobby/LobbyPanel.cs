using TMPro;
using UnityEngine;
using UnityEngine.UI;
<<<<<<< Updated upstream
=======
using StorageData;
using UnityEngine.SceneManagement;
>>>>>>> Stashed changes

public class LobbyPanel : MonoBehaviour, PlayerDataStorageInterface
{
    // Private Variables
    [Header("Lobby Screen")]
    [SerializeField] TextMeshProUGUI announcementText;
    [SerializeField] TextMeshProUGUI profileLevelText;
    [SerializeField] TextMeshProUGUI profileName;
    [SerializeField] Image profilePicture;
    [SerializeField] Image profileProgressBar;
    [SerializeField] Button luckyTicketTimerButton;
    [SerializeField] Text luckyTicketTimerText;

    [Header("Notification Text Bubbles")]
    [SerializeField] Text eventButtonNotificationText;
    [SerializeField] Text inventoryButtonNotificationText;
    [SerializeField] Text dailyQuestButtonNotificationText;
    [SerializeField] Text shopButtonNotificationText;

    [Header("Game Modes UI")]
    [SerializeField] GameObject[] nyangModeObjects;
    [SerializeField] GameObject[] nyangModeButtons;
    [SerializeField] TextMeshProUGUI[] nyangModePeopleOnlineText;
    [SerializeField] GameObject[] chipModeObjects;
    [SerializeField] GameObject[] chipModeButtons;
    [SerializeField] TextMeshProUGUI[] chipModePeopleOnlineText;

    float luckyTicketTimerValue = 1800; // 30 minutes

    int unreadNotificationsEventPanel;
    int unreadNotificationsInventoryPanel;
    int unreadNotificationsDailyQuestPanel;
    int unreadNotificationsShopPanel;
    
    int nyangsPocket;
    int nyangsSafe;
    int nyangsTotal;
    int chipsPocket;
    int chipsSafe;
    int chipsTotal;
    int playerLevel;
    int playerLevelExperience;
    int playerLevelExperienceToAdd;
    int dailyLossLimit;
    int nyangsLostToday;
    int chipsLostToday;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void OnEnable() => UpdateValues();

    void Update()
    {
        if (luckyTicketTimerValue > 0)
        {
            luckyTicketTimerValue -= Time.deltaTime;

            if (luckyTicketTimerButton.interactable) luckyTicketTimerButton.interactable = false;
        }
        else
        {
            luckyTicketTimerValue = 0;

            // Enable button if disabled
            if (!luckyTicketTimerButton.interactable) luckyTicketTimerButton.interactable = true;
        }

        UpdateLuckyTicketTimer(luckyTicketTimerValue);
    }

    void UpdateValues()
    {
        UpdatePlayerLevel();
        UpdateAnnouncements();
        ResetLuckyTicketTimer();
        UpdateNotificationTexts();
    }

    void UpdatePlayerLevel()
    {
        if (playerLevelExperience < playerLevelExperienceToAdd)
        {
            // Show level up panel
            PopupManager.instance.levelUpPopup.gameObject.SetActive(true);
       
            // Add experience
            playerLevelExperience += playerLevelExperienceToAdd;

            // Update player level text
            profileLevelText.text = playerLevel.ToString();

            // Update progress bar
            profileProgressBar.fillAmount = Mathf.Clamp(playerLevelExperience % 10, 0.0f, 1.0f);

            // Reset experience to add
            playerLevelExperienceToAdd = 0;
        }
    }

    void UpdateAnnouncements()
    {
        announcementText.text = "알림";
    }

    void ResetLuckyTicketTimer() => luckyTicketTimerValue = 1800f;

    void UpdateNotificationTexts()
    {
        eventButtonNotificationText.text = unreadNotificationsEventPanel == 0 ? "N" : unreadNotificationsEventPanel.ToString();
        inventoryButtonNotificationText.text = unreadNotificationsInventoryPanel == 0 ? "N" : unreadNotificationsInventoryPanel.ToString();
        dailyQuestButtonNotificationText.text = unreadNotificationsDailyQuestPanel == 0 ? "N" : unreadNotificationsDailyQuestPanel.ToString();
        shopButtonNotificationText.text = unreadNotificationsShopPanel == 0 ? "N" : unreadNotificationsShopPanel.ToString();
    }

    void UpdateLuckyTicketTimer(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // 0 index hours two digits, 1 index minutes two digits, 2 index seconds two digits 
        luckyTicketTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus) UpdateValues();
    }

    // GAME MENUS
    public void SelectNyangMode(int modeIndex)
    {
        nyangsTotal = nyangsPocket + nyangsSafe;

        // If daily loss limit exceeded
        if (CheckDailyLossLimit("Nyangs"))
        {
            PopupManager.instance.lossLimitPopup.gameObject.SetActive(true);
        }
        // If daily loss limit not exceeded
        else
        {
            switch (modeIndex)
            {
                case 0:
                    //if (nyangsTotal > 5000 && nyangsTotal < 1500000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Normal);
                        GoToGameplayScene();
                    }
                    break;
                case 1:
                    //if (nyangsTotal > 5000 && nyangsTotal < 1500000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Normal);
                        GoToGameplayScene();
                    }
                    break;
                case 2:
                    //if (nyangsTotal > 100000 && nyangsTotal < 10000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Normal);
                        GoToGameplayScene();
                    }
                    break;
                case 3:
                    //if (nyangsTotal > 2000000 && nyangsTotal < 20000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Master);
                        GoToGameplayScene();
                    }
                    break;
                case 4:
                    //if (nyangsTotal > 10000000 && nyangsTotal < 100000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Master);
                        GoToGameplayScene();
                    }
                    break;
                case 5:
                    //if (nyangsTotal > 50000000 && nyangsTotal < 1000000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Sharper);
                        GoToGameplayScene();
                    }
                    break;
                case 6:
                    //if (nyangsTotal > 100000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Sharper);
                        GoToGameplayScene();
                    }
                    break;
                case 7:
                    //if (nyangsTotal > 300000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Freedom);
                        GoToGameplayScene();
                    }
                    break;
            }
        }
    }

    public void SelectChipMode(int modeIndex)
    {
        chipsTotal = chipsPocket + chipsSafe;

        // If daily loss limit exceeded
        if (CheckDailyLossLimit("Chips"))
        {
            PopupManager.instance.lossLimitPopup.gameObject.SetActive(true);
        }
        else
        {
            switch (modeIndex)
            {
                case 1:
                    //if (chipsTotal > 50000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Normal);
                        GoToGameplayScene();
                    }
                    break;
                case 2:
                    //if (chipsTotal > 100000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Normal);
                        GoToGameplayScene();
                    }
                    break;
                case 3:
                    //if (chipsTotal > 300000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Master);
                        GoToGameplayScene();
                    }
                    break;
                case 4:
                    //if (chipsTotal > 500000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Sharper);
                        GoToGameplayScene();
                    }
                    break;

                case 5:
                    //if (chipsTotal > 1000000)
                    {
                        UpdateGameMode(DataTypes.GameModes.Freedom);
                        GoToGameplayScene();
                    }
                    break;
            }
        }
    }

    void Locked()
    {

    }

    void Unlockable()
    {

    }

    void Unlocked()
    {

    }

    bool CheckDailyLossLimit(string valueType)
    {
        if (valueType.Equals("Nyangs"))
        {
            if (nyangsLostToday >= dailyLossLimit) return true;
            else return false;
        }
        if (valueType.Equals("Chips"))
        {
            if (chipsLostToday >= dailyLossLimit) return true;
            else return false;
        }
        return false;
    }

    void UpdateGameMode(DataTypes.GameModes gameMode) => this.gameMode = gameMode;

    void GoToGameplayScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(SoundManager.instance.gameObject); //~!
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = nyangsPocket;
        data.nyangsSafe = nyangsSafe;
        data.nyangsTotal = nyangsTotal;
        data.chipsPocket = chipsPocket;
        data.chipsSafe = chipsSafe;
        data.chipsTotal = chipsTotal;
        data.playerLevel = playerLevel;
        data.playerLevelExperience = playerLevelExperience;
        data.playerLevelExperienceToAdd = playerLevelExperienceToAdd;
        data.dailyLossLimit = dailyLossLimit;
        data.nyangsLostToday = nyangsLostToday;
        data.chipsLostToday = chipsLostToday;
    }

    public void LoadData(PlayerDataManager data)
    {
        unreadNotificationsEventPanel = data.unreadNotificationsEventsPanel;
        unreadNotificationsInventoryPanel = data.unreadNotificationsInventoryPanel;
        unreadNotificationsDailyQuestPanel = data.unreadNotificationsDailyQuestPanel;
        unreadNotificationsShopPanel = data.unreadNotificationsShopPanel;

        nyangsPocket = data.nyangsPocket;
        nyangsSafe = data.nyangsSafe;
        nyangsTotal = data.nyangsTotal;
        chipsPocket = data.chipsPocket;
        chipsSafe = data.chipsSafe;
        chipsTotal = data.chipsTotal;
        playerLevel = data.playerLevel;
        playerLevelExperience = data.playerLevelExperience;
        playerLevelExperienceToAdd = data.playerLevelExperienceToAdd;
        dailyLossLimit = data.dailyLossLimit;
        nyangsLostToday = data.nyangsLostToday;
        chipsLostToday = data.chipsLostToday;
    }
}
