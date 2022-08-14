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

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {
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

        // Add to list
        questTabContent.Add(this);

        // Update unread notifications
        unreadNotificationsDailyQuestPanel++;

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

    public void ClaimReward()
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

        // Add reward amount
        rewardAmount = PanelManager.instance.dailyQuestPanel.questContent[transform.GetSiblingIndex()].questContentRewardAmount;

        // Remove from List
        questTabContent.Remove(this);

        // Add to claimed list
        questTabContentClaimed.Add(this);

        // Save then load
        PlayerDataStorageManager.instance.SaveThenLoad();

        // Reset reward amount
        rewardAmount = 0;

        // Update index of unused object to the end of the list
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public void UpdateIndex(int index) => questTabContentIndex = index;

    public void LoadData(PlayerDataManager data)
    {
        questTabContent = data.questTabContent;
        questTabContentClaimed = data.questTabContentClaimed;

        // Load reward reward based on type
        switch (PanelManager.instance.dailyQuestPanel.questContent[transform.GetSiblingIndex()].rewardType)
        {
            // Nyangs
            case RewardType_QuestContent.Nyangs:
            {
                rewardAmount += data.nyangsPocket;
                break;
            }
            // Chips
            case RewardType_QuestContent.Chips:
            {
                rewardAmount += data.chipsPocket;
                break;
            }
            // Rubies
            case RewardType_QuestContent.Rubies:
            {
                rewardAmount += data.rubies;
                break;
            }
        }

        UpdateValues();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.questTabContent = questTabContent;
        data.questTabContentClaimed = questTabContentClaimed;

        // Save reward based on type
        switch (PanelManager.instance.dailyQuestPanel.questContent[questTabContentIndex].rewardType)
        {
            // Nyangs
            case RewardType_QuestContent.Nyangs:
            {
                data.nyangsPocket += rewardAmount;
                break;
            }
            // Chips
            case RewardType_QuestContent.Chips:
            {
                data.chipsPocket += rewardAmount;
                break;
            }
            // Rubies
            case RewardType_QuestContent.Rubies:
            {
                data.rubies += rewardAmount;
                break;
            }
        }
    }
}
