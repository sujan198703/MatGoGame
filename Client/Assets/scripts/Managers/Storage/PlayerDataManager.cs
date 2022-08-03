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
    public int playerLevel;
    public int playerLevelExperience;
    public int playerLevelExperienceToAdd;
    public int dailyLossLimit;
    public int safeTier;
    public int nyangAdsWatched;
    public int chipAdsWatched;
    public int characterIndex;
    public int nyangsWonToday;
    public int nyangsWonTotal;
    public int chipsWonToday;
    public int chipsWonTotal;
    public int nyangsLostToday;
    public int nyangsLostTotal;
    public int chipsLostToday;
    public int chipsLostTotal;
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
<<<<<<< Updated upstream
=======
        this.refillsLeft = 20;
        this.rubies = 0;
        this.playerLevel = 0;
        this.playerLevelExperience = 0;
        this.playerLevelExperienceToAdd = 0;
        this.dailyLossLimit = 500000;
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        this.safeTier = 0;
        this.nyangAdsWatched = 0;
        this.chipAdsWatched = 0;
        this.nyangsWonToday = 0;
        this.nyangsWonTotal = 0;
        this.chipsWonToday = 0;
        this.chipsWonTotal = 0;
        this.nyangsLostToday = 0;
        this.nyangsLostTotal = 0;
        this.chipsLostToday = 0;
        this.chipsLostTotal = 0;
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
