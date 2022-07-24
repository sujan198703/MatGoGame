using UnityEngine;
using UnityEngine.UI;

public class DailyQuestPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject questTabContent;
    [SerializeField] GameObject questTabContentTransform;
    public QuestContent[] questContent;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    public void GetAll()
    {

    }

    public void LoadData(PlayerDataManager data)
    {
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }

}

[System.Serializable]
public class QuestContent
{
    [SerializeField] private string questContentName;
    [SerializeField] private float questContentProgress;
    [SerializeField] private Image questContentImage;
    public int questContentRewardAmount;
    public RewardType_QuestContent rewardType;

    public enum RewardType_QuestContent { Nyangs, Chips, Rubies }
}