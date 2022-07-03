using UnityEngine;
using UnityEngine.UI;
using ScratchCardAsset;

public class LuckyTicketPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] private Text luckyTicketText;
    [SerializeField] private ScratchCardManager scratchCardManager;
    [SerializeField] private GameObject scratchCardImage;

    private string[] bigNumbers = { "심만", "백만", "천만", "억"}; // 10,000, 100,000, 1,000,000, 10,000,000
    private int nyangsPocket;
    private int currencyIndex;
    private int bigNumberIndex;

    void Start() => GetReward();

    void Update()
    {
        // If greater than 70%, hide scratch card image
        if (scratchCardManager.Progress.GetProgress() >= 0.7f)
        {
            scratchCardImage.SetActive(false);
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

        // Update lucky ticket text
        luckyTicketText.text = "당첨을 축하합니다.\n<size=28>" + 
            currencyIndex + "</size>" + bigNumbers[bigNumberIndex] + "냥";

        // Add to pocket
        PlayerDataStorageManager.instance.SaveGame();
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