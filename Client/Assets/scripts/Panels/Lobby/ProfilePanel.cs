using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour, PlayerDataStorageInterface
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
    public Image profilePicture;

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

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    private void Start() { PlayerDataStorageManager.instance.LoadGame(); }

    public void CopyEmail()
    {
        TextEditor clipboard = new TextEditor();
        clipboard.text = profileEmail.text;
        clipboard.SelectAll();
        clipboard.Copy();
    }

    public void CopyMembershipCode()
    {
        TextEditor clipboard = new TextEditor();
        clipboard.text = profileMembershipCode.text;
        clipboard.SelectAll();
        clipboard.Copy();
    }

    public void LoadData(PlayerDataManager data)
    {
        profileName.text = PlayerPrefs.GetString("ProfileName");
        profileEmail.text = PlayerPrefs.GetString("ProfileEmail");
        profileMembershipCode.text = PlayerPrefs.GetString("ProfileMembershipCode");

        currentLevel.text = "Lv." + data.playerLevel.ToString();
        experienceProgressBar.fillAmount = data.playerLevelExperience;
        experienceLeftToLevelUp.text = "레벨업까지 " + data.playerLevelExperienceToLevelUp.ToString() + " exp 남음";
       
        availableNyangLimit.text = "(한도 : " + data.nyangPocketLimit.ToString() + " 억 냥)";
        nyangAvailable.text = (data.nyangsPocket + data.nyangsSafe).ToString() + " 냥";
        nyangInSafe.text = data.nyangsSafe.ToString() + " 냥";
        limitOfNyangSafe.text = "(한도 : " + data.nyangSafeLimit.ToString() + ")";

        availableChipLimit.text = "(한도: " + data.chipPocketLimit.ToString() + "조 맞고칩)";
        chipsAvailable.text = (data.chipsPocket + data.chipsSafe).ToString() + " 칩";
        chipsInSafe.text = data.chipsSafe.ToString() + " 칩";
        limitOfChipsSafe.text = "(한도 : " + data.chipSafeLimit.ToString() + ")";

        totalRubies.text = data.rubies.ToString() + " 루비";

        totalMoneyLost.text = data.nyangsLostToday.ToString() + " 냥" + '\n' + data.chipsLostToday.ToString() + " 칩";
        freeMoneyRefills.text = data.refillsLeft.ToString() + " 개 리필 가능";

        if (data.todaysWins == 0 && data.todaysLosses == 0)
            todaysWinsAndLossesAndWinningRate.text = "오늘의 전적 : <color=#FF213B>총 " + (data.todaysWins + data.todaysLosses).ToString() + "전 " + data.todaysWins.ToString() + "승 " + data.todaysLosses.ToString() + "패 </color>(승률 0%)"; 
        else
            todaysWinsAndLossesAndWinningRate.text = "오늘의 전적 : <color=#FF213B>총 " + (data.todaysWins + data.todaysLosses).ToString() + "전 " + data.todaysWins.ToString() + "승 " + data.todaysLosses.ToString() + "패 </color>(승률 " + data.todaysWins / (data.todaysWins + data.totalLosses) + "%)";

        todaysAllInRate.text = "<color=#FF213B> "+ data.todaysAllInRate.ToString() + " </color>  회";
        todaysHighestWinAmount.text = "<color=#FF213B> " + data.todaysHighestWinAmount.ToString() + " </color>  냥";
        todaysHighestWinScore.text = "<color=#FF213B> " + data.todaysHighestWinScore.ToString() + " </color>  점";
        todaysBestWinningStreak.text = "<color=#FF213B> " + data.todaysBestWinningStreak.ToString() + " </color> 연승";

        if (data.totalWins == 0 && data.totalLosses == 0)
            totalWinsAndLossesAndWinningRate.text = "누적 전적 : <color=#FF213B> 총 " + (data.totalWins + data.totalLosses).ToString() + "전 " + data.totalWins.ToString() + "승 " + data.totalLosses.ToString() + "패  </color>((승률 0%)";
        else
            totalWinsAndLossesAndWinningRate.text = "누적 전적 : <color=#FF213B> 총 " + (data.totalWins + data.totalLosses).ToString() + "전 " + data.totalWins.ToString() + "승 " + data.totalLosses.ToString() + "패  </color>((승률 " + data.totalWins / (data.totalWins + data.totalLosses) + "%)";
        
        totalAllInRate.text = "<color=#FF213B> " + data.totalAllInRate.ToString() + " </color> 회";
        totalHighestWinAmount.text = "<color=#FF213B> " + data.totalHighestWinAmount.ToString() + " </color> 냥";
        totalHighestWinScore.text = "<color=#FF213B> " + data.totalHighestWinScore.ToString() + " </color> 점";
        totalBestWinningStreak.text = "<color=#FF213B> " + data.totalBestWinningStreak.ToString() + " </color>  연승";
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }
}
