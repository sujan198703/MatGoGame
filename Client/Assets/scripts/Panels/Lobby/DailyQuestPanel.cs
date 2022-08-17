using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Image progressBar;
    [SerializeField] GameObject questTabContentParent;
    [SerializeField] QuestTabContent questTabContentPrefab;
    [SerializeField] Transform questTabContentTransform;
    public QuestContent[] questContent;
    GameObject tempDailyQuest;
   
    List<QuestTabContent> questTabContent;
    List<QuestTabContent> questTabContentClaimed;
    int unreadNotificationsDailyQuestPanel;

    bool questTabContentObjectsInstantiated;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void OnEnable() => PlayerDataStorageManager.instance.LoadGame(); 

    void UpdateValues() 
    {
        //if (!questTabContentObjectsInstantiated && questTabContentTransform.childCount != questContent.Length)
        //{
        //    for (int i = 0; i < questContent.Length; i++)
        //    {
        //        AddDailyQuest(i);
        //    }

        //    questTabContentObjectsInstantiated = true;
        //}
    }

    public void GetAll()
    {
        foreach (QuestTabContent qtc in questTabContentParent.GetComponentsInChildren<QuestTabContent>())
        {
            if (qtc.IsAvailable()) qtc.ClaimReward();
        }
    }

    public void AddDailyQuest(int questIndex)
    {
        // Instantiate
        tempDailyQuest = Instantiate(questTabContentPrefab.gameObject, questTabContentParent.transform) as GameObject;

        // Update index
        tempDailyQuest.GetComponent<QuestTabContent>().UpdateIndex(questIndex);
    }

    public void LoadData(PlayerDataManager data)
    {
        questTabContent = data.questTabContent;
        questTabContentClaimed = data.questTabContentClaimed;

        unreadNotificationsDailyQuestPanel = data.unreadNotificationsDailyQuestPanel;
        questTabContent = data.questTabContent;
        
        UpdateValues();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.questTabContent = questTabContent;
        data.questTabContentClaimed = questTabContentClaimed;

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