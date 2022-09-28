using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static QuestContent;

public class QuestTabContent : MonoBehaviour, PlayerDataStorageInterface
{
    public GameObject progressBar;
    public GameObject receiveButton;
    public Image progressBarFill;
    public Image questContentIcon;
    public Image questContentRewardIcon;
    public Text questContentName;
    public Text questContentRewardText;
    public long rewardAmount;
    public RewardType_QuestContent rewardType;

    private List<QuestTabContent> questTabContent = new List<QuestTabContent>();
    private List<QuestTabContent> questTabContentClaimed = new List<QuestTabContent>();
    
    private int unreadNotificationsDailyQuestPanel;

    private long nyangsPocket;
    private long chipsPocket;
    private long rubies;

    private string uniqueID = "";

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {
        // Load game
        PlayerDataStorageManager.instance.LoadGame();

        // Update values
        foreach (QuestTabContent qtc in questTabContent)
        {
            if (GetUniqueID().Equals(qtc.GetUniqueID()))
            {
                Unclaimed();
            }
        }

        foreach (QuestTabContent qtc in questTabContentClaimed)
        {
            if (GetUniqueID().Equals(qtc.GetUniqueID()))
            {
                Claimed();
            }
        }

        // Quest progress
        QuestCompleted();
    }

    void QuestCompleted()
    {
        if (progressBarFill.fillAmount > 1.0f)
        {
            progressBar.SetActive(true);
            receiveButton.SetActive(false);
        }
        else
        {
            progressBar.SetActive(false);
            receiveButton.SetActive(true);
        }
    }

    public bool IsAvailable()
    {
        if (questTabContent.Contains(this)) return false;
        if (questTabContentClaimed.Contains(this)) return true;
        
        return false;
    }

    void Claimed()
    {
        // Grey out
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        }

        foreach (Text txt in GetComponentsInChildren<Text>())
        {
            txt.material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        }

        // Disable button
        receiveButton.GetComponent<Button>().interactable = false;
    }

    void Unclaimed()
    {
        
    }

    public void ClaimReward()
    {
        // Claim reward
        Claimed();

        // Remove from unclaimed
        if (questTabContent.Count > 0) questTabContent.RemoveAt(questTabContent.Count - 1);

        // Add to claimed
        QuestTabContent _qtc = Instantiate(this, transform.position, transform.rotation);

        if (!questTabContentClaimed.Contains(_qtc))
        {
            questTabContentClaimed.Add(_qtc);
        }

        // Decrement counter
        if (unreadNotificationsDailyQuestPanel > 0) unreadNotificationsDailyQuestPanel--;

        // Update rewards
        AddReward();

        // Update index of unused object to the end of the list
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);

        // Save then load
        PlayerDataStorageManager.instance.SaveThenLoad();

    }

    void AddReward()
    {
        // Save reward based on type
        switch (rewardType)
        {
            // Nyangs
            case RewardType_QuestContent.Nyangs:
                {
                    nyangsPocket += rewardAmount;
                    break;
                }
            // Chips
            case RewardType_QuestContent.Chips:
                {
                    chipsPocket += rewardAmount;
                    break;
                }
            // Rubies
            case RewardType_QuestContent.Rubies:
                {
                    rubies += rewardAmount;
                    break;
                }
        }
        //Debug.LogError(nyangsPocket + " | " + chipsPocket + " | " + rubies);
    }

    public void SetUniqueID() => uniqueID = (questContentRewardIcon.GetHashCode() + rewardAmount.GetHashCode()).GetHashCode().ToString();

    public string GetUniqueID() 
    { return (questContentRewardIcon.GetHashCode() + rewardAmount.GetHashCode()).GetHashCode().ToString(); }


    public void LoadData(PlayerDataManager data)
    {
        questTabContent = data.questTabContent;
        questTabContentClaimed = data.questTabContentClaimed;

        nyangsPocket = data.nyangsPocket;
        chipsPocket = data.chipsPocket;
        rubies = data.rubies;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.questTabContent = questTabContent;
        data.questTabContentClaimed = questTabContentClaimed;

        data.nyangsPocket = nyangsPocket;
        data.chipsPocket = chipsPocket;
        data.rubies = rubies;
    }
}
