using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour, PlayerDataStorageInterface
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
                _instance = FindObjectOfType<LobbyManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Start()
    {
        UpdateAnnouncements();
    }


    void UpdateAnnouncements()
    {
        announcementText.text = "알림";
    }

    public void LoadData(PlayerDataManager data)
    {
        // Player Data 
        currentNyangs.text = data.nyangsPocket.ToString() + " 냥";
        currentMatgoChips.text = data.chipsPocket.ToString() + " 칩";
        currentRubies.text = data.rubies.ToString() + " 루비";

        // Misc
        UpdateAnnouncements();
    }

    public void SaveData(ref PlayerDataManager data)
    {

    }
}
