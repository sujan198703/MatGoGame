using System;
using System.Collections.Generic;
using UnityEngine.UI;
using StorageData;

[Serializable]
public class PlayerDataManager
{
    // Public Variables
    public int nyangsTotal;
    public int nyangsPocket;
    public int nyangsSafe;
    public int chipsPocket;
    public int chipsSafe;
    public int chipsTotal;
    public int refillsLeft;
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
    public List<QuestTabContent> questTabContent;
    public List<QuestTabContent> questTabContentClaimed;
    public List<GiftTabContent> giftTabContent;
    public List<GiftTabContent> giftTabContentClaimed;
    public List<ItemTabContent> itemTabContent;
    public List<ItemTabContent> itemTabContentClaimed;
    public List<MailTabContent> mailTabContent;
    public List<MailTabContent> mailTabContentClaimed;
    public List<string> remainingTime_giftTabContent;
    public List<string> remainingTime_ItemTabContent;
    public List<string> remainingTime_MailTabContent;
    public List<string> finishingTime_giftTabContent;
    public List<string> finishingTime_itemTabContent;
    public List<string> finishingTime_mailTabContent;
    public DataTypes.GameModes gameMode;

    // New game settings
    public PlayerDataManager()
    {
        this.nyangsTotal = 0;
        this.nyangsPocket = 0;
        this.nyangsSafe = 0;
        this.chipsPocket = 0;
        this.chipsSafe = 0;
        this.chipsTotal = 0;
        this.refillsLeft = 20;
        this.rubies = 0;
        this.playerLevel = 0;
        this.playerLevelExperience = 0;
        this.playerLevelExperienceToAdd = 0;
        this.dailyLossLimit = 500000;
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
        this.questTabContent = new List<QuestTabContent>();
        this.questTabContentClaimed = new List<QuestTabContent>();
        this.giftTabContent = new List<GiftTabContent>();
        this.giftTabContentClaimed = new List<GiftTabContent>();
        this.itemTabContent = new List<ItemTabContent>();
        this.itemTabContentClaimed = new List<ItemTabContent>();
        this.mailTabContent = new List<MailTabContent>();
        this.mailTabContentClaimed = new List<MailTabContent>();
        this.remainingTime_giftTabContent = new List<string>() { "" };
        this.remainingTime_ItemTabContent = new List<string>() { "" };
        this.remainingTime_MailTabContent = new List<string>() { "" };
        this.finishingTime_giftTabContent = new List<string>() { "" };
        this.finishingTime_itemTabContent = new List<string>() { "" };
        this.finishingTime_mailTabContent = new List<string>() { "" };
        this.gameMode = DataTypes.GameModes.Normal;
    }
}

namespace StorageData
{
    public class DataTypes
    {
        public enum GameModes { Normal, Master, Sharper, Freedom }
    }
}
