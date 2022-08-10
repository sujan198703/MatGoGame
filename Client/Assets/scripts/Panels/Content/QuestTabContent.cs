using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static QuestContent;

public class QuestTabContent : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject receiveButton;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private Image questContentIcon;
    [SerializeField] private Image questContentRewardIcon;
    [SerializeField] private Text questContentRewardText;

    private List<QuestTabContent> questTabContent;
    private List<QuestTabContent> questTabContentClaimed;
    private long rewardAmount;

    private bool claimed;
    private float fadeProgress = 0.5f;
    private byte[] fadeValue;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);
    
    void Start() => UpdateValues();

    void Update()
    {
        if (claimed) 
        {
            if (fadeProgress > 0.0f)
            {
                fadeProgress -= Time.deltaTime;
                fadeValue = BitConverter.GetBytes(fadeProgress * 100f);
               
                // Images
                foreach (Image img in GetComponentsInChildren<Image>())
                {
                    img.color = new Color32(255, 255, 255, fadeValue[3]);
                }

                // Texts
                foreach (Text txt in GetComponentsInChildren<Text>())
                {
                    txt.color = new Color32(255, 255, 255, fadeValue[3]);
                }
            }
            else
            {
                RemoveQuestTabContentObject();
            }
        }
    }

    void UpdateValues() => QuestCompleted();

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

    public void ClaimReward() => claimed = true;

    void RemoveQuestTabContentObject()
    {
        // Add reward amount
        rewardAmount = PanelManager.instance.dailyQuestPanel.questContent[transform.GetSiblingIndex()].questContentRewardAmount;
       
        // Remove from List
        questTabContent.Remove(this);

        // Add to claimed list
        questTabContentClaimed.Add(this);

        // Save game
        PlayerDataStorageManager.instance.SaveGame();

        // Reset reward amount
        rewardAmount = 0;

        // Update index of unused object to the end of the list
        this.transform.SetSiblingIndex(transform.parent.childCount - 1); 
    }

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
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.questTabContent = questTabContent;
        data.questTabContentClaimed = questTabContentClaimed;

        // Save reward based on type
        switch (PanelManager.instance.dailyQuestPanel.questContent[transform.GetSiblingIndex()].rewardType)
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
