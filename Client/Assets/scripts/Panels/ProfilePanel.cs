using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    //public Texture2D buttonEnabled;
    //public Texture2D buttonDisabled;

    [Header("Profile Panel")]
    public Text experienceLeftToLevelUp;
    public Text limitText; 
    public Text totalMoney;
    public Text moneyAvailable;
    public Text moneyInSafe;
    public Text limitOfSafe;

    public Text chipsAvailable;
    public Text chipsInSafe;

    public Text totalRubies;

    public Text totalMoneyLost;
    public Text freeMoneyRefills;

    public Image experienceProgressBar;


    [Header("Avatar Panel")]

    [Header("Game Information Panel")]
    public Text todaysWinsAndLosses;
    public Text todaysWinningRate;
    public Text todaysAllInRate;
    public Text todaysHighestWinAmount;
    public Text todaysHighestWinScore;
    public Text todaysBestWinningStreak;
    public Text totalWinsAndLosses;
    public Text totalWinningRate;
    public Text totalAllInRate;
    public Text totalHighestWinAmount;
    public Text totalHighestWinScore;
    public Text totalBestWinningStreak;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
