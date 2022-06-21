using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    //public Texture2D buttonEnabled;
    //public Texture2D buttonDisabled;

    [Header("Profile Panel")]
    public GameObject profilePanel;
    public Text profileName;
    public Text profileEmail;
    public Text profileMembershipCode;
    public Text currentLevel;
    public Text experienceLeftToLevelUp;
    public Text availableNyangLimit;
    public Text nyangAvailable;
    public Text nyangInSafe;
    public Text limitOfNyangSafe;

    public Text availableChipLimit;
    public Text chipsAvailable;
    public Text chipsInSafe;
    public Text limitOfChipsSafe;

    public Text totalRubies;

    public Text totalMoneyLost;
    public Text freeMoneyRefills;

    public Image experienceProgressBar;

    [Header("Avatar Panel")]
    public GameObject avatarPanel;

    [Header("Game Information Panel")]
    public GameObject gameInformationPanel;
    public Text todaysWinsAndLossesAndWinningRate;
    public Text todaysAllInRate;
    public Text todaysHighestWinAmount;
    public Text todaysHighestWinScore;
    public Text todaysBestWinningStreak;
    public Text totalWinsAndLossesAndWinningRate;
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
