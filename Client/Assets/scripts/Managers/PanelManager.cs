using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Panels")]
    public ChipSafePanel chipSafePanel;
    public DailyQuestPanel dailyQuestPanel;
    public EventsPanel eventsPanel;
    public InventoryPanel inventoryPanel;
    public LobbyPanel lobbyPanel;
    public LuckyTicketPanel luckyTicketPanel;
    public MovieRewardPanel movieRewardPanel;
    public NyangSafePanel nyangSafePanel;
    public PigBankPanel pigBankPanel;
    public ProfilePanel profilePanel;
    public SettingsPanel settingsPanel;
    public ShopPanel shopPanel;
    public static PanelManager instance { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;
    }
}
