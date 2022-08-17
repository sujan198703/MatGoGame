using System;
using System.Collections;
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

    private List<QuestTabContent> questTabContent;
    private List<QuestTabContent> questTabContentClaimed;
    
    private long rewardAmount;
    private int questTabContentIndex;
    private int unreadNotificationsDailyQuestPanel;

    private QuestContent questContent;

    long nyangsPocket;
    long chipsPocket;
    long rubies;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void OnEnable() => PlayerDataStorageManager.instance.LoadGame();

    void UpdateValues()
    {
        // Update reference from parent
        questContent = PanelManager.instance.dailyQuestPanel.questContent[questTabContentIndex];
       
        // Update values
        // Name
        questContentName.text = questContent.questContentName;

        // Icon
        questContentIcon.sprite = Sprite.Create
               (questContent.questContentIcon, new Rect(0, 0, questContent.questContentIcon.width, questContent.questContentIcon.height),
               new Vector2(questContent.questContentIcon.width / 2, questContent.questContentIcon.height / 2)); ;
        
        // Reward Name
        questContentRewardText.text = questContent.questContentRewardAmount.ToString();

        // Append reward type
        string rewardTypeSuffix = "";
        if (questContent.rewardType == RewardType_QuestContent.Nyangs)
            rewardTypeSuffix = " 냥";
        else if (questContent.rewardType == RewardType_QuestContent.Chips)
            rewardTypeSuffix = " 칩";
        else if (questContent.rewardType == RewardType_QuestContent.Rubies)
            rewardTypeSuffix = " 루비";

        // Amount
        questContentRewardText.text = questContent.questContentRewardAmount + rewardTypeSuffix;

        // Reward Icon
        questContentRewardIcon.sprite = Sprite.Create
               (questContent.questContentRewardIcon, new Rect(0, 0, questContent.questContentRewardIcon.width, questContent.questContentRewardIcon.height),
               new Vector2(questContent.questContentRewardIcon.width / 2, questContent.questContentRewardIcon.height / 2)); ;

        // Update claimed
        if (questTabContent.Exists(x => x.GetInstanceID() == this.GetInstanceID())) Unclaimed();

        // Update unclaimed
        if (questTabContentClaimed.Exists(x => x.GetInstanceID() == this.GetInstanceID())) Claimed();

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
        if (questTabContent.Exists(x => x == this)) return false;
        if (questTabContentClaimed.Exists(x => x == this)) return true;
        
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
        // Add to list
        if (!questTabContent.Exists(x => x == this)) questTabContent.Add(this);
    }

    public void ClaimReward()
    {
        // Claim reward
        Claimed();

        // Add reward amount
        rewardAmount = PanelManager.instance.dailyQuestPanel.questContent[transform.GetSiblingIndex()].questContentRewardAmount;

        // Remove from List
        if (questTabContent.Exists(x => x == this)) questTabContent.Remove(this);

        // Add to claimed list
        if (!questTabContentClaimed.Exists(x => x == this)) questTabContentClaimed.Add(this);

        // Save then load
        PlayerDataStorageManager.instance.SaveThenLoad();

        // Update rewards
        GetReward();

        // Reset reward amount
        rewardAmount = 0;

        // Update index of unused object to the end of the list
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    void GetReward()
    {
        // Save reward based on type
        switch (PanelManager.instance.dailyQuestPanel.questContent[questTabContentIndex].rewardType)
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
        
        PlayerDataStorageManager.instance.SaveThenLoad();
    }

    public void UpdateIndex(int index) => questTabContentIndex = index;

    public void LoadData(PlayerDataManager data)
    {
        questTabContent = data.questTabContent;
        questTabContentClaimed = data.questTabContentClaimed;

        nyangsPocket = data.nyangsPocket;
        chipsPocket = data.chipsPocket;
        rubies = data.rubies;

        UpdateValues();
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
