using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StorageData;

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

    float luckyTicketTimerValue = 1800; // 30 minutes

    int unreadNotificationsEventPanel;
    int unreadNotificationsInventoryPanel;
    int unreadNotificationsDailyQuestPanel;
    int unreadNotificationsShopPanel;

    DataTypes.GameModes gameMode;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

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
        UpdateAnnouncements();
        ResetLuckyTicketTimer();
        UpdateNotificationTexts();
    }

    void UpdateNotificationTexts()
    {
        eventButtonNotificationText.text = unreadNotificationsEventPanel.ToString();
        inventoryButtonNotificationText.text = unreadNotificationsInventoryPanel.ToString();
        dailyQuestButtonNotificationText.text = unreadNotificationsDailyQuestPanel.ToString();
        shopButtonNotificationText.text = unreadNotificationsShopPanel.ToString();
    }

    void ResetLuckyTicketTimer() => luckyTicketTimerValue = 1800f;

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

    void UpdateAnnouncements()
    {
        announcementText.text = "알림";
    }

    public void LoadData(PlayerDataManager data)
    {
        unreadNotificationsEventPanel = data.unreadNotificationsEventsPanel;
        unreadNotificationsInventoryPanel = data.unreadNotificationsInventoryPanel;
        unreadNotificationsDailyQuestPanel = data.unreadNotificationsDailyQuestPanel;
        unreadNotificationsShopPanel = data.unreadNotificationsShopPanel;
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus) UpdateValues();
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }
}
