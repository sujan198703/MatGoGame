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
    public int playerLevelExperienceToLevelUp;
    public int playerLevelExperienceToAdd;
    public int nyangSafeTier;
    public int chipSafeTier;
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
    public int todaysWins;
    public int todaysLosses;
    public int todaysAllInRate;
    public int todaysHighestWinAmount;
    public int todaysHighestWinScore;
    public int todaysBestWinningStreak;
    public int totalWins;
    public int totalLosses;
    public int totalWinningRate;
    public int totalAllInRate;
    public int totalHighestWinAmount;
    public int totalHighestWinScore;
    public int totalBestWinningStreak;
    public int unreadNotificationsEventsPanel;
    public int unreadNotificationsInventoryPanel;
    public int unreadNotificationsDailyQuestPanel;
    public int unreadNotificationsShopPanel;
    public int unreadNotificationsInventoryPanel_GiftTab;
    public int unreadNotificationsInventoryPanel_ItemTab;
    public int unreadNotificationsInventoryPanel_MailTab;
    public long dailyLossLimit;
    public long nyangPocketLimit;
    public long chipPocketLimit;
    public long nyangSafeLimit;
    public long chipSafeLimit;
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
        this.playerLevel = 1;
        this.playerLevelExperience = 0;
        this.playerLevelExperienceToLevelUp = 0;
        this.playerLevelExperienceToAdd = 0;
        this.nyangSafeTier = 0;
        this.chipSafeTier = 0;
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
        this.todaysWins = 0;
        this.todaysLosses = 0;
        this.todaysAllInRate = 0;
        this.todaysHighestWinAmount = 0;
        this.todaysHighestWinScore = 0;
        this.todaysBestWinningStreak = 0;
        this.totalWins = 0;
        this.totalLosses = 0;
        this.totalWinningRate = 0;
        this.totalAllInRate = 0;
        this.totalHighestWinAmount = 0;
        this.totalHighestWinScore = 0;
        this.totalBestWinningStreak = 0;
        this.unreadNotificationsEventsPanel = 0;
        this.unreadNotificationsInventoryPanel = 0;
        this.unreadNotificationsDailyQuestPanel = 0;
        this.unreadNotificationsShopPanel = 0;
        this.unreadNotificationsInventoryPanel_GiftTab = 0;
        this.unreadNotificationsInventoryPanel_ItemTab = 0;
        this.unreadNotificationsInventoryPanel_MailTab = 1;
        this.dailyLossLimit = 6600000000;
        this.nyangPocketLimit = 1000;
        this.chipPocketLimit = 1;
        this.nyangSafeLimit = 4500000000;
        this.chipSafeLimit = 4500000000;
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
