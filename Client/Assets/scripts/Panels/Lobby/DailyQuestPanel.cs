using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Image progressBar;
    [SerializeField] GameObject questTabContentParent;
    [SerializeField] QuestTabContent questTabContentNyangPrefab;
    [SerializeField] QuestTabContent questTabContentChipPrefab;

    //public QuestContent[] questContent;

    private List<QuestTabContent> questTabContent = new List<QuestTabContent>();
    private List<QuestTabContent> questTabContentClaimed = new List<QuestTabContent>();

    private QuestTabContent tempQtc;

    private int unreadNotificationsDailyQuestPanel;

    private bool questTabContentInitialized;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues(); 

    void UpdateValues() 
    {
        PlayerDataStorageManager.instance.LoadGame();

        AssignQuestTabObjects();

        InstantiateObjects();
    }

    // Load the initial objects once
    void AssignQuestTabObjects()
    {
        if (!this.questTabContentInitialized)
        {
            #region Dynamic Quest Tab Content Generation
            //for (int i = 0; i < questContent.Length; i++)
            //{
            //    // Temp object
            //    tempQtc = questTabContentPrefab;

            //    // Name
            //    tempQtc.questContentName.text = questContent[i].questContentName;
            //    // Icon
            //    tempQtc.questContentIcon.sprite = Sprite.Create
            //           (questContent[i].questContentIcon, new Rect(0, 0, questContent[i].questContentIcon.width, questContent[i].questContentIcon.height),
            //           new Vector2(questContent[i].questContentIcon.width / 2, questContent[i].questContentIcon.height / 2));
            //    // Reward Icon
            //    tempQtc.questContentRewardIcon.sprite = Sprite.Create
            //           (questContent[i].questContentRewardIcon, new Rect(0, 0, questContent[i].questContentRewardIcon.width, questContent[i].questContentRewardIcon.height),
            //           new Vector2(questContent[i].questContentRewardIcon.width / 2, questContent[i].questContentRewardIcon.height / 2));
            //    // Reward Name
            //    tempQtc.questContentRewardText.text = questContent[i].questContentRewardAmount.ToString();
            //    // Append reward type suffix
            //    string rewardTypeSuffix = "";
            //    // Reward Type++
            //    switch (questContent[i].rewardType)
            //    {
            //        case QuestContent.RewardType_QuestContent.Nyangs:
            //            rewardTypeSuffix = " 냥";
            //            tempQtc.rewardType = QuestContent.RewardType_QuestContent.Nyangs;
            //            break;
            //        case QuestContent.RewardType_QuestContent.Chips:
            //            rewardTypeSuffix = " 칩";
            //            tempQtc.rewardType = QuestContent.RewardType_QuestContent.Chips;
            //            break;
            //        case QuestContent.RewardType_QuestContent.Rubies:
            //            rewardTypeSuffix = " 루비";
            //            tempQtc.rewardType = QuestContent.RewardType_QuestContent.Rubies;
            //            break;
            //    }
            //    // Reward Type Suffix
            //    tempQtc.questContentRewardText.text += rewardTypeSuffix;
            //    // Amount
            //    tempQtc.rewardAmount = questContent[i].questContentRewardAmount;
            //    // Update unique ID
            //    tempQtc.SetUniqueID();
            //    // Append to list if doesn't exist
            //    questTabContent.Add(tempQtc);
            //    // Increment counter, like this if you add later in game
            //    //unreadNotificationsDailyQuestPanel++;
            //}
            #endregion

            QuestTabContent tempNyangMtc = questTabContentNyangPrefab;
            QuestTabContent tempChipMtc = questTabContentChipPrefab;

            // Update unique ID
            tempNyangMtc.SetUniqueID();
            tempChipMtc.SetUniqueID();

            questTabContent.Add(tempNyangMtc);
            questTabContent.Add(tempChipMtc);

            questTabContentInitialized = true;

            // Reload
            PlayerDataStorageManager.instance.SaveThenLoad();
        }
    }

    // Load files in scene tree from JSON
    public void InstantiateObjects()
    {
        foreach (QuestTabContent qtc in questTabContent)
        {
            // Add to hierarchy
            GameObject tempQuestObject = Instantiate(qtc.gameObject, questTabContentParent.transform);
        }

        foreach (QuestTabContent qtc in questTabContentClaimed)
        {
            // Add to hierarchy
            GameObject tempQuestObject = Instantiate(qtc.gameObject, questTabContentParent.transform);
        }
    }

    public void GetAll()
    {
        foreach (QuestTabContent qtc in questTabContentParent.GetComponentsInChildren<QuestTabContent>())
        {
            if (qtc.IsAvailable()) qtc.ClaimReward();
        }
    }
   
    public void LoadData(PlayerDataManager data)
    {
        questTabContentInitialized = data.questTabContentInitialized;

        questTabContent = data.questTabContent;
        questTabContentClaimed = data.questTabContentClaimed;

        unreadNotificationsDailyQuestPanel = data.unreadNotificationsDailyQuestPanel;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.questTabContentInitialized = questTabContentInitialized;

        data.questTabContent = questTabContent;
        data.questTabContentClaimed = questTabContentClaimed;

        data.unreadNotificationsDailyQuestPanel = unreadNotificationsDailyQuestPanel;
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