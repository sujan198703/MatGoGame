using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Image progressBar;
    [SerializeField] GameObject questTabContentParent;
    [SerializeField] QuestTabContent questTabContentPrefab;
    public QuestContent[] questContent;
    GameObject tempDailyQuest;

    int unreadNotificationsDailyQuestPanel;
    List<QuestTabContent> questTabContent = new List<QuestTabContent>();

    bool questTabContentObjectsInstantiated;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => PlayerDataStorageManager.instance.LoadGame(); 

    void UpdateValues() 
    {
        if (!questTabContentObjectsInstantiated)
        {
            for (int i = 0; i < questContent.Length; i++)
            {
                AddDailyQuest(i);
            }

            questTabContentObjectsInstantiated = true;
        }
    }

    public void GetAll()
    {
        foreach (QuestTabContent qtc in questTabContentParent.GetComponentsInChildren<QuestTabContent>())
        {
            qtc.ClaimReward();
        }
    }

    public void AddDailyQuest(int questIndex)
    {
        // Instantiate FIX!!!
        tempDailyQuest = Instantiate(questTabContentPrefab.gameObject, questTabContentParent.transform) as GameObject;

        // Update index
        tempDailyQuest.GetComponent<QuestTabContent>().UpdateIndex(questIndex);
    }

    public void LoadData(PlayerDataManager data)
    {
        unreadNotificationsDailyQuestPanel = data.unreadNotificationsDailyQuestPanel;
        questTabContent = data.questTabContent;
        UpdateValues();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.unreadNotificationsDailyQuestPanel = unreadNotificationsDailyQuestPanel;
        data.questTabContent = questTabContent;
    }
}

[System.Serializable]
public class QuestContent
{
    public string questContentName;
    public float questContentProgress;
    public Texture2D questContentIcon;
    public Texture2D questContentRewardIcon;
    public int questContentRewardAmount;
    public RewardType_QuestContent rewardType;

    public enum RewardType_QuestContent { Nyangs, Chips, Rubies }
}