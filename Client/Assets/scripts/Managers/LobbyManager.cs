using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
   
    // Public Variables
    [Header("Panels")]
    public EventsPanel eventsPanel;
    public DailyQuestPanel dailyQuestPanel;
    public ChipSafePanel chipSafePanel;
    public NyangSafePanel nyangSafePanel;
    public MovieRewardPanel movieRewardPanel;
    public LobbyPanel lobbyPanel;
    public LuckyTicketPanel luckyTicketPanel;
    public PigBankPanel pigBankPanel;
    public ProfilePanel profilePanel;
    public SettingsPanel settingsPanel;
    public ShopPanel shopPanel;

    [Header("GUI")]
    [SerializeField] TextMeshProUGUI currentNyangs;
    [SerializeField] TextMeshProUGUI currentMatgoChips;
    [SerializeField] TextMeshProUGUI currentRubies;

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
}
