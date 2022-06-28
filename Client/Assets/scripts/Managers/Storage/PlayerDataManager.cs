using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerDataManager
{
    public int nyangsTotal;
    public int nyangsPocket;
    public int nyangsSafe;
    public int rubies;
    public int matgoChips;
    public int safeTier;
    public int nyangAdsWatched;
    public int chipAdsWatched;
    public string playerName;
    public string playerEmail;
    public Image playerProfilePicture;
    public DateTime profileNameChangeTime;
    
    public PlayerDataManager()
    {
        this.nyangsTotal = 0;
        this.nyangsPocket = 0;
        this.nyangsSafe = 0;
        this.rubies = 0;
        this.matgoChips = 0;
        this.safeTier = 0;
        this.nyangAdsWatched = 0;
        this.chipAdsWatched = 0;
        this.playerName = "";
        this.playerEmail = "";
        this.playerProfilePicture = null;
    }
}
