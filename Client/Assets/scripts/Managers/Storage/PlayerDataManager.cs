using System;
using UnityEngine.UI;

[System.Serializable]
public class PlayerDataManager
{
    public int nyangsTotal;
    public int nyangsPocket;
    public int nyangsSafe;
    public int chipsPocket;
    public int chipsSafe;
    public int chipsTotal;
    public int rubies;
    public int safeTier;
    public int nyangAdsWatched;
    public int chipAdsWatched;
    public int characterIndex;
    public int nyangsWon;
    public int unreadNotificationsEventsPanel;
    public int unreadNotificationsInventoryPanel;
    public int unreadNotificationsDailyQuestPanel;
    public int unreadNotificationsShopPanel;
    public int unreadNotificationsInventoryPanel_GiftTab;
    public int unreadNotificationsInventoryPanel_ItemTab;
    public int unreadNotificationsInventoryPanel_MailTab;
    public string playerName;
    public string playerEmail;
    public string playerMembershipCode;
    public Image playerProfilePicture;
    public DateTime profileNameChangeTime;
    public bool vibrationEnabled;
    
    // New game settings
    public PlayerDataManager()
    {
        this.nyangsTotal = 0;
        this.nyangsPocket = 0;
        this.nyangsSafe = 0;
        this.rubies = 0;
        this.chipsPocket = 0;
        this.chipsSafe = 0;
        this.chipsTotal = 0;
        this.safeTier = 0;
        this.nyangAdsWatched = 0;
        this.chipAdsWatched = 0;
        this.nyangsWon = 0;
        this.unreadNotificationsEventsPanel = 0;
        this.unreadNotificationsInventoryPanel = 0;
        this.unreadNotificationsDailyQuestPanel = 0;
        this.unreadNotificationsShopPanel = 0;
        this.playerName = "";
        this.playerEmail = "";
        this.playerProfilePicture = null;
        this.profileNameChangeTime = DateTime.UtcNow.Add(new TimeSpan(24, 0, 0));
        this.vibrationEnabled = true;
    }
}
