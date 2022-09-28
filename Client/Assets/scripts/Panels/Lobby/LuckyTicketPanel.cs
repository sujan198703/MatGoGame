using UnityEngine;
using UnityEngine.UI;
using ScratchCardAsset;

public class LuckyTicketPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] private Text luckyTicketText;
    [SerializeField] private ScratchCardManager scratchCardManager;
    [SerializeField] private GameObject scratchCardImage;

    string[] bigNumbers = { "심만", "백만", "천만", "억"}; // 10,000, 1,000,000, 10,000,000, 100,000,000
    long nyangsPocket;
    int currencyIndex;
    int bigNumberIndex;
    bool rewardAdded;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => GetReward();

    void Update()
    {
        // If greater than 70%, hide scratch card image
        if (scratchCardManager.Progress.GetProgress() >= 0.7f && !rewardAdded)
        {
            scratchCardImage.SetActive(false);
            AddNyang();
        }
    }

    void GetReward()
    {
        // Load nyangs in pocket
        PlayerDataStorageManager.instance.LoadGame();

        // Get currency index
        currencyIndex = Random.Range(10, 100);

        // Get big number index
        bigNumberIndex = Random.Range(0, bigNumbers.Length);

        // Limit to 1 billion
        if (bigNumberIndex == bigNumbers.Length) currencyIndex = 1;

        // Update lucky ticket text
        luckyTicketText.text = "당첨을 축하합니다.\n<size=24>" + 
            currencyIndex + "</size>" + bigNumbers[bigNumberIndex] + "냥";

        // Add to pocket
        PlayerDataStorageManager.instance.SaveGame();
    }

    void AddNyang()
    {
        switch (bigNumberIndex)
        {
            case 0:
                nyangsPocket += int.Parse(currencyIndex.ToString() + "0000"); 
                break;
            case 1:
                nyangsPocket += int.Parse(currencyIndex.ToString() + "000000");
                break;
            case 2:
                nyangsPocket += int.Parse(currencyIndex.ToString() + "0000000");
                break;
            case 3:
                nyangsPocket += int.Parse(currencyIndex.ToString() + "00000000");
                break;
        }

        // Save game
        PlayerDataStorageManager.instance.SaveThenLoad();

        // Break out loop
        rewardAdded = true;
    }

    public void CloseButton()
    {
        this.gameObject.SetActive(false);
        rewardAdded = false;
        PanelManager.instance.lobbyPanel.ResetLuckyTicketTimer();
    }

    public void LoadData(PlayerDataManager data)
    {
        nyangsPocket = data.nyangsPocket;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = nyangsPocket;
    }
}