using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // Private Variables
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

    // Public Variables
    [Header("Panels")]
    public EventsPanel eventsPanel;
    public DailyQuestPanel dailyQuestPanel;
    public ChipSafePanel chipSafePanel;
    public NyangSafePanel nyangSafePanel;
    public MovieRewardPanel movieRewardPanel;
    public LuckyTicketPanel luckyTicketPanel;
    public PigBankPanel pigBankPanel;
    public ProfilePanel profilePanel;
    public SettingsPanel settingsPanel;
    public ShopPanel shopPanel;

    // Static Variables
    private static LobbyManager _instance;

    public static LobbyManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LobbyManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Start()
    {
        UpdatePlayerDataValues();
    }

    void UpdatePlayerDataValues()
    {
        // Player Data
        currentNyangs.text = PlayerDataStorageManager.instance.playerDataManager.nyangsPocket.ToString() + " 냥";
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
