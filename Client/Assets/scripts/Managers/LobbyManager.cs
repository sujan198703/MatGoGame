using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("Lobby Screen")]
    [SerializeField] TextMeshProUGUI announcementText;
    [SerializeField] TextMeshProUGUI currentNyangs;
    [SerializeField] TextMeshProUGUI currentMatgoChips;
    [SerializeField] TextMeshProUGUI currentRubies;
    [SerializeField] TextMeshProUGUI profileLevelText;
    [SerializeField] TextMeshProUGUI profileName;
    [SerializeField] Image profilePicture;
    [SerializeField] Image profileProgressBar;
    [SerializeField] Button luckyTicketTimerButton;
    [SerializeField] Text luckyTicketTimerText;

    [Header("Panels")]
    [SerializeField] EventsPanel eventsPanel;
    [SerializeField] DailyQuestPanel dailyQuestPanel;
    [SerializeField] MoneySafePanel moneySafePanel;
    [SerializeField] MovieRewardPanel movieRewardPanel;
    [SerializeField] PigBankPanel pigBankPanel;
    [SerializeField] ProfilePanel profilePanel;
    [SerializeField] SettingsPanel settingsPanel;
    [SerializeField] ShopPanel shopPanel;

    // Private Variables
   
    

    void Start()
    {
        UpdatePlayerDataValues();
    }

    void UpdatePlayerDataValues()
    {
        // Player Data
        currentNyangs.text = PlayerDataStorageManager.instance.playerDataManager.nyangsTotal.ToString() + " 냥";
        currentMatgoChips.text = PlayerDataStorageManager.instance.playerDataManager.matgoChips.ToString() + " 칩";
        currentRubies.text = PlayerDataStorageManager.instance.playerDataManager.rubies.ToString() + " 루비";

        // Misc
        UpdateAnnouncements();
    }

    void UpdateAnnouncements()
    {
        announcementText.text = "알림";
    }

}
