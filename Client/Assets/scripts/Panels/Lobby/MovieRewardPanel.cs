using UnityEngine;
using UnityEngine.UI;

public class MovieRewardPanel : MonoBehaviour, PlayerDataStorageInterface
{
    public Text nyangAdsWatchedText;
    public Text chipAdsWatchedText;
    public GameObject movieRewardLotteryTicket;

    private int nyangsPocket;
    private int nyangAdsWatched;
    private int chipsPocket;
    private int chipAdsWatched;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    public void RequestAndLoadRewardedAd(int rewardedVideoIndex)
    {
        //AdManager.instance.RequestAndLoadRewardedAd(rewardedVideoIndex);
        //AdManager.instance.ShowRewardedAd(rewardedVideoIndex);
        AddNyang();
        PlayerDataStorageManager.instance.SaveGame();
    }

    public void AddNyang()
    {
        // Update Nyang
        nyangsPocket += 1000;

        // Update Nyang Counter
        if (nyangAdsWatched < 10) nyangAdsWatched++;

        // Update Nyang Text
        nyangAdsWatchedText.text = "오늘 시청 <color=red>" + nyangAdsWatched + "</color>/10회";
    }


    public void AddChip()
    {
        // Update Chip
        chipsPocket += 1000;

        // Update Chip
        if (chipAdsWatched < 10) chipAdsWatched++;

        // Update Chip Text
        chipAdsWatchedText.text = "오늘 시청 <color=red>" + chipAdsWatched + "</color>/10회";
    }

    public void LoadData(PlayerDataManager data)
    {
        nyangsPocket = data.nyangsPocket;
        nyangAdsWatched = data.nyangAdsWatched;
        chipsPocket = data.chipsPocket;
        chipAdsWatched = data.chipAdsWatched;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = nyangsPocket;
        data.nyangAdsWatched = nyangAdsWatched;
        data.chipsPocket = chipsPocket;
        data.chipAdsWatched = chipAdsWatched;
    }
}
